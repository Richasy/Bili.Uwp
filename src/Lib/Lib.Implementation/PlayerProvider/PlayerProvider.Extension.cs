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
using Bili.Toolkit.Interfaces;
using Newtonsoft.Json.Linq;
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
        private readonly IVideoAdapter _videoAdapter;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IPlayerAdapter _playerAdapter;

        private CancellationToken GetExpiryToken(int seconds = 5)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
            return source.Token;
        }

        private async Task<MediaInformation> InternalGetDashAsync(string cid, string aid = "", string seasonType = "", string proxy = "", string area = "", string episodeId = "")
        {
            var isPgc = string.IsNullOrEmpty(aid) && !string.IsNullOrEmpty(seasonType);

            var url = isPgc ? ApiConstants.Pgc.PlayInformation(proxy) : ApiConstants.Video.PlayInformation;

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

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, url, queryParameters, Models.Enums.RequestClientType.Web, additionalQuery: otherQuery);
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
    }
}
