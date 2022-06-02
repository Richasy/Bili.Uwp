// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 搜索工具.
    /// </summary>
    public partial class SearchProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly ISearchAdapter _searchAdapter;

        private Dictionary<string, string> GetSearchBasicQueryParameters(string keyword, string orderType, int pageNumber)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Keyword, Uri.EscapeDataString(keyword) },
                { Query.Order, orderType },
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, "20" },
            };

            return queryParameters;
        }

        private async Task<SubModuleSearchResultResponse<T>> GetSubModuleResultAsync<T>(int typeId, string keyword, string orderType, int pageNumber, Dictionary<string, string> additionalParameters = null)
        {
            var proxy = string.Empty;
            var isOpenRoaming = _settingsToolkit.ReadLocalSetting(SettingNames.IsOpenRoaming, false);
            var localProxy = _settingsToolkit.ReadLocalSetting(SettingNames.RoamingSearchAddress, string.Empty);
            if (isOpenRoaming && !string.IsNullOrEmpty(localProxy))
            {
                proxy = localProxy;
            }

            var queryParameters = GetSearchBasicQueryParameters(keyword, orderType, pageNumber);
            queryParameters.Add(Query.Type, typeId.ToString());
            if (additionalParameters != null && additionalParameters.Count > 0)
            {
                foreach (var item in additionalParameters)
                {
                    queryParameters.Add(item.Key, item.Value);
                }
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Search.SubModuleSearch(proxy), queryParameters, RequestClientType.IOS, additionalQuery: "area=hk");
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<SubModuleSearchResultResponse<T>>>(response);
            return result.Data;
        }
    }
}
