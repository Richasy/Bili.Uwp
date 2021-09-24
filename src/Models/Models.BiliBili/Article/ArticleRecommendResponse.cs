// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 文章推荐响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ArticleRecommendResponse
    {
        /// <summary>
        /// 横幅.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "banners", Required = Required.Default)]
        public List<PartitionBanner> Banners { get; set; }

        /// <summary>
        /// 文章列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "articles", Required = Required.Default)]
        public List<Article> Articles { get; set; }

        /// <summary>
        /// 排行榜.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ranks", Required = Required.Default)]
        public List<Article> Ranks { get; set; }
    }
}
