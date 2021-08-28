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
        /// <returns>内容详情.</returns>
        Task<PgcResponse> GetPageDetailAsync(int tabId);

        /// <summary>
        /// 获取PGC页面详情.
        /// </summary>
        /// <param name="type">类型.</param>
        /// <param name="cursor">偏移指针.</param>
        /// <returns>内容详情.</returns>
        Task<PgcResponse> GetPageDetailAsync(PgcType type, string cursor);

        /// <summary>
        /// 获取全区动态.
        /// </summary>
        /// <param name="partitionId">分区Id.</param>
        /// <param name="offsetId">偏移值.</param>
        /// <returns><see cref="SubPartition"/>.</returns>
        Task<SubPartition> GetPartitionRecommendVideoAsync(int partitionId, int offsetId = 0);

        /// <summary>
        /// 获取PGC内容的详细信息.
        /// </summary>
        /// <param name="episodeId">(可选项) 单集Id.</param>
        /// <param name="seasonId">(可选项) 剧集/系列Id.</param>
        /// <returns>PGC内容详情.</returns>
        Task<PgcDisplayInformation> GetDisplayInformationAsync(int episodeId = 0, int seasonId = 0);

        /// <summary>
        /// 获取分集的交互信息，包括用户的投币/点赞/收藏.
        /// </summary>
        /// <param name="episodeId">分集Id.</param>
        /// <returns>交互信息.</returns>
        Task<EpisodeInteraction> GetEpisodeInteractionAsync(int episodeId);
    }
}
