// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 直播源附加卡片基类.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedExtraCardBase
    {
        /// <summary>
        /// 标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 导航地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pic", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }
    }
}
