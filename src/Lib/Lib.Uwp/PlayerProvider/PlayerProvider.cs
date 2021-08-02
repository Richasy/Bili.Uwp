// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Bilibili.Community.Service.Dm.V1;
using Newtonsoft.Json.Linq;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
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

            var request = await _httpProvider.GetRequestMessageAsync(Api.Video.Detail, viewRequest);
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

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Video.OnlineViewerCount, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<OnlineViewerResponse>>(response);
            return data.Data.Data.DisplayText;
        }

        /// <inheritdoc/>
        public async Task<PlayerDashInformation> GetDashAsync(long videoId, long partId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Fnver, "0" },
                { Query.AVid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.Fourk, "1" },
                { Query.Fnval, "16" },
                { Query.Qn, "64" },
                { Query.OType, "json" },
            };

            if (_accountProvider.UserId != 0)
            {
                queryParameters.Add(Query.MyId, _accountProvider.UserId.ToString());
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Video.PlayInformation, queryParameters, Models.Enums.RequestClientType.Web);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PlayerDashInformation>, ServerResponse2<PlayerDashInformation>>(response, (str) =>
              {
                  var jobj = JObject.Parse(str);
                  return jobj.ContainsKey("data");
              });

            if (data is ServerResponse<PlayerDashInformation> res1)
            {
                return res1.Data;
            }
            else if (data is ServerResponse2<PlayerDashInformation> res2)
            {
                return res2.Result;
            }

            return null;
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

            var request = await _httpProvider.GetRequestMessageAsync(Api.Video.DanmakuMetaData, req);
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

            var request = await _httpProvider.GetRequestMessageAsync(Api.Video.SegmentDanmaku, req);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, DmSegMobileReply.Parser);
            return result;
        }
    }
}
