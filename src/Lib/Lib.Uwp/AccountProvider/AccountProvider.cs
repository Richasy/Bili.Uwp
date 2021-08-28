// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bilibili.App.Interfaces.V1;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using static Richasy.Bili.Models.App.Constants.ApiConstants;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供已登录用户的数据操作.
    /// </summary>
    public partial class AccountProvider : IAccountProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        public AccountProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public int UserId { get; private set; }

        /// <inheritdoc/>
        public async Task<MyInfo> GetMyInformationAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.MyInfo, type: Models.Enums.RequestClientType.IOS, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<MyInfo>>(response);
            UserId = result.Data.Mid;
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<CursorV2Reply> GetMyHistorySetAsync(string tabSign, Cursor cursor)
        {
            var req = new CursorV2Req
            {
                Business = tabSign,
                Cursor = cursor,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Account.HistoryCursor, req, true);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, CursorV2Reply.Parser);
            return data;
        }

        /// <inheritdoc/>
        public async Task<HistoryTabReply> GetMyHistoryTabsAsync()
        {
            var req = new HistoryTabReq
            {
                Source = HistorySource.HistoryValue,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Account.HistoryTabs, req, true);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, HistoryTabReply.Parser);
            return data;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveHistoryItemAsync(string tabSign, long itemId)
        {
            var req = new DeleteReq
            {
                HisInfo = new HisInfo
                {
                    Business = tabSign,
                    Kid = itemId,
                },
            };

            var request = await _httpProvider.GetRequestMessageAsync(Account.DeleteHistoryItem, req, true);
            _ = await _httpProvider.SendAsync(request);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> ClearHistoryAsync(string tabSign)
        {
            var req = new ClearReq() { Business = tabSign };
            var request = await _httpProvider.GetRequestMessageAsync(Account.ClearHistory, req, true);
            _ = await _httpProvider.SendAsync(request);
            return true;
        }

        /// <inheritdoc/>
        public async Task<UserSpaceResponse> GetUserSpaceInformationAsync(int userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Space, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserSpaceResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<UserSpaceVideoSet> GetUserSpaceVideoSetAsync(int userId, string offsetId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
                { Query.Aid, offsetId },
                { Query.Order, "pubdate" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.VideoCursor, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserSpaceVideoSet>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<bool> ModifyUserRelationAsync(int userId, bool isFollow)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Fid, userId.ToString() },
                { Query.ReSrc, "21" },
                { Query.Action, isFollow ? "1" : "2" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ModifyRelation, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);

            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<Mine> GetMyDataAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Mine, type: Models.Enums.RequestClientType.IOS, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<Mine>>(response);
            UserId = result.Data.Mid;
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<RelatedUserResponse> GetFansAsync(int userId, int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
                { Query.Page, page.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Fans, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<RelatedUserResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<RelatedUserResponse> GetFollowsAsync(int userId, int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
                { Query.PageNumber, page.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Follows, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<RelatedUserResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<ViewLaterResponse> GetViewLaterListAsync(int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, page.ToString() },
                { Query.PageSize, "40" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.ViewLaterList, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<ViewLaterResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<bool> ClearViewLaterAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterClear, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> AddVideoToViewLaterAsync(int videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterAdd, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveVideoFromViewLaterAsync(params int[] videoIds)
        {
            var ids = string.Join(',', videoIds);
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, ids },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterDelete, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<FavoriteListResponse> GetFavoriteListAsync(int userId, int videoId = 0)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UpId, userId.ToString() },
                { Query.Type, "2" },
                { Query.PartitionId, videoId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.FavoriteList, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<FavoriteListResponse>>(response);
            return result.Data;
        }
    }
}
