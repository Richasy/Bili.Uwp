// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Article;
using Bili.Models.Data.Live;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Search;
using Bili.Models.Data.User;
using Bili.Toolkit.Interfaces;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
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
        /// <param name="articleAdapter">文章数据适配器.</param>
        /// <param name="pgcAdapter">PGC 内容数据适配器.</param>
        /// <param name="liveAdapter">直播数据适配器.</param>
        /// <param name="videoAdapter">视频数据适配器.</param>
        /// <param name="userAdapter">用户数据适配器.</param>
        public SearchProvider(
            IHttpProvider httpProvider,
            ISettingsToolkit settingsToolkit,
            ISearchAdapter searchAdapter,
            IArticleAdapter articleAdapter,
            IPgcAdapter pgcAdapter,
            ILiveAdapter liveAdapter,
            IVideoAdapter videoAdapter,
            IUserAdapter userAdapter)
        {
            _httpProvider = httpProvider;
            _settingsToolkit = settingsToolkit;
            _searchAdapter = searchAdapter;
            _articleAdapter = articleAdapter;
            _liveAdapter = liveAdapter;
            _videoAdapter = videoAdapter;
            _pgcAdapter = pgcAdapter;
            _userAdapter = userAdapter;

            ClearStatus();
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

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Models.App.Constants.ApiConstants.Search.Square, queryParameters, Models.Enums.RequestClientType.IOS);
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

            var request = await _httpProvider.GetRequestMessageAsync(Models.App.Constants.ApiConstants.Search.Suggestion, req);
            var response = await _httpProvider.SendAsync(request, cancellationToken);
            var result = await _httpProvider.ParseAsync(response, Bilibili.App.Interfaces.V1.SuggestionResult3Reply.Parser);
            return !cancellationToken.IsCancellationRequested
                ? result.List.Select(p => _searchAdapter.ConvertToSearchSuggest(p)).ToList()
                : null;
        }

        /// <inheritdoc/>
        public async Task<SearchSet<ArticleInformation>> GetArticleSearchResultAsync(string keyword, string orderType, string partitionId)
        {
            var parameters = new Dictionary<string, string>
            {
                { Query.FullCategoryId, partitionId },
            };
            var data = await GetSubModuleResultAsync<ArticleSearchItem>(6, keyword, orderType, _articlePageNumber, parameters);
            _articlePageNumber++;
            var items = data.ItemList == null
                ? new List<ArticleInformation>()
                : data.ItemList.Select(p => _articleAdapter.ConvertToArticleInformation(p));
            return new SearchSet<ArticleInformation>(items, data.PageNumber < _articlePageNumber);
        }

        /// <inheritdoc/>
        public async Task<SearchSet<SeasonInformation>> GetAnimeSearchResultAsync(string keyword, string orderType)
        {
            var data = await GetSubModuleResultAsync<PgcSearchItem>(7, keyword, orderType, _animePageNumber);
            _animePageNumber++;
            var items = data.ItemList == null
                ? new List<SeasonInformation>()
                : data.ItemList.Select(p => _pgcAdapter.ConvertToSeasonInformation(p)).ToList();
            return new SearchSet<SeasonInformation>(items, data.PageNumber < _animePageNumber);
        }

        /// <inheritdoc/>
        public async Task<SearchSet<SeasonInformation>> GetMovieSearchResultAsync(string keyword, string orderType)
        {
            var data = await GetSubModuleResultAsync<PgcSearchItem>(8, keyword, orderType, _moviePageNumber);
            _moviePageNumber++;
            var items = data.ItemList == null
                ? new List<SeasonInformation>()
                : data.ItemList.Select(p => _pgcAdapter.ConvertToSeasonInformation(p)).ToList();
            return new SearchSet<SeasonInformation>(items, data.PageNumber < _moviePageNumber);
        }

        /// <inheritdoc/>
        public async Task<SearchSet<AccountInformation>> GetUserSearchResultAsync(string keyword, string orderType, string orderSort, string userType)
        {
            var parameters = new Dictionary<string, string>
            {
                { Query.OrderSort, orderSort },
                { Query.UserType, userType },
            };
            var data = await GetSubModuleResultAsync<UserSearchItem>(2, keyword, orderType, _userPageNumber, parameters);
            _userPageNumber++;
            var items = data.ItemList == null
                ? new List<AccountInformation>()
                : data.ItemList.Select(p => _userAdapter.ConvertToAccountInformation(p)).ToList();
            return new SearchSet<AccountInformation>(items, data.PageNumber < _userPageNumber);
        }

        /// <inheritdoc/>
        public async Task<ComprehensiveSet> GetComprehensiveSearchResultAsync(string keyword, string orderType, string partitionId, string duration)
        {
            var queryParameters = GetSearchBasicQueryParameters(keyword, orderType, _comprehensivePageNumber);
            queryParameters.Add(Query.Recommend, "1");
            queryParameters.Add(Query.PartitionId, partitionId);
            queryParameters.Add(Query.Duration, duration);
            queryParameters.Add(Query.HighLight, "0");
            queryParameters.Add(Query.IsOrgQuery, "0");
            queryParameters.Add(Query.Device, "phone");
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Models.App.Constants.ApiConstants.Search.ComprehensiveSearch, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<ComprehensiveSearchResultResponse>>(response);
            var data = _searchAdapter.ConvertToComprehensiveSet(result.Data);
            _comprehensivePageNumber++;
            return data;
        }

        /// <inheritdoc/>
        public async Task<SearchSet<LiveInformation>> GetLiveSearchResultAsync(string keyword)
        {
            var queryParameters = GetSearchBasicQueryParameters(keyword, string.Empty, _livePageNumber);
            queryParameters.Add(Query.Type, "4");
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Models.App.Constants.ApiConstants.Search.LiveModuleSearch, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveSearchResultResponse>>(response);
            _livePageNumber++;
            var items = result.Data.RoomResult?.Items == null
                ? new List<LiveInformation>()
                : result.Data.RoomResult.Items.Select(p => _liveAdapter.ConvertToLiveInformation(p));
            return new SearchSet<LiveInformation>(items, result.Data.PageNumber < _livePageNumber);
        }

        /// <inheritdoc/>
        public void ResetComprehensiveStatus()
            => _comprehensivePageNumber = 1;

        /// <inheritdoc/>
        public void ResetAnimeStatus()
            => _animePageNumber = 1;

        /// <inheritdoc/>
        public void ResetMovieStatus()
            => _moviePageNumber = 1;

        /// <inheritdoc/>
        public void ResetUserStatus()
            => _userPageNumber = 1;

        /// <inheritdoc/>
        public void ResetArticleStatus()
            => _articlePageNumber = 1;

        /// <inheritdoc/>
        public void ResetLiveStatus()
            => _livePageNumber = 1;

        /// <inheritdoc/>
        public void ClearStatus()
        {
            ResetComprehensiveStatus();
            ResetAnimeStatus();
            ResetArticleStatus();
            ResetLiveStatus();
            ResetMovieStatus();
            ResetUserStatus();
        }
    }
}
