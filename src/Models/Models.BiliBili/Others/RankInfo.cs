// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 排行榜信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RankInfo
    {
        /// <summary>
        /// 排行榜说明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "note", Required = Required.Default)]
        public string Note { get; set; }

        /// <summary>
        /// 排行榜下的视频列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<RankVideo> VideoList { get; set; }
    }
}
