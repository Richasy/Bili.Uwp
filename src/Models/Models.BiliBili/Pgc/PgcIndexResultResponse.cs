// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// PGC索引筛选结果响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcIndexResultResponse
    {
        /// <summary>
        /// 是否还有下一页.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "has_next", Required = Required.Default)]
        public int HasNext { get; set; }

        /// <summary>
        /// 结果.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<PgcIndexItem> List { get; set; }

        /// <summary>
        /// 当前页码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "num", Required = Required.Default)]
        public int PageNumber { get; set; }

        /// <summary>
        /// 每页条目数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "size", Required = Required.Default)]
        public int PageSize { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total", Required = Required.Default)]
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// PGC索引条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcIndexItem
    {
        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }

        /// <summary>
        /// 筛选条件.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 显示附加文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "index_show", Required = Required.Default)]
        public string AdditionalText { get; set; }

        /// <summary>
        /// 是否完结.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_finish", Required = Required.Default)]
        public int IsFinish { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 媒体Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "media_id", Required = Required.Default)]
        public int MediaId { get; set; }

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
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 排序显示文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "order", Required = Required.Default)]
        public string OrderText { get; set; }
    }
}
