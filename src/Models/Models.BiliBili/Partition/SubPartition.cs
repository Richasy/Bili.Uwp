﻿// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 子分区类型定义.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubPartition
    {
        /// <summary>
        /// 推荐视频列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "recommend", Required = Required.Default)]
        public List<PartitionVideo> RecommendVideos { get; set; }

        /// <summary>
        /// 新的视频列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "new", Required = Required.Default)]
        public List<PartitionVideo> NewVideos { get; set; }

        /// <summary>
        /// 向上刷新的标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ctop", Required = Required.Default)]
        public int TopOffsetId { get; set; }

        /// <summary>
        /// 向下刷新的标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cbottom", Required = Required.Default)]
        public int BottomOffsetId { get; set; }
    }

    /// <summary>
    /// 子分区的推荐模块.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubPartitionRecommend : SubPartition
    {
        /// <summary>
        /// 横幅.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "banner", Required = Required.Default)]
        public RecommendBanner Banner { get; set; }

        /// <summary>
        /// 推荐列表下的横幅定义.
        /// </summary>
        public class RecommendBanner
        {
            /// <summary>
            /// 顶层横幅.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "top", Required = Required.Default)]
            public List<PartitionBanner> TopBanners { get; set; }
        }
    }

    /// <summary>
    /// 常规子分区.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubPartitionDefault : SubPartition
    {
        /// <summary>
        /// 高频标签.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "top_tag", Required = Required.Default)]
        public List<Tag> TopTags { get; set; }
    }
}
