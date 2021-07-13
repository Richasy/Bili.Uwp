// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 直播间基类.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveRoomBase
    {
        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uid", Required = Required.Default)]
        public int UserId { get; set; }

        /// <summary>
        /// 直播间封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 直播间标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 清晰度描述列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "quality_description", Required = Required.Default)]
        public List<LiveQualityDescription> QualityDescriptionList { get; set; }

        /// <summary>
        /// 会话标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "session_id", Required = Required.Default)]
        public string SessionId { get; set; }

        /// <summary>
        /// 分组标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "group_id", Required = Required.Default)]
        public int GroupId { get; set; }

        /// <summary>
        /// 在线观看人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "online", Required = Required.Default)]
        public int ViewerCount { get; set; }

        /// <summary>
        /// 播放地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play_url", Required = Required.Default)]
        public string PlayUrl { get; set; }

        /// <summary>
        /// H265播放地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play_url_h265", Required = Required.Default)]
        public string H265PlayUrl { get; set; }

        /// <summary>
        /// 支持的清晰度列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "accept_quality", Required = Required.Default)]
        public List<int> AcceptQuality { get; set; }

        /// <summary>
        /// 当前清晰度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "current_quality", Required = Required.Default)]
        public int CurrentQuality { get; set; }

        /// <summary>
        /// 对决的直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pk_id", Required = Required.Default)]
        public int PkId { get; set; }

        /// <summary>
        /// 直播间地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }
    }

    /// <summary>
    /// 直播清晰度说明.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveQualityDescription
    {
        /// <summary>
        /// 清晰度标识.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "qn", Required = Required.Default)]
        public int Quality { get; set; }

        /// <summary>
        /// 清晰度说明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }
    }
}
