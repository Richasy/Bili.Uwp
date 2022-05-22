﻿// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bilibili.App.Dynamic.V2;

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
        /// 将 PGC 模块条目 <see cref="PgcModuleItem"/> 转换为单集信息.
        /// </summary>
        /// <param name="item">PGC 模块条目.</param>
        /// <returns><see cref="EpisodeInformation"/>.</returns>
        EpisodeInformation ConvertToEpisodeInformation(PgcModuleItem item);

        /// <summary>
        /// 将 PGC 动态条目 <see cref="MdlDynPGC"/> 转换为单集信息.
        /// </summary>
        /// <param name="pgc">PGC 动态条目.</param>
        /// <returns><see cref="EpisodeInformation"/>.</returns>
        EpisodeInformation ConvertToEpisodeInformation(MdlDynPGC pgc);

        /// <summary>
        /// 将 PGC 模块条目 <see cref="PgcModuleItem"/> 转换为剧集信息.
        /// </summary>
        /// <param name="item">PGC 模块条目.</param>
        /// <param name="type">PGC 内容类型.</param>
        /// <returns><see cref="SeasonInformation"/>.</returns>
        SeasonInformation ConvertToSeasonInformation(PgcModuleItem item, PgcType type);

        /// <summary>
        /// 将 PGC 搜索条目 <see cref="PgcSearchItem"/> 转换为剧集信息.
        /// </summary>
        /// <param name="item">PGC 搜索条目.</param>
        /// <returns><see cref="SeasonInformation"/>.</returns>
        SeasonInformation ConvertToSeasonInformation(PgcSearchItem item);

        /// <summary>
        /// 将 PGC 索引条目 <see cref="PgcIndexItem"/> 转换为剧集信息.
        /// </summary>
        /// <param name="item">PGC 索引条目.</param>
        /// <returns><see cref="SeasonInformation"/>.</returns>
        SeasonInformation ConvertToSeasonInformation(PgcIndexItem item);

        /// <summary>
        /// 将时间线剧集 <see cref="TimeLineEpisode"/> 转换为剧集信息.
        /// </summary>
        /// <param name="item">时间线剧集.</param>
        /// <returns><see cref="SeasonInformation"/>.</returns>
        SeasonInformation ConvertToSeasonInformation(TimeLineEpisode item);

        /// <summary>
        /// 将 PGC 播放清单的剧集条目 <see cref="PgcPlayListSeason"/> 转换为剧集信息.
        /// </summary>
        /// <param name="season">PGC 播放清单条目.</param>
        /// <returns><see cref="SeasonInformation"/>.</returns>
        SeasonInformation ConvertToSeasonInformation(PgcPlayListSeason season);

        /// <summary>
        /// 将收藏的 PGC 条目 <see cref="FavoritePgcItem"/> 转换为剧集信息.
        /// </summary>
        /// <param name="item">收藏的 PGC 条目.</param>
        /// <returns><see cref="SeasonInformation"/>.</returns>
        SeasonInformation ConvertToSeasonInformation(FavoritePgcItem item);

        /// <summary>
        /// 将 PGC 展示信息 <see cref="PgcDisplayInformation"/> 封装成视图信息.
        /// </summary>
        /// <param name="display">PGC 展示信息.</param>
        /// <returns><see cref="PgcView"/>.</returns>
        PgcView ConvertToPgcView(PgcDisplayInformation display);
    }
}