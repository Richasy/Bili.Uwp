﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.BiliBili;
using Bili.Models.Data.Video;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bilibili.App.View.V1;
using Bilibili.Community.Service.Dm.V1;
using Newtonsoft.Json;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
{
    /// <summary>
    /// 提供视频操作.
    /// </summary>
    public partial class PlayerProvider : IPlayerProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        /// <param name="accountProvider">账户操作工具.</param>
        /// <param name="videoToolkit">视频工具.</param>
        /// <param name="videoAdapter">视频数据适配器.</param>
        public PlayerProvider(
            IHttpProvider httpProvider,
            IAccountProvider accountProvider,
            IVideoToolkit videoToolkit,
            IVideoAdapter videoAdapter)
        {
            _httpProvider = httpProvider;
            _accountProvider = accountProvider;
            _videoToolkit = videoToolkit;
            _videoAdapter = videoAdapter;
        }

        /// <inheritdoc/>
        public async Task<VideoView> GetVideoDetailAsync(string videoId)
        {
            var type = _videoToolkit.GetVideoIdType(videoId, out var avId);
            var viewRequest = new ViewReq();
            if (type == Models.Enums.VideoIdType.Av && !string.IsNullOrEmpty(avId))
            {
                viewRequest.Aid = Convert.ToInt64(avId);
            }
            else if (type == Models.Enums.VideoIdType.Bv)
            {
                viewRequest.Bvid = videoId;
            }

            var request = await _httpProvider.GetRequestMessageAsync(Video.Detail, viewRequest);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, ViewReply.Parser);
            return _videoAdapter.ConvertToVideoView(data);
        }

        /// <inheritdoc/>
        public async Task<string> GetOnlineViewerCountAsync(long videoId, long partId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.Device, "phone" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Video.OnlineViewerCount, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<OnlineViewerResponse>>(response);
            return data.Data != null ? data.Data.Data.DisplayText : "--";
        }

        /// <inheritdoc/>
        public async Task<PlayerInformation> GetDashAsync(long videoId, long partId)
            => await InternalGetDashAsync(partId.ToString(), videoId.ToString());

        /// <inheritdoc/>
        public async Task<PlayerInformation> GetDashAsync(int partId, int episodeId, int seasonType, string proxy = "", string area = "")
            => await InternalGetDashAsync(partId.ToString(), string.Empty, seasonType.ToString(), proxy, area, episodeId.ToString());

        /// <inheritdoc/>
        public async Task<DmViewReply> GetDanmakuMetaDataAsync(long videoId, long partId)
        {
            var req = new DmViewReq()
            {
                Pid = videoId,
                Oid = partId,
                Type = 1,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Video.DanmakuMetaData, req);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, DmViewReply.Parser);
            return result;
        }

        /// <inheritdoc/>
        public async Task<DmSegMobileReply> GetSegmentDanmakuAsync(long videoId, long partId, int segmentIndex)
        {
            var req = new DmSegMobileReq
            {
                Pid = videoId,
                Oid = partId,
                SegmentIndex = segmentIndex,
                Type = 1,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Video.SegmentDanmaku, req);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, DmSegMobileReply.Parser);
            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> ReportProgressAsync(long videoId, long partId, long progress)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.Progress, progress.ToString() },
                { Query.Type, "3" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.ProgressReport, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.Code == 0;
        }

        /// <inheritdoc/>
        public async Task<bool> ReportProgressAsync(long videoId, long partId, int episodeId, int seasonId, long progress)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.EpisodeIdSlim, episodeId.ToString() },
                { Query.SeasonIdSlim, seasonId.ToString() },
                { Query.RealTime, progress.ToString() },
                { Query.Progress, progress.ToString() },
                { Query.Type, "4" },
                { Query.SubType, "1" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.ProgressReport, queryParameters, Models.Enums.RequestClientType.Android, true);
            var response = await _httpProvider.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.Code == 0;
        }

        /// <inheritdoc/>
        public async Task<bool> LikeAsync(long videoId, bool isLike)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Like, isLike ? "0" : "1" },
            };

            try
            {
                var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.Like, queryParameters, needToken: true);
                var response = await _httpProvider.SendAsync(request, GetExpiryToken());
                var result = await _httpProvider.ParseAsync<ServerResponse>(response);
                return result.Code == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<CoinResult> CoinAsync(long videoId, int number, bool alsoLike)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Multiply, number.ToString() },
                { Query.AlsoLike, alsoLike ? "1" : "0" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.Coin, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request, GetExpiryToken());
            var result = await _httpProvider.ParseAsync<ServerResponse<CoinResult>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<FavoriteResult> FavoriteAsync(long videoId, IList<string> needAddFavoriteList, IList<string> needRemoveFavoriteList)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PartitionId, videoId.ToString() },
                { Query.Type, "2" },
            };

            if (needAddFavoriteList?.Any() ?? false)
            {
                queryParameters.Add(Query.AddFavoriteIds, string.Join(",", needAddFavoriteList));
            }

            if (needRemoveFavoriteList?.Any() ?? false)
            {
                queryParameters.Add(Query.DeleteFavoriteIds, string.Join(",", needRemoveFavoriteList));
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.ModifyFavorite, queryParameters, Models.Enums.RequestClientType.IOS, true);
            try
            {
                var response = await _httpProvider.SendAsync(request, GetExpiryToken());
            }
            catch (ServiceException ex)
            {
                var result = (FavoriteResult)ex.Error.Code;
                return result;
            }

            return FavoriteResult.Success;
        }

        /// <inheritdoc/>
        public async Task<TripleResult> TripleAsync(long videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.Triple, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request, GetExpiryToken());
            var result = await _httpProvider.ParseAsync<ServerResponse<TripleResult>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<bool> SendDanmakuAsync(string content, int videoId, int partId, int progress, string color, bool isStandardSize, DanmakuLocation location)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Type, "1" },
                { Query.Oid, partId.ToString() },
                { Query.MessageSlim, content },
                { Query.Progress, progress.ToString() },
                { Query.Color, color },
                { Query.FontSize, isStandardSize ? "25" : "18" },
                { Query.Mode, ((int)location).ToString() },
                { Query.Rnd, DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds().ToString() },
            };

            try
            {
                var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.SendDanmaku, queryParameters, needToken: true);
                var response = await _httpProvider.SendAsync(request);
                var result = await _httpProvider.ParseAsync<ServerResponse>(response);
                return result.IsSuccess();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<SubtitleIndexResponse> GetSubtitleIndexAsync(long videoId, int partId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Id, $"cid:{partId}" },
                { Query.Aid, videoId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Video.Subtitle, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var text = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(text) && text.Contains("subtitle"))
            {
                var json = Regex.Match(text, @"<subtitle>(.*?)</subtitle>").Groups[1].Value;
                var index = JsonConvert.DeserializeObject<SubtitleIndexResponse>(json);
                return index;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<SubtitleDetailResponse> GetSubtitleDetailAsync(string url)
        {
            if (!url.StartsWith("http"))
            {
                url = "https:" + url;
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, url);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<SubtitleDetailResponse>(response);
            return result;
        }

        /// <inheritdoc/>
        public async Task<InteractionEdgeResponse> GetInteractionEdgeAsync(long videoId, string graphVersion, long edgeId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.GraphVersion, graphVersion },
                { Query.EdgeId, edgeId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Video.InteractionEdge, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<InteractionEdgeResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<VideoStatusInfo> GetVideoStatusAsync(long videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Video.Stat, queryParameters);
            var response = await _httpProvider.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token);
            var result = await _httpProvider.ParseAsync<ServerResponse<VideoStatusInfo>>(response);
            return result.Data;
        }
    }
}