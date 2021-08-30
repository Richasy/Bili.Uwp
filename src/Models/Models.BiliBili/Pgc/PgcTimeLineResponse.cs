// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// PGC时间线响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcTimeLineResponse
    {
        /// <summary>
        /// 副标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "current_time_text", Required = Required.Default)]
        public string Subtitle { get; set; }

        /// <summary>
        /// 标签页Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "data", Required = Required.Default)]
        public List<PgcTimeLineItem> Data { get; set; }

        /// <summary>
        /// 导航标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "navigation_title", Required = Required.Default)]
        public string Title { get; set; }
    }

    /// <summary>
    /// 时间轴条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcTimeLineItem
    {
        /// <summary>
        /// 日期.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "date", Required = Required.Default)]
        public string Date { get; set; }

        /// <summary>
        /// 日期时间戳.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "date_ts", Required = Required.Default)]
        public int DateTimeStamp { get; set; }

        /// <summary>
        /// 周几.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "day_of_week", Required = Required.Default)]
        public int DayOfWeek { get; set; }

        /// <summary>
        /// 占位符文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "day_update_text", Required = Required.Default)]
        public string HolderText { get; set; }

        /// <summary>
        /// 标签页Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "episodes", Required = Required.Default)]
        public List<TimeLineEpisode> Episodes { get; set; }

        /// <summary>
        /// 是否为今天，0-不是，1-是.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_today", Required = Required.Default)]
        public int IsToday { get; set; }
    }

    /// <summary>
    /// 时间轴剧集信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TimeLineEpisode
    {
        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 分集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "episode_id", Required = Required.Default)]
        public int EpisodeId { get; set; }

        /// <summary>
        /// 是否关注，0-不关注，1-关注.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow", Required = Required.Default)]
        public int IsFollow { get; set; }

        /// <summary>
        /// 发布到第几集.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pub_index", Required = Required.Default)]
        public string PublishIndex { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pub_time", Required = Required.Default)]
        public string PublishTime { get; set; }

        /// <summary>
        /// 发布时间戳.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pub_ts", Required = Required.Default)]
        public int PublishTimeStamp { get; set; }

        /// <summary>
        /// 是否已经发布.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "published", Required = Required.Default)]
        public int IsPublished { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public int SeasonId { get; set; }

        /// <summary>
        /// 剧集类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_type", Required = Required.Default)]
        public int SeasonType { get; set; }

        /// <summary>
        /// 矩形封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "square_cover", Required = Required.Default)]
        public string SqureCover { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string Url { get; set; }
    }
}
