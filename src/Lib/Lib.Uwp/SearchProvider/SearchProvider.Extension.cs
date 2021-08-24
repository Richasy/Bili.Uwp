// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;

using static Richasy.Bili.Models.App.Constants.ApiConstants;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 搜索工具.
    /// </summary>
    public partial class SearchProvider
    {
        private readonly IHttpProvider _httpProvider;

        private Dictionary<string, string> GetSearchBasicQueryParameters(string keyword, string orderType, int pageNumber)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Keyword, Uri.EscapeDataString(keyword) },
                { Query.Order, orderType },
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSize, "20" },
            };

            return queryParameters;
        }

        private async Task<SubModuleSearchResultResponse<T>> GetSubModuleResultAsync<T>(int typeId, string keyword, string orderType, int pageNumber, Dictionary<string, string> additionalParameters = null)
        {
            var queryParameters = GetSearchBasicQueryParameters(keyword, orderType, pageNumber);
            queryParameters.Add(Query.Type, typeId.ToString());
            if (additionalParameters != null && additionalParameters.Count > 0)
            {
                foreach (var item in additionalParameters)
                {
                    queryParameters.Add(item.Key, item.Value);
                }
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Search.SubModuleSearch, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<SubModuleSearchResultResponse<T>>>(response);
            return result.Data;
        }
    }
}
