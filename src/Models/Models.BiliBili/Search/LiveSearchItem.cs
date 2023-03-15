// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 直播搜索条目.
    /// </summary>
    public class LiveSearchItem : SearchItemBase
    {
        /// <summary>
        /// 主播名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "roomid", Required = Required.Default)]
        public int RoomId { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public long UserId { get; set; }

        /// <summary>
        /// 类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }

        /// <summary>
        /// 关注数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "attentions", Required = Required.Default)]
        public int FollowerCount { get; set; }

        /// <summary>
        /// 直播状态.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_status", Required = Required.Default)]
        public int LiveStatus { get; set; }

        /// <summary>
        /// 直播间标签.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tags", Required = Required.Default)]
        public string Tags { get; set; }

        /// <summary>
        /// 直播分区.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "region", Required = Required.Default)]
        public int Region { get; set; }

        /// <summary>
        /// 观看人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "online", Required = Required.Default)]
        public int ViewerCount { get; set; }

        /// <summary>
        /// 直播分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_v2_name", Required = Required.Default)]
        public string AreaName { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }

        /// <summary>
        /// 直播网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_link", Required = Required.Default)]
        public string LiveLink { get; set; }
    }
}
