// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 专业产出内容响应结果（包括动漫，电影，电视剧，纪录片等非用户产出内容）.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcResponse
    {
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
}
