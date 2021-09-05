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
    /// 提供已登录用户相关的数据操作.
    /// </summary>
    public partial class AccountProvider
    {
        private readonly IHttpProvider _httpProvider;

        private async Task<PgcFavoriteListResponse> GetPgcFavoriteListInternalAsync(string requestUrl, int pageNumber)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, "20" },
                { Query.Status, "2" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, requestUrl, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse2<PgcFavoriteListResponse>>(response);
            return result.Result;
        }
    }
}
