// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
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
        Task<List<SearchRecommendItem>> GetHotSearchListAsync();

        /// <summary>
        /// 获取综合搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>综合搜索结果.</returns>
        Task<ComprehensiveSearchResultResponse> GetComprehensiveSearchResultAsync(string keyword, string orderType, int pageNumber);

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
        /// <param name="pageNumber">页码.</param>
        /// <returns>用户搜索结果.</returns>
        Task<SubModuleSearchResultResponse<UserSearchItem>> GetUserSearchResultAsync(string keyword, string orderType, int pageNumber);

        /// <summary>
        /// 获取文章搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>文章搜索结果.</returns>
        Task<SubModuleSearchResultResponse<ArticleSearchItem>> GetArticleSearchResultAsync(string keyword, string orderType, int pageNumber);

        /// <summary>
        /// 获取直播间搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>直播搜索结果.</returns>
        Task<LiveSearchResultResponse> GetLiveSearchResultAsync(string keyword, string orderType, int pageNumber);
    }
}
