// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 在线观看人数.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class OnlineViewerCount
    {
        /// <summary>
        /// 显示文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total_text", Required = Required.Default)]
        public string DisplayText { get; set; }
    }

    /// <summary>
    /// 在线观看人数响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class OnlineViewerResponse
    {
        /// <summary>
        /// 数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "online", Required = Required.Default)]
        public OnlineViewerCount Data { get; set; }
    }
}
