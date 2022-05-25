// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.BiliBili;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;

namespace Bili.Lib.Interfaces
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
        Task<IEnumerable<Models.Data.Community.Partition>> GetAnimeTabsAsync(PgcType type);

        /// <summary>
        /// 获取导航标签所指向的内容详情.
        /// </summary>
        /// <param name="tabId">标签Id.</param>
        /// <returns>内容详情.</returns>
        Task<PgcPageView> GetPageDetailAsync(string tabId);

        /// <summary>
        /// 获取PGC页面详情.
        /// </summary>
        /// <param name="type">类型.</param>
        /// <returns>内容详情.</returns>
        Task<PgcPageView> GetPageDetailAsync(PgcType type);

        /// <summary>
        /// 获取PGC内容的详细信息.
        /// </summary>
        /// <param name="episodeId">(可选项) 单集Id.</param>
        /// <param name="seasonId">(可选项) 剧集/系列Id.</param>
        /// <param name="proxy">代理地址.</param>
        /// <param name="area">地区.</param>
        /// <returns>PGC内容详情.</returns>
        Task<PgcDisplayInformation> GetDisplayInformationAsync(int episodeId = 0, int seasonId = 0, string proxy = "", string area = "");

        /// <summary>
        /// 获取分集的交互信息，包括用户的投币/点赞/收藏.
        /// </summary>
        /// <param name="episodeId">分集Id.</param>
        /// <returns>交互信息.</returns>
        Task<EpisodeInteraction> GetEpisodeInteractionAsync(int episodeId);

        /// <summary>
        /// 追番/追剧.
        /// </summary>
        /// <param name="seasonId">剧Id.</param>
        /// <param name="isFollow">是否关注.</param>
        /// <returns>关注结果.</returns>
        Task<bool> FollowAsync(int seasonId, bool isFollow);

        /// <summary>
        /// 获取PGC索引条件.
        /// </summary>
        /// <param name="type">PGC类型.</param>
        /// <returns>PGC索引条件响应.</returns>
        Task<IEnumerable<Filter>> GetPgcIndexFiltersAsync(PgcType type);

        /// <summary>
        /// 获取PGC索引结果.
        /// </summary>
        /// <param name="type">类型.</param>
        /// <param name="queryParameters">查询参数.</param>
        /// <returns>PGC索引结果响应.</returns>
        Task<(bool IsFinished, IEnumerable<SeasonInformation> Items)> GetPgcIndexResultAsync(PgcType type, Dictionary<string, string> queryParameters);

        /// <summary>
        /// 获取PGC内容发布时间线.
        /// </summary>
        /// <param name="type">类型.</param>
        /// <returns>时间轴响应结果.</returns>
        Task<TimelineView> GetPgcTimelinesAsync(PgcType type);

        /// <summary>
        /// 获取播放列表详情.
        /// </summary>
        /// <param name="listId">播放列表Id.</param>
        /// <returns>播放列表响应结果.</returns>
        Task<PgcPlaylist> GetPgcPlaylistAsync(string listId);

        /// <summary>
        /// 重置PGC页面请求的状态.
        /// </summary>
        /// <param name="type">PGC类型.</param>
        void ResetPageStatus(PgcType type);

        /// <summary>
        /// 重置索引页面请求状态.
        /// </summary>
        void ResetIndexStatus();
    }
}
