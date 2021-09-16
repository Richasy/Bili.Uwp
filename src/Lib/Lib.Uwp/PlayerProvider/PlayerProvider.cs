// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.Models.Enums.Bili;

using static Richasy.Bili.Models.App.Constants.ApiConstants;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
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
        public PlayerProvider(IHttpProvider httpProvider, IAccountProvider accountProvider)
        {
            _httpProvider = httpProvider;
            _accountProvider = accountProvider;
        }

        /// <inheritdoc/>
        public async Task<ViewReply> GetVideoDetailAsync(long videoId)
        {
            var viewRequest = new ViewReq()
            {
                Aid = videoId,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Video.Detail, viewRequest);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, ViewReply.Parser);
            return data;
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
        public async Task<PlayerDashInformation> GetDashAsync(long videoId, long partId)
        {
            return await InternalGetDashAsync(partId.ToString(), videoId.ToString());
        }

        /// <inheritdoc/>
        public async Task<PlayerDashInformation> GetDashAsync(int partId, int seasonType)
        {
            return await InternalGetDashAsync(partId.ToString(), seasonType: seasonType.ToString());
        }

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
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.ProgressReport, queryParameters, Models.Enums.RequestClientType.IOS, true);
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
        public async Task<FavoriteResult> FavoriteAsync(long videoId, IList<int> needAddFavoriteList, IList<int> needRemoveFavoriteList)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PartitionId, videoId.ToString() },
                { Query.Type, "2" },
            };

            if (needAddFavoriteList?.Any() ?? false)
            {
                queryParameters.Add(Query.AddFavoriteIds, string.Join(',', needAddFavoriteList));
            }

            if (needRemoveFavoriteList?.Any() ?? false)
            {
                queryParameters.Add(Query.DeleteFavoriteIds, string.Join(',', needAddFavoriteList));
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
    }
}
