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
    public partial class SearchProvider : ISearchProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络工具.</param>
        public SearchProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<List<SearchRecommendItem>> GetHotSearchListAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
                { Query.From, "0" },
                { Query.Limit, "50" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Search.Square, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var resData = await _httpProvider.ParseAsync<ServerResponse<List<SearchSquareItem>>>(response);
            foreach (var item in resData.Data)
            {
                if (item.Type == "trending")
                {
                    return item.Data.List;
                }
            }

            return null;
        }
    }
}
