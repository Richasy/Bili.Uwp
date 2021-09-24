// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// PGC搜索条目.
    /// </summary>
    public class PgcSearchItem : SearchItemBase
    {
        /// <summary>
        /// 发布时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ptime", Required = Required.Default)]
        public int PublishTime { get; set; }

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
        /// 剧集类型名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_type_name", Required = Required.Default)]
        public string SeasonTypeName { get; set; }

        /// <summary>
        /// 说明文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "label", Required = Required.Default)]
        public string Label { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "styles", Required = Required.Default)]
        public string SubTitle { get; set; }

        /// <summary>
        /// 声优.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cv", Required = Required.Default)]
        public string CV { get; set; }

        /// <summary>
        /// 所属区域.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area", Required = Required.Default)]
        public string Area { get; set; }

        /// <summary>
        /// 参与人员.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "staff", Required = Required.Default)]
        public string Staff { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }

        /// <summary>
        /// 是否已追，0-未追，1-已追.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_atten", Required = Required.Default)]
        public int IsFollow { get; set; }

        /// <summary>
        /// 评分.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rating", Required = Required.Default)]
        public double Rating { get; set; }

        /// <summary>
        /// 投票人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "vote", Required = Required.Default)]
        public int VoteNumber { get; set; }
    }
}
