// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bili.Models.Data.Article;
using Bili.Models.Data.Live;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Search;
using Bili.Models.Data.User;

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
        /// <returns>综合搜索结果.</returns>
        Task<ComprehensiveSet> GetComprehensiveSearchResultAsync(string keyword, string orderType, string partitionId, string duration);

        /// <summary>
        /// 获取动漫搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <returns>响应结果.</returns>
        Task<SearchSet<SeasonInformation>> GetAnimeSearchResultAsync(string keyword, string orderType);

        /// <summary>
        /// 获取电影电视剧搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <returns>响应结果.</returns>
        Task<SearchSet<SeasonInformation>> GetMovieSearchResultAsync(string keyword, string orderType);

        /// <summary>
        /// 获取用户搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="orderSort">排序规则.</param>
        /// <param name="userType">用户类型.</param>
        /// <returns>用户搜索结果.</returns>
        Task<SearchSet<AccountInformation>> GetUserSearchResultAsync(string keyword, string orderType, string orderSort, string userType);

        /// <summary>
        /// 获取文章搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="orderType">排序方式.</param>
        /// <param name="partitionId">分区Id.</param>
        /// <returns>文章搜索结果.</returns>
        Task<SearchSet<ArticleInformation>> GetArticleSearchResultAsync(string keyword, string orderType, string partitionId);

        /// <summary>
        /// 获取直播间搜索结果.
        /// </summary>
        /// <param name="keyword">搜索关键词.</param>
        /// <returns>直播搜索结果.</returns>
        Task<SearchSet<LiveInformation>> GetLiveSearchResultAsync(string keyword);

        /// <summary>
        /// 重置综合搜索的请求状态.
        /// </summary>
        void ResetComprehensiveStatus();

        /// <summary>
        /// 重置动漫搜索请求的状态.
        /// </summary>
        void ResetAnimeStatus();

        /// <summary>
        /// 重置电影搜索请求的状态.
        /// </summary>
        void ResetMovieStatus();

        /// <summary>
        /// 重置用户搜索请求的状态.
        /// </summary>
        void ResetUserStatus();

        /// <summary>
        /// 重置文章搜索请求的状态.
        /// </summary>
        void ResetArticleStatus();

        /// <summary>
        /// 重置直播搜索请求状态.
        /// </summary>
        void ResetLiveStatus();

        /// <summary>
        /// 清空所有状态.
        /// </summary>
        void ClearStatus();
    }
}
