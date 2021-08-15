// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
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
            return await _searchProvider.GetHotSearchListAsync();
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

                        VideoSearchIteration?.Invoke(this, new VideoSearchEventArgs(videoData, pageNumber));
                    }
                    catch (System.Exception)
                    {
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
                        BangumiSearchIteration?.Invoke(this, new PgcSearchEventArgs(bangumiData, pageNumber, keyword));
                    }
                    catch (System.Exception)
                    {
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
                        MovieSearchIteration?.Invoke(this, new PgcSearchEventArgs(movieData, pageNumber, keyword));
                    }
                    catch (System.Exception)
                    {
                        if (pageNumber == 1)
                        {
                            throw;
                        }
                    }

                    break;

                // case SearchModuleType.Live:
                //     var liveData = await _searchProvider.GetLiveSearchResultAsync(keyword, orderType, pageNumber);
                //     break;
                // case SearchModuleType.User:
                //     var userData = await _searchProvider.GetUserSearchResultAsync(keyword, orderType, pageNumber);
                //     break;

                // case SearchModuleType.Article:
                //     var articleData = await _searchProvider.GetArticleSearchResultAsync(keyword, orderType, pageNumber);
                //     break;
                default:
                    break;
            }
        }
    }
}
