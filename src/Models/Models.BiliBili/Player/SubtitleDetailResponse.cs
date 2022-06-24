// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 字幕详情响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubtitleDetailResponse
    {
        /// <summary>
        /// 字幕列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Required.Default)]
        public List<SubtitleItem> Body { get; set; }
    }

    /// <summary>
    /// 字幕条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubtitleItem
    {
        /// <summary>
        /// 开始时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "from", Required = Required.Default)]
        public double From { get; set; }

        /// <summary>
        /// 结束时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "to", Required = Required.Default)]
        public double To { get; set; }

        /// <summary>
        /// 字幕内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "content", Required = Required.Default)]
        public string Content { get; set; }
    }
}
