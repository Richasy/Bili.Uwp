// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 首页推荐信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class HomeRecommendInfo
    {
        /// <summary>
        /// 返回的推荐卡片信息列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<RecommendCard> Items { get; set; }
    }
}
