// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

using static Richasy.Bili.Models.App.Constants.ControllerConstants.Search;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的搜索部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 获取热搜列表.
        /// </summary>
        /// <returns>热搜列表.</returns>
        public async Task<List<SearchRecommendItem>> GetHotSearchListAsync()
        {
            try
            {
                return await _searchProvider.GetHotSearchListAsync();
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 获取搜索建议.
        /// </summary>
        /// <param name="keyword">关键字.</param>
        /// <returns>关键字列表.</returns>
        public async Task<List<SearchSuggestTag>> GetSearchSuggestTagsAsync(string keyword)
        {
            try
            {
                var dict = await _searchProvider.GetSearchSuggestTagsAsync(keyword);
                return dict.Select(a => a.Value).ToList();
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 请求搜索模块数据.
        /// </summary>
        /// <param name="type">模块类型.</param>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="pairs">附加的查询参数.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestSearchModuleDataAsync(SearchModuleType type, string keyword, int pageNumber, Dictionary<string, string> pairs)
        {
            switch (type)
            {
                case SearchModuleType.Video:
                    try
                    {
                        var v_orderType = pairs[OrderType];
                        var v_partitionId = pairs[PartitionId];
                        var v_duration = pairs[Duration];
                        var videoData = await _searchProvider.GetComprehensiveSearchResultAsync(keyword, v_orderType, v_partitionId, v_duration, pageNumber);
                        if (pageNumber == 1)
                        {
                            SearchMetaChanged?.Invoke(this, new SearchMetaEventArgs(videoData));
                        }

                        VideoSearchIteration?.Invoke(this, new VideoSearchIterationEventArgs(videoData, pageNumber));
                    }
                    catch (System.Exception ex)
                    {
                        _loggerModule.LogError(ex, pageNumber > 1);
                        if (pageNumber == 1)
                        {
                            throw;
                        }
                    }

                    break;

                case SearchModuleType.Bangumi:
                    try
                    {
                        var bangumiData = await _searchProvider.GetBangumiSearchResultAsync(keyword, TotalRank, pageNumber);
                        BangumiSearchIteration?.Invoke(this, new PgcSearchIterationEventArgs(bangumiData, pageNumber, keyword));
                    }
                    catch (System.Exception ex)
                    {
                        _loggerModule.LogError(ex, pageNumber > 1);
                        if (pageNumber == 1)
                        {
                            throw;
                        }
                    }

                    break;
                case SearchModuleType.Movie:
                    try
                    {
                        var movieData = await _searchProvider.GetMovieSearchResultAsync(keyword, TotalRank, pageNumber);
                        MovieSearchIteration?.Invoke(this, new PgcSearchIterationEventArgs(movieData, pageNumber, keyword));
                    }
                    catch (System.Exception ex)
                    {
                        _loggerModule.LogError(ex, pageNumber > 1);
                        if (pageNumber == 1)
                        {
                            throw;
                        }
                    }

                    break;
                case SearchModuleType.Live:
                    try
                    {
                        var liveData = await _searchProvider.GetLiveSearchResultAsync(keyword, pageNumber);
                        LiveSearchIteration?.Invoke(this, new LiveSearchIterationEventArgs(liveData, keyword, pageNumber));
                    }
                    catch (System.Exception ex)
                    {
                        _loggerModule.LogError(ex, pageNumber > 1);
                        if (pageNumber == 1)
                        {
                            throw;
                        }
                    }

                    break;
                case SearchModuleType.User:
                    try
                    {
                        var u_orderType = pairs[OrderType];
                        var u_orderSort = pairs[OrderSort];
                        var u_userType = pairs[UserType];
                        var userData = await _searchProvider.GetUserSearchResultAsync(keyword, u_orderType, u_orderSort, u_userType, pageNumber);
                        UserSearchIteration?.Invoke(this, new UserSearchIterationEventArgs(userData, pageNumber, keyword));
                    }
                    catch (System.Exception ex)
                    {
                        _loggerModule.LogError(ex, pageNumber > 1);
                        if (pageNumber == 1)
                        {
                            throw;
                        }
                    }

                    break;
                case SearchModuleType.Article:
                    try
                    {
                        var a_orderType = pairs[OrderType];
                        var a_partitionId = pairs[PartitionId];
                        var articleData = await _searchProvider.GetArticleSearchResultAsync(keyword, a_orderType, a_partitionId, pageNumber);
                        ArticleSearchIteration?.Invoke(this, new ArticleSearchIterationEventArgs(articleData, pageNumber, keyword));
                    }
                    catch (System.Exception ex)
                    {
                        _loggerModule.LogError(ex, pageNumber > 1);
                        if (pageNumber == 1)
                        {
                            throw;
                        }
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
