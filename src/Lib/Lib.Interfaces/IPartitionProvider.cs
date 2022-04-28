// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.BiliBili;
using Bili.Models.Enums;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 分区，标签相关的数据操作定义.
    /// </summary>
    public interface IPartitionProvider
    {
        /// <summary>
        /// 获取分区索引.
        /// </summary>
        /// <returns>全部分区索引，但不包括需要网页显示的分区.</returns>
        Task<IEnumerable<Partition>> GetPartitionIndexAsync();

        /// <summary>
        /// 获取子分区数据.
        /// </summary>
        /// <param name="subPartitionId">子分区Id.</param>
        /// <param name="isRecommend">是否是推荐子分区.</param>
        /// <param name="offsetId">偏移Id.</param>
        /// <param name="sortType">排序方式.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>返回的子分区数据.</returns>
        Task<SubPartition> GetSubPartitionDataAsync(
            int subPartitionId,
            bool isRecommend,
            int offsetId = 0,
            VideoSortType sortType = VideoSortType.Default,
            int pageNumber = 1);
    }
}
