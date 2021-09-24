// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// PGC播放列表响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcPlayListResponse
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 剧集列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "seasons", Required = Required.Default)]
        public List<PgcPlayListSeason> Seasons { get; set; }

        /// <summary>
        /// 说明文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "summary", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 播放列表标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total", Required = Required.Default)]
        public string Total { get; set; }
    }

    /// <summary>
    /// PGC播放列表剧集条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcPlayListSeason
    {
        /// <summary>
        /// 演员.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "actors", Required = Required.Default)]
        public string Actors { get; set; }

        /// <summary>
        /// 模块子项列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 简介.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "evaluate", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 媒体链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "media_id", Required = Required.Default)]
        public int MediaId { get; set; }

        /// <summary>
        /// 最新剧集.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "new_ep", Required = Required.Default)]
        public PgcEpisode NewEpisode { get; set; }

        /// <summary>
        /// 评分.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rating", Required = Required.Default)]
        public PgcRating Rating { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "seasonId", Required = Required.Default)]
        public int SeasonId { get; set; }

        /// <summary>
        /// 剧集类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "seasonType", Required = Required.Default)]
        public int SeasonType { get; set; }

        /// <summary>
        /// 用户交互信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stat", Required = Required.Default)]
        public PgcPlayListItemStat Stat { get; set; }

        /// <summary>
        /// 标签.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "styles", Required = Required.Default)]
        public string Styles { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subtitle", Required = Required.Default)]
        public string Subtitle { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }
    }

    /// <summary>
    /// PGC播放列表条目用户交互信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcPlayListItemStat
    {
        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmakus", Required = Required.Default)]
        public int DanmakuCount { get; set; }

        /// <summary>
        /// 收藏数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favorites", Required = Required.Default)]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// 播放数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "views", Required = Required.Default)]
        public int PlayCount { get; set; }
    }
}
