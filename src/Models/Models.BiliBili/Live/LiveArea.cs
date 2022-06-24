// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 直播间分区.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveArea
    {
        /// <summary>
        /// 分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 标签地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 标志.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pic", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 父分区 Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "parent_id", Required = Required.Default)]
        public int ParentId { get; set; }

        /// <summary>
        /// 父分区名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "parent_name", Required = Required.Default)]
        public string ParentName { get; set; }

        /// <summary>
        /// 分区类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_type", Required = Required.Default)]
        public int AreaType { get; set; }

        /// <summary>
        /// 是否为新分区.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_new", Required = Required.Default)]
        public bool IsNew { get; set; }
    }

    /// <summary>
    /// 直播间分区组.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAreaGroup
    {
        /// <summary>
        /// 标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 父分区类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "parent_area_type", Required = Required.Default)]
        public int ParentAreaType { get; set; }

        /// <summary>
        /// 分区列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_list", Required = Required.Default)]
        public List<LiveArea> AreaList { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveAreaGroup group && Id == group.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }
}
