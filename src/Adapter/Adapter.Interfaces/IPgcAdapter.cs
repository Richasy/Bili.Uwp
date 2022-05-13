// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Pgc;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// PGC 内容适配器接口.
    /// </summary>
    public interface IPgcAdapter
    {
        /// <summary>
        /// 将单集详情 <see cref="PgcEpisodeDetail"/> 转换为单集信息.
        /// </summary>
        /// <param name="episode">单集详情.</param>
        /// <returns><see cref="EpisodeInformation"/>.</returns>
        EpisodeInformation ConvertToEpisodeInformation(PgcEpisodeDetail episode);

        /// <summary>
        /// 将推荐视频卡片 <see cref="RecommendCard"/> 转换为单集信息.
        /// </summary>
        /// <param name="card">推荐卡片.</param>
        /// <returns><see cref="EpisodeInformation"/>.</returns>
        EpisodeInformation ConvertToEpisodeInformation(RecommendCard card);

        /// <summary>
        /// 将 PGC 展示信息 <see cref="PgcDisplayInformation"/> 转换为剧集信息.
        /// </summary>
        /// <param name="display">PGC 展示信息.</param>
        /// <returns><see cref="SeasonInformation"/>.</returns>
        SeasonInformation ConvertToSeasonInformation(PgcDisplayInformation display);
    }
}
