// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 稍后再看响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ViewLaterResponse
    {
        /// <summary>
        /// 视频列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<ViewLaterVideo> List { get; set; }

        /// <summary>
        /// 稍后再看视频数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }
    }
}
