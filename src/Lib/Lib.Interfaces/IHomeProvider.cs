// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Models.Enums;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 分区，标签相关的数据操作定义.
    /// </summary>
    public interface IHomeProvider
    {
        /// <summary>
        /// 请求推荐视频列表.
        /// </summary>
        /// <returns>推荐视频或番剧的列表.</returns>
        /// <remarks>
        /// 视频推荐请求返回的是一个信息流，每请求一次返回一个固定数量的集合。
        /// 除第一次请求外，后一次请求依赖前一次请求的偏移值，以避免出现重复的视频.
        /// 视频推荐服务内置偏移值管理，如果要重置偏移值（比如需要刷新），可以调用 <c>Reset</c> 方法重置.
        /// </remarks>
        Task<IEnumerable<IVideoBase>> RequestRecommendVideosAsync();

        /// <summary>
        /// 获取热门详情.
        /// </summary>
        /// <returns>热门视频信息.</returns>
        Task<IEnumerable<VideoInformation>> RequestPopularVideosAsync();

        /// <summary>
        /// 获取视频分区索引.
        /// </summary>
        /// <returns>全部分区索引，但不包括需要网页显示的分区.</returns>
        Task<IEnumerable<Partition>> GetVideoPartitionIndexAsync();

        /// <summary>
        /// 获取视频子分区数据.
        /// </summary>
        /// <param name="subPartitionId">子分区Id.</param>
        /// <param name="isRecommend">是否是推荐子分区.</param>
        /// <param name="sortType">排序方式.</param>
        /// <returns>返回的子分区数据.</returns>
        Task<VideoPartitionView> GetVideoSubPartitionDataAsync(
            string subPartitionId,
            bool isRecommend,
            VideoSortType sortType = VideoSortType.Default);

        /// <summary>
        /// 获取排行榜详情.
        /// </summary>
        /// <param name="partitionId">分区Id. 如果是全区则为0.</param>
        /// <returns>排行榜信息.</returns>
        Task<IEnumerable<VideoInformation>> GetRankDetailAsync(string partitionId);

        /// <summary>
        /// 重置子分区请求状态，将偏移值归零.
        /// </summary>
        void ResetSubPartitionState();

        /// <summary>
        /// 重置分区请求状态，将缓存和偏移值归零.
        /// </summary>
        void ClearPartitionState();

        /// <summary>
        /// 重置推荐列表状态，将偏移值归零.
        /// </summary>
        void ResetRecommendState();

        /// <summary>
        /// 重置热门视频状态，将偏移值归零.
        /// </summary>
        void ResetPopularState();
    }
}
