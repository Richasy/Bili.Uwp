// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// PGC收藏夹响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcFavoriteListResponse
    {
        /// <summary>
        /// 剧集类型名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow_list", Required = Required.Default)]
        public List<FavoritePgcItem> FollowList { get; set; }

        /// <summary>
        /// 是否还有更多.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "has_next", Required = Required.Default)]
        public int HasMore { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total", Required = Required.Default)]
        public int Total { get; set; }
    }

    /// <summary>
    /// PGC收藏夹条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoritePgcItem
    {
        /// <summary>
        /// 所属地区.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "areas", Required = Required.Default)]
        public List<PgcArea> Areas { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 是否正在追.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow", Required = Required.Default)]
        public int IsFollow { get; set; }

        /// <summary>
        /// 是否已完结.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_finish", Required = Required.Default)]
        public int IsFinish { get; set; }

        /// <summary>
        /// 收藏时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mtime", Required = Required.Default)]
        public int CollectTime { get; set; }

        /// <summary>
        /// 最新章节.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "new_ep", Required = Required.Default)]
        public PgcEpisode NewEpisode { get; set; }

        /// <summary>
        /// 播放历史记录.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "progress", Required = Required.Default)]
        public PgcProgress Progress { get; set; }

        /// <summary>
        /// 剧集的季Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public int SeasonId { get; set; }

        /// <summary>
        /// 剧集类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_type", Required = Required.Default)]
        public string SeasonType { get; set; }

        /// <summary>
        /// 剧集类型名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_type_name", Required = Required.Default)]
        public string SeasonTypeName { get; set; }

        /// <summary>
        /// 剧集类型名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "series", Required = Required.Default)]
        public PgcSeries Series { get; set; }

        /// <summary>
        /// 剧集类型名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "square_cover", Required = Required.Default)]
        public string SquareCover { get; set; }

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
