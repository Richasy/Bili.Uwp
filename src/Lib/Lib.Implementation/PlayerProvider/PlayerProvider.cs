// Copyright (c) Richasy. All rights reserved.

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
using Bili.Models.Data.Community;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Playeronline.V1;
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
        public PlayerProvider(
            IHttpProvider httpProvider,
            IAccountProvider accountProvider,
            IVideoToolkit videoToolkit,
            ISettingsToolkit settingsToolkit,
            IVideoAdapter videoAdapter,
            ICommunityAdapter communityAdapter,
            IPlayerAdapter playerAdapter,
            IPgcAdapter pgcAdapter)
        {
            _httpProvider = httpProvider;
            _accountProvider = accountProvider;
            _videoToolkit = videoToolkit;
            _settingsToolkit = settingsToolkit;
            _videoAdapter = videoAdapter;
            _pgcAdapter = pgcAdapter;
            _communityAdapter = communityAdapter;
            _playerAdapter = playerAdapter;
        }

        /// <inheritdoc/>
        public async Task<VideoPlayerView> GetVideoDetailAsync(string videoId)
        {
            var type = _videoToolkit.GetVideoIdType(videoId, out var avId);
            var viewRequest = new ViewReq();
            if (type == VideoIdType.Av && !string.IsNullOrEmpty(avId))
            {
                viewRequest.Aid = Convert.ToInt64(avId);
            }
            else if (type == VideoIdType.Bv)
            {
                viewRequest.Bvid = videoId;
            }

            var request = await _httpProvider.GetRequestMessageAsync(Video.Detail, viewRequest);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, ViewReply.Parser);
            return _videoAdapter.ConvertToVideoView(data);
        }

        /// <inheritdoc/>
        public async Task<PgcPlayerView> GetPgcDetailAsync(string episodeId, string seasonId, string proxy = "", string area = "")
        {
            var queryParameters = GetPgcDetailInformationQueryParameters(int.Parse(episodeId), int.Parse(seasonId), area);
            var otherQuery = string.Empty;

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Models.App.Constants.ApiConstants.Pgc.SeasonDetail(proxy), queryParameters, RequestClientType.IOS, additionalQuery: otherQuery);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PgcDisplayInformation>>(response);
            return _pgcAdapter.ConvertToPgcPlayerView(data.Data);
        }

        /// <inheritdoc/>
        public async Task<EpisodeInteractionInformation> GetEpisodeInteractionInformationAsync(string episodeId)
        {
            var queryParameters = GetEpisodeInteractionQueryParameters(episodeId);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Models.App.Constants.ApiConstants.Pgc.EpisodeInteraction, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<EpisodeInteraction>>(response);
            return _communityAdapter.ConvertToEpisodeInteractionInformation(data.Data);
        }

        /// <inheritdoc/>
        public async Task<string> GetOnlineViewerCountAsync(string videoId, string partId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.Device, "phone" },
            };

            var req = new PlayerOnlineReq
            {
                Aid = Convert.ToInt64(videoId),
                Cid = Convert.ToInt64(partId),
                PlayOpen = true,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Video.OnlineViewerCount, req);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, PlayerOnlineReply.Parser);
            return data.TotalText ?? "--";
        }

        /// <inheritdoc/>
        public async Task<MediaInformation> GetVideoMediaInformationAsync(string videoId, string partId)
            => await InternalGetDashAsync(partId, videoId);

        /// <inheritdoc/>
        public async Task<MediaInformation> GetPgcMediaInformationAsync(string partId, string episodeId, string seasonType, string proxy = default, string area = default)
            => await InternalGetDashAsync(partId, string.Empty, seasonType, proxy, area, episodeId);

        /// <inheritdoc/>
        public async Task<IEnumerable<DanmakuInformation>> GetSegmentDanmakuAsync(string videoId, string partId, int segmentIndex)
        {
            var req = new DmSegMobileReq
            {
                Pid = Convert.ToInt64(videoId),
                Oid = Convert.ToInt64(partId),
                SegmentIndex = segmentIndex,
                Type = 1,
            };
            var request = await _httpProvider.GetRequestMessageAsync(Video.SegmentDanmaku, req);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, DmSegMobileReply.Parser);
            return result.Elems.Select(p => _playerAdapter.ConvertToDanmakuInformation(p)).ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> ReportProgressAsync(string videoId, string partId, double progress)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.Progress, Convert.ToInt32(progress).ToString() },
                { Query.Type, "3" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.ProgressReport, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.Code == 0;
        }

        /// <inheritdoc/>
        public async Task<bool> ReportProgressAsync(string videoId, string partId, string episodeId, string seasonId, double progress)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.EpisodeIdSlim, episodeId.ToString() },
                { Query.SeasonIdSlim, seasonId.ToString() },
                { Query.Progress, Convert.ToInt32(progress).ToString() },
                { Query.Type, "4" },
                { Query.SubType, "1" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.ProgressReport, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.Code == 0;
        }

        /// <inheritdoc/>
        public async Task<bool> LikeAsync(string videoId, bool isLike)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId },
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
        public async Task<bool> CoinAsync(string videoId, int number, bool alsoLike)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId },
                { Query.Multiply, number.ToString() },
                { Query.AlsoLike, alsoLike ? "1" : "0" },
            };

            try
            {
                var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.Coin, queryParameters, needToken: true);
                var response = await _httpProvider.SendAsync(request, GetExpiryToken());
                var result = await _httpProvider.ParseAsync<ServerResponse<CoinResult>>(response);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<FavoriteResult> FavoriteAsync(string videoId, IEnumerable<string> needAddFavoriteList, IEnumerable<string> needRemoveFavoriteList, bool isVideo)
        {
            var resourceId = isVideo ? $"{videoId}:2" : $"{videoId}:24";
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Resources, resourceId },
            };

            if (needAddFavoriteList?.Any() ?? false)
            {
                queryParameters.Add(Query.AddFavoriteIds, string.Join(",", needAddFavoriteList));
            }

            if (needRemoveFavoriteList?.Any() ?? false)
            {
                queryParameters.Add(Query.DeleteFavoriteIds, string.Join(",", needRemoveFavoriteList));
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.ModifyFavorite, queryParameters, RequestClientType.IOS, true);
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
        public async Task<TripleInformation> TripleAsync(string videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.Triple, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request, GetExpiryToken());
            var result = await _httpProvider.ParseAsync<ServerResponse<TripleResult>>(response);
            return _communityAdapter.ConvertToTripleInformation(result.Data, videoId);
        }

        /// <inheritdoc/>
        public async Task<bool> SendDanmakuAsync(string content, string videoId, string partId, int progress, string color, bool isStandardSize, DanmakuLocation location)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId },
                { Query.Type, "1" },
                { Query.Oid, partId },
                { Query.MessageSlim, content },
                { Query.Progress, (progress * 1000).ToString() },
                { Query.Color, color },
                { Query.FontSize, isStandardSize ? "25" : "18" },
                { Query.Mode, ((int)location).ToString() },
                { Query.Rnd, DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds().ToString() },
            };

            try
            {
                var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Video.SendDanmaku, queryParameters, RequestClientType.IOS, needToken: true);
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
        public async Task<IEnumerable<SubtitleMeta>> GetSubtitleIndexAsync(string videoId, string partId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Id, $"cid:{partId}" },
                { Query.Aid, videoId },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Video.Subtitle, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var text = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(text) && text.Contains("subtitle"))
            {
                var json = Regex.Match(text, @"<subtitle>(.*?)</subtitle>").Groups[1].Value;
                var index = JsonConvert.DeserializeObject<SubtitleIndexResponse>(json);
                return index.Subtitles.Select(p => _playerAdapter.ConvertToSubtitleMeta(p)).ToList();
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SubtitleInformation>> GetSubtitleDetailAsync(string url)
        {
            if (!url.StartsWith("http"))
            {
                url = "https:" + url;
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, url, type: RequestClientType.IOS, needCookie: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<SubtitleDetailResponse>(response);
            return result.Body.Select(p => _playerAdapter.ConvertToSubtitleInformation(p)).ToList();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<InteractionInformation>> GetInteractionInformationsAsync(string videoId, string graphVersion, string edgeId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId },
                { Query.GraphVersion, graphVersion },
                { Query.EdgeId, edgeId },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Video.InteractionEdge, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<InteractionEdgeResponse>>(response);
            if (result.Data?.Edges?.Questions?.Any() ?? false)
            {
                var choices = result.Data.Edges.Questions.First().Choices;
                return choices.Select(p => _playerAdapter.ConvertToInteractionInformation(p, result.Data.HiddenVariables)).ToList();
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<VideoCommunityInformation> GetVideoCommunityInformationAsync(string videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Video.Stat, queryParameters);
            var response = await _httpProvider.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token);
            var result = await _httpProvider.ParseAsync<ServerResponse<VideoStatusInfo>>(response);
            return _communityAdapter.ConvertToVideoCommunityInformation(result.Data);
        }
    }
}
