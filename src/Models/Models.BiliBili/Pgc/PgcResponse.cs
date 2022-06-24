// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 专业产出内容响应结果（包括动漫，电影，电视剧，纪录片等非用户产出内容）.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcResponse
    {
        /// <summary>
        /// 数据源标识.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "feed", Required = Required.Default)]
        public PgcFeedIdentifier FeedIdentifier { get; set; }

        /// <summary>
        /// 模块.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "modules", Required = Required.Default)]
        public List<PgcModule> Modules { get; set; }

        /// <summary>
        /// 下次请求的指针.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "next_cursor", Required = Required.Default)]
        public string NextCursor { get; set; }
    }

    /// <summary>
    /// PGC数据源标识.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcFeedIdentifier
    {
        /// <summary>
        /// 下属分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fall_wid", Required = Required.Default)]
        public List<int> PartitionIds { get; set; }

        /// <summary>
        /// 数据源类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }
    }
}
