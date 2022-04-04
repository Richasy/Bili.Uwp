// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;
using static Richasy.Bili.Models.App.Constants.ApiConstants;
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
        /// <param name="accountProvider">账户工具.</param>
        public LiveProvider(IHttpProvider httpProvider, IAccountProvider accountProvider)
        {
            _httpProvider = httpProvider;
            _accountProvider = accountProvider;
        }

        /// <inheritdoc/>
        public async Task<bool> EnterLiveRoomAsync(int roomId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.RoomId, roomId.ToString() },
                { Query.ActionKey, Query.AppKey },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Live.EnterRoom, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse>(response);
            return data.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<LiveAreaDetailResponse> GetLiveAreaDetailAsync(int areaId, int parentId, string sortType, int pageNumber, int pageSize = 40)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Page, pageNumber.ToString() },
                { Query.PageSizeUnderline, pageSize.ToString() },
                { Query.AreaId, areaId.ToString() },
                { Query.ParentAreaId, parentId.ToString() },
                { Query.Device, "phone" },
            };

            if (!string.IsNullOrEmpty(sortType))
            {
                queryParameters.Add(Query.SortType, sortType);
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.AreaDetail, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveAreaDetailResponse>>(response);

            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<LiveAreaResponse> GetLiveAreaIndexAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.LiveArea, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveAreaResponse>>(response);

            return result.Data;
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
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.LiveFeed, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveFeedResponse>>(response);

            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<LivePlayInformation> GetLivePlayInformationAsync(int roomId, int quality)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.RoomId, roomId.ToString() },
                { Query.PlayUrl, "1" },
                { Query.Qn, quality.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.PlayInformation, queryParameter, RequestClientType.Web);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LivePlayUrlResponse>>(response);
            return result.Data.Information;
        }

        /// <inheritdoc/>
        public async Task<LiveAppPlayUrlInfo> GetAppLivePlayInformation(int roomId, int quality, bool audioOnly)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.RoomId, roomId.ToString() },
                { Query.NoPlayUrl, "0" },
                { Query.Qn, quality.ToString() },
                { Query.Codec, Uri.EscapeDataString("0,1") },
                { Query.Device, "phone" },
                { Query.DeviceName, Uri.EscapeDataString("iPhone 13") },
                { Query.Dolby, "1" },
                { Query.Format, Uri.EscapeDataString("0,2") },
                { Query.Http, "1" },
                { Query.OnlyAudio, audioOnly ? "1" : "0" },
                { Query.OnlyVideo, "0" },
                { Query.Protocol, Uri.EscapeDataString("0,1") },
                { Query.NeedHdr, "0" },
                { Query.Mask, "0" },
                { Query.PlayType, "0" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.AppPlayInformation, queryParameter, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveAppPlayInformation>>(response);
            return result.Data.PlayUrlInfo;
        }

        /// <inheritdoc/>
        public async Task<LiveRoomDetail> GetLiveRoomDetailAsync(int roomId)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.RoomId, roomId.ToString() },
                { Query.Device, "phone" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.RoomDetail, queryParameter, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveRoomDetail>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<bool> SendMessageAsync(int roomId, string message, string color, bool isStandardSize, DanmakuLocation location)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.Cid, roomId.ToString() },
                { Query.MyId, _accountProvider.UserId.ToString() },
                { Query.MessageSlim, message },
                { Query.Rnd, DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds().ToString() },
                { Query.Mode, ((int)location).ToString() },
                { Query.Pool, "0" },
                { Query.Type, "json" },
                { Query.Color, color },
                { Query.FontSize, isStandardSize ? "25" : "18" },
                { Query.PlayTime, "0.0" },
            };

            try
            {
                var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Live.SendMessage, queryParameter, needToken: true);
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
