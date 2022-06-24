// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 文章搜索条目.
    /// </summary>
    public class ArticleSearchItem : SearchItemBase
    {
        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 描述.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public int UserId { get; set; }

        /// <summary>
        /// 文章Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 图片链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "image_urls", Required = Required.Default)]
        public List<string> CoverUrls { get; set; }

        /// <summary>
        /// 阅读次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "view", Required = Required.Default)]
        public int ViewCount { get; set; }

        /// <summary>
        /// 点赞次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public int LikeCount { get; set; }

        /// <summary>
        /// 评论数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reply", Required = Required.Default)]
        public int ReplyCount { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }
    }
}
