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
    /// 搜索工具.
    /// </summary>
    public partial class SearchProvider
    {
        private readonly IHttpProvider _httpProvider;

        private Dictionary<string, string> GetSearchBasicQueryParameters(string keyword, string orderType, int pageNumber)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Keyword, keyword },
                { Query.Order, orderType },
                { Query.Fnval, "976" },
                { Query.Fnver, "0" },
                { Query.Fourk, "1" },
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSize, "20" },
                { Query.Qn, "112" },
            };

            return queryParameters;
        }

        private async Task<SubModuleSearchResultResponse<T>> GetSubModuleResultAsync<T>(int typeId, string keyword, string orderType, int pageNumber)
        {
            var queryParameters = GetSearchBasicQueryParameters(keyword, orderType, pageNumber);
            queryParameters.Add(Query.Type, typeId.ToString());
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Search.SubModuleSearch, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<SubModuleSearchResultResponse<T>>>(response);
            return result.Data;
        }
    }
}
