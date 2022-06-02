// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Search;
using Bili.Toolkit.Interfaces;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
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
        /// <param name="settingsToolkit">设置工具.</param>
        /// <param name="searchAdapter">搜索数据适配器.</param>
        public SearchProvider(
            IHttpProvider httpProvider,
            ISettingsToolkit settingsToolkit,
            ISearchAdapter searchAdapter)
        {
            _httpProvider = httpProvider;
            _settingsToolkit = settingsToolkit;
            _searchAdapter = searchAdapter;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SearchSuggest>> GetHotSearchListAsync()
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
            var list = resData.Data.Where(p => p.Type == "trending")
                .SelectMany(p => p.Data.List)
                .Select(p => _searchAdapter.ConvertToSearchSuggest(p));
            return list;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SearchSuggest>> GetSearchSuggestion(string keyword, CancellationToken cancellationToken)
        {
            var req = new Bilibili.App.Interfaces.V1.SuggestionResult3Req()
            {
                Keyword = keyword,
                Highlight = 0,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Search.Suggestion, req);
            var response = await _httpProvider.SendAsync(request, cancellationToken);
            var result = await _httpProvider.ParseAsync(response, Bilibili.App.Interfaces.V1.SuggestionResult3Reply.Parser);
            return !cancellationToken.IsCancellationRequested
                ? result.List.Select(p => _searchAdapter.ConvertToSearchSuggest(p)).ToList()
                : (IEnumerable<SearchSuggest>)null;
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
    }
}
