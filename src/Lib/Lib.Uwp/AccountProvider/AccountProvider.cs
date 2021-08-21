// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
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
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Account.MyInfo, type: Models.Enums.RequestClientType.IOS, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<MyInfo>>(response);
            UserId = result.Data.Mid;
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<UserSpaceResponse> GetUserSpaceInformationAsync(int userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Account.Space, queryParameters);
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

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Account.VideoCursor, queryParameters);
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

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Api.Account.ModifyRelation, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);

            return result.IsSuccess();
        }
    }
}
