// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供直播相关的数据操作.
    /// </summary>
    public partial class LiveProvider : ILiveProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络请求处理工具.</param>
        public LiveProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<LiveFeedResponse> GetLiveFeedsAsync(int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Page, page.ToString() },
                { Query.RelationPage, page.ToString() },
                { Query.Scale, "2" },
                { Query.LoginEvent, "1" },
                { Query.Device, "phone" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Live.LiveFeed, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveFeedResponse>>(response);

            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<LivePlayInformation> GetLivePlayInformationAsync(int roomId)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.Cid, roomId.ToString() },
                { Query.Qn, "0" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Live.PlayInformation, queryParameter, RequestClientType.Web);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LivePlayInformation>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<LiveRoomDetail> GetLiveRoomDetailAsync(int roomId)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.RoomId, roomId.ToString() },
                { Query.Device, "phone" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Live.RoomDetail, queryParameter, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveRoomDetail>>(response);
            return result.Data;
        }
    }
}
