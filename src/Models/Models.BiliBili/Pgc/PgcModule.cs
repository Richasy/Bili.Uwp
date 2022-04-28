// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 动漫及影视模块.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcModule
    {
        /// <summary>
        /// 模块子项列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<PgcModuleItem> Items { get; set; }

        /// <summary>
        /// 模块Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "module_id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 模块样式. banner, function, v_card, topic.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "style", Required = Required.Default)]
        public string Style { get; set; }

        /// <summary>
        /// 模块标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 模块类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public int Type { get; set; }

        /// <summary>
        /// 模块头列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "headers", Required = Required.Default)]
        public List<PgcModuleHeader> Headers { get; set; }
    }

    /// <summary>
    /// PGC内容头.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcModuleHeader
    {
        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 导航地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string Url { get; set; }
    }

    /// <summary>
    /// 动漫及影视模块条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcModuleItem
    {
        /// <summary>
        /// 分集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public int Aid { get; set; }

        /// <summary>
        /// 徽章内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string Badge { get; set; }

        /// <summary>
        /// 网页链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "blink", Required = Required.Default)]
        public string WebLink { get; set; }

        /// <summary>
        /// 不明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
        public int Cid { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 描述内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 最新章节.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "new_ep", Required = Required.Default)]
        public PgcEpisode NewEpisode { get; set; }

        /// <summary>
        /// 所属动漫或影视剧的Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "oid", Required = Required.Default)]
        public int OriginId { get; set; }

        /// <summary>
        /// 剧集的季Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public int SeasonId { get; set; }

        /// <summary>
        /// 剧集的标签.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_styles", Required = Required.Default)]
        public string SeasonTags { get; set; }

        /// <summary>
        /// PGC用户交互参数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stat", Required = Required.Default)]
        public PgcItemStat Stat { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 卡片类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }

        /// <summary>
        /// 动漫状态.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Required.Default)]
        public PgcItemStatus Status { get; set; }

        /// <summary>
        /// 徽章内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cards", Required = Required.Default)]
        public List<PgcModuleItem> Cards { get; set; }

        /// <summary>
        /// 显示的综合评分文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pts", Required = Required.Default)]
        public string DisplayScoreText { get; set; }
    }

    /// <summary>
    /// 剧集信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcEpisode
    {
        /// <summary>
        /// 剧集封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 显示内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "index_show", Required = Required.Default)]
        public string DisplayText { get; set; }
    }

    /// <summary>
    /// 内容参数.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcItemStat
    {
        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmaku", Required = Required.Default)]
        public int DanmakuCount { get; set; }

        /// <summary>
        /// 关注数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow", Required = Required.Default)]
        public int FollowCount { get; set; }

        /// <summary>
        /// 关注的显示文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow_view", Required = Required.Default)]
        public string FollowDisplayText { get; set; }

        /// <summary>
        /// 观看次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "view", Required = Required.Default)]
        public int ViewCount { get; set; }
    }

    /// <summary>
    /// 内容状态（关于我是否关注或点赞）.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcItemStatus
    {
        /// <summary>
        /// 是否已关注，0-未关注，1-已关注.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow", Required = Required.Default)]
        public int IsFollow { get; set; }

        /// <summary>
        /// 是否已点赞，0-未点赞，1-已点赞.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public int IsLike { get; set; }
    }
}
