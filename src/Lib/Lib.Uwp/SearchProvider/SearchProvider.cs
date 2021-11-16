// Copyright (c) Richasy. All rights reserved.

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

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Search.Square, queryParameters, Models.Enums.RequestClientType.IOS);
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

        /// <inheritdoc/>
        public Task<SubModuleSearchResultResponse<ArticleSearchItem>> GetArticleSearchResultAsync(string keyword, string orderType, string partitionId, int pageNumber)
        {
            var parameters = new Dictionary<string, string>
            {
                { Query.FullCategoryId, partitionId },
            };
            return GetSubModuleResultAsync<ArticleSearchItem>(6, keyword, orderType, pageNumber, parameters);
        }

        /// <inheritdoc/>
        public Task<SubModuleSearchResultResponse<PgcSearchItem>> GetBangumiSearchResultAsync(string keyword, string orderType, int pageNumber)
            => GetSubModuleResultAsync<PgcSearchItem>(7, keyword, orderType, pageNumber);

        /// <inheritdoc/>
        public Task<SubModuleSearchResultResponse<PgcSearchItem>> GetMovieSearchResultAsync(string keyword, string orderType, int pageNumber)
            => GetSubModuleResultAsync<PgcSearchItem>(8, keyword, orderType, pageNumber);

        /// <inheritdoc/>
        public Task<SubModuleSearchResultResponse<UserSearchItem>> GetUserSearchResultAsync(string keyword, string orderType, string orderSort, string userType, int pageNumber)
        {
            var parameters = new Dictionary<string, string>
            {
                { Query.OrderSort, orderSort },
                { Query.UserType, userType },
            };
            return GetSubModuleResultAsync<UserSearchItem>(2, keyword, orderType, pageNumber, parameters);
        }

        /// <inheritdoc/>
        public async Task<ComprehensiveSearchResultResponse> GetComprehensiveSearchResultAsync(string keyword, string orderType, string partitionId, string duration, int pageNumber)
        {
            var queryParameters = GetSearchBasicQueryParameters(keyword, orderType, pageNumber);
            queryParameters.Add(Query.Recommend, "1");
            queryParameters.Add(Query.PartitionId, partitionId);
            queryParameters.Add(Query.Duration, duration);
            queryParameters.Add(Query.HighLight, "0");
            queryParameters.Add(Query.IsOrgQuery, "0");
            queryParameters.Add(Query.Device, "phone");
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Search.ComprehensiveSearch, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<ComprehensiveSearchResultResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<LiveSearchResultResponse> GetLiveSearchResultAsync(string keyword, int pageNumber)
        {
            var queryParameters = GetSearchBasicQueryParameters(keyword, string.Empty, pageNumber);
            queryParameters.Add(Query.Type, "4");
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Search.LiveModuleSearch, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveSearchResultResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, SearchSuggestTag>> GetSearchSuggestTagsAsync(string keyword)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Term, keyword },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Search.SearchSuggest, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<Dictionary<string, SearchSuggestTag>>(response);
            return result;
        }
    }
}
