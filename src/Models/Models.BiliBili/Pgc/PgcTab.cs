// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 动漫或影视的顶部标签页.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcTab
    {
        /// <summary>
        /// 标签页Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 所指向的链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }
    }
}
