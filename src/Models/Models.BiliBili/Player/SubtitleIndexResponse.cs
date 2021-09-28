// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 字幕索引响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubtitleIndexResponse
    {
        /// <summary>
        /// 支持提交.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "allow_submit", Required = Required.Default)]
        public bool AllowSubmit { get; set; }

        /// <summary>
        /// 字幕索引列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subtitles", Required = Required.Default)]
        public List<SubtitleIndexItem> Subtitles { get; set; }
    }

    /// <summary>
    /// 字幕索引条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubtitleIndexItem
    {
        /// <summary>
        /// 字幕Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public long Id { get; set; }

        /// <summary>
        /// 语言代码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "lan", Required = Required.Default)]
        public string Language { get; set; }

        /// <summary>
        /// 显示语言.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "lan_doc", Required = Required.Default)]
        public string DisplayLanguage { get; set; }

        /// <summary>
        /// 字幕地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subtitle_url", Required = Required.Default)]
        public string Url { get; set; }
    }
}
