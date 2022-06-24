// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 直播源横幅.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedBanner : LiveFeedExtraCardBase
    {
        /// <summary>
        /// 内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "content", Required = Required.Default)]
        public string Content { get; set; }

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
    }

    /// <summary>
    /// 直播源横幅列表.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedBannerList
    {
        /// <summary>
        /// 列表数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<LiveFeedBanner> List { get; set; }
    }
}
