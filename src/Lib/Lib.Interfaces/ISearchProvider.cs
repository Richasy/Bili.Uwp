﻿// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bili.Models.BiliBili;
using Bili.Models.Data.Search;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 搜索操作.
    /// </summary>
    public interface ISearchProvider
    {
        /// <summary>
        /// 获取热搜列表.
        /// </summary>
        /// <returns>热搜推荐列表.</returns>
        Task<IEnumerable<SearchSuggest>> GetHotSearchListAsync();

        /// <summary>
        /// 获取搜索建议.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="cancellationToken">异步中止令牌.</param>
        /// <returns>搜索建议列表.</returns>
        Task<IEnumerable<SearchSuggest>> GetSearchSuggestion(string keyword, CancellationToken cancellationToken);

        /// <summary>
        /// 获取综合搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="partitionId">分区筛选.</param>
        /// <param name="duration">时长筛选.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>综合搜索结果.</returns>
        Task<ComprehensiveSearchResultResponse> GetComprehensiveSearchResultAsync(string keyword, string orderType, string partitionId, string duration, int pageNumber);

        /// <summary>
        /// 获取番剧搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>响应结果.</returns>
        Task<SubModuleSearchResultResponse<PgcSearchItem>> GetBangumiSearchResultAsync(string keyword, string orderType, int pageNumber);

        /// <summary>
        /// 获取电影电视剧搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>响应结果.</returns>
        Task<SubModuleSearchResultResponse<PgcSearchItem>> GetMovieSearchResultAsync(string keyword, string orderType, int pageNumber);

        /// <summary>
        /// 获取用户搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="orderSort">排序规则.</param>
        /// <param name="userType">用户类型.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>用户搜索结果.</returns>
        Task<SubModuleSearchResultResponse<UserSearchItem>> GetUserSearchResultAsync(string keyword, string orderType, string orderSort, string userType, int pageNumber);

        /// <summary>
        /// 获取文章搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="partitionId">分区Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>文章搜索结果.</returns>
        Task<SubModuleSearchResultResponse<ArticleSearchItem>> GetArticleSearchResultAsync(string keyword, string orderType, string partitionId, int pageNumber);

        /// <summary>
        /// 获取直播间搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>直播搜索结果.</returns>
        Task<LiveSearchResultResponse> GetLiveSearchResultAsync(string keyword, int pageNumber);
    }
}
