// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 标签类型.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Tag
    {
        /// <summary>
        /// 标签ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tid", Required = Required.Default)]
        public int TagId { get; set; }

        /// <summary>
        /// 标签名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tname", Required = Required.Default)]
        public string TagName { get; set; }

        /// <summary>
        /// 所属子分区ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rid", Required = Required.Default)]
        public int SubPartitionId { get; set; }

        /// <summary>
        /// 所属子分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rname", Required = Required.Default)]
        public string SubPartitionName { get; set; }

        /// <summary>
        /// 所属主分区ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reid", Required = Required.Default)]
        public int PartitionId { get; set; }

        /// <summary>
        /// 所属主分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rename", Required = Required.Default)]
        public string PartitionName { get; set; }
    }
}
