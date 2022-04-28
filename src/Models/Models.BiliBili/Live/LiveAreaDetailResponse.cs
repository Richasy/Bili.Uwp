// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 直播间分区详情响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAreaDetailResponse
    {
        /// <summary>
        /// 子标签.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "new_tags", Required = Required.Default)]
        public List<LiveAreaDetailTag> Tags { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 直播间列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<LiveFeedRoom> List { get; set; }
    }

    /// <summary>
    /// 直播间详情的子标签.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAreaDetailTag
    {
        /// <summary>
        /// 标签Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 标签名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sort_type", Required = Required.Default)]
        public string SortType { get; set; }

        /// <summary>
        /// 类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public int Type { get; set; }

        /// <summary>
        /// 排序.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sort", Required = Required.Default)]
        public int Sort { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveAreaDetailTag tag && Id == tag.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }
}
