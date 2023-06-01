// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Playurl.V1;
using Newtonsoft.Json.Linq;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
{
    /// <summary>
    /// 提供视频相关操作.
    /// </summary>
    public partial class PlayerProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IVideoToolkit _videoToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IVideoAdapter _videoAdapter;
        private readonly IPgcAdapter _pgcAdapter;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IPlayerAdapter _playerAdapter;

        private CancellationToken GetExpiryToken(int seconds = 5)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
            return source.Token;
        }

        private Dictionary<string, string> GetEpisodeInteractionQueryParameters(string episodeId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.EpisodeId, episodeId },
            };

            return queryParameters;
        }

        private Dictionary<string, string> GetPgcDetailInformationQueryParameters(int episodeId, int seasonId, string area)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.AutoPlay, "0" },
                { Query.IsShowAllSeries, "0" },
            };

            if (!string.IsNullOrEmpty(area))
            {
                queryParameters.Add(Query.Area, area);
            }

            if (episodeId > 0)
            {
                queryParameters.Add(Query.EpisodeId, episodeId.ToString());
            }

            if (seasonId > 0)
            {
                queryParameters.Add(Query.SeasonId, seasonId.ToString());
            }

            return queryParameters;
        }

        private async Task<MediaInformation> InternalGetDashAsync(string cid, string aid = "", string seasonType = "", string proxy = "", string area = "", string episodeId = "")
        {
            var isPgc = string.IsNullOrEmpty(aid) && !string.IsNullOrEmpty(seasonType);

            var url = isPgc ? ApiConstants.Pgc.PlayInformation(proxy) : ApiConstants.Video.PlayInformation;
            var requestType = isPgc ? RequestClientType.Web : RequestClientType.IOS;

            var queryParameters = new Dictionary<string, string>
            {
                { Query.Fnver, "0" },
                { Query.Cid, cid.ToString() },
                { Query.Fourk, "1" },
                { Query.Fnval, "4048" },
                { Query.Qn, "64" },
                { Query.OType, "json" },
            };

            if (isPgc)
            {
                queryParameters.Add(Query.Module, "bangumi");
                queryParameters.Add(Query.SeasonType, seasonType);
                queryParameters.Add(Query.EpisodeId, episodeId);
            }
            else
            {
                queryParameters.Add(Query.AVid, aid);
            }

            if (_accountProvider.UserId != 0)
            {
                queryParameters.Add(Query.MyId, _accountProvider.UserId.ToString());
            }

            var otherQuery = string.Empty;
            if (!string.IsNullOrEmpty(area))
            {
                otherQuery = $"area={area}";
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, url, queryParameters, requestType, additionalQuery: otherQuery);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PlayerInformation>, ServerResponse2<PlayerInformation>>(response, (str) =>
            {
                var jobj = JObject.Parse(str);
                return jobj.ContainsKey("data");
            });

            if (data is ServerResponse<PlayerInformation> res1)
            {
                return _playerAdapter.ConvertToMediaInformation(res1.Data);
            }
            else if (data is ServerResponse2<PlayerInformation> res2)
            {
                return _playerAdapter.ConvertToMediaInformation(res2.Result);
            }

            return null;
        }

        private async Task<MediaInformation> InternalGetDashFromgRPCAsync(string videoId, string partId)
        {
            var preferCodec = _settingsToolkit.ReadLocalSetting(SettingNames.PreferCodec, PreferCodec.H264);
            var codeType = preferCodec switch
            {
                PreferCodec.H265 => CodeType.Code265,
                PreferCodec.H264 => CodeType.Code264,
                PreferCodec.Av1 => CodeType.Codeav1,
                _ => CodeType.Code264,
            };

            var playUrlReq = new PlayViewReq
            {
                Aid = Convert.ToInt64(videoId),
                Cid = Convert.ToInt64(partId),
                Fourk = true,
                Fnval = 4048,
                Qn = 64,
                ForceHost = 2,
                PreferCodecType = codeType,
            };
            var appReq = await _httpProvider.GetRequestMessageAsync(Video.PlayUrl, playUrlReq);
            var appRes = await _httpProvider.SendAsync(appReq);
            var reply = await _httpProvider.ParseAsync(appRes, PlayViewReply.Parser);
            return _playerAdapter.ConvertToMediaInformation(reply);
        }
    }
}
