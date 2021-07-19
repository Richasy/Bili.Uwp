// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供动漫的数据处理.
    /// </summary>
    public interface IPgcProvider
    {
        /// <summary>
        /// 获取顶部导航（过滤掉网页标签）.
        /// </summary>
        /// <param name="type">动漫类型.</param>
        /// <returns>顶部导航列表.</returns>
        Task<List<PgcTab>> GetTabAsync(PgcType type);

        /// <summary>
        /// 获取导航标签所指向的内容详情.
        /// </summary>
        /// <param name="tabId">标签Id.</param>
        /// <param name="cursor">标识符指针（用于增量加载）.</param>
        /// <returns>内容详情.</returns>
        Task<PgcResponse> GetPageDetailAsync(int tabId, string cursor = null);

        /// <summary>
        /// 获取全区动态.
        /// </summary>
        /// <param name="partitionId">分区Id.</param>
        /// <param name="offsetId">偏移值.</param>
        /// <returns><see cref="SubPartition"/>.</returns>
        Task<SubPartition> GetPartitionRecommendVideoAsync(int partitionId, int offsetId = 0);
    }
}
