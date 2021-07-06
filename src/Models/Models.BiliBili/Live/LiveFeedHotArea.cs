// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 直播源热门分区.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedHotArea : LiveFeedExtraCardBase
    {
        /// <summary>
        /// 分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_v2_id", Required = Required.Default)]
        public int AreaId { get; set; }

        /// <summary>
        /// 父分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_v2_parent_id", Required = Required.Default)]
        public int ParentAreaId { get; set; }

        /// <summary>
        /// 标签类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tag_type", Required = Required.Default)]
        public int TagType { get; set; }
    }

    /// <summary>
    /// 直播源热门分区列表.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedHotAreaList
    {
        /// <summary>
        /// 列表数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<LiveFeedHotArea> List { get; set; }
    }
}
