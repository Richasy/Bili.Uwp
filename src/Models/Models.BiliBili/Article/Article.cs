// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 专栏文章.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Article
    {
        /// <summary>
        /// 文章Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 所属标签.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "category", Required = Required.Default)]
        public ArticleCategory Category { get; set; }

        /// <summary>
        /// 关联标签列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "categories", Required = Required.Default)]
        public List<ArticleCategory> RelatedCategories { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 提要.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "summary", Required = Required.Default)]
        public string Summary { get; set; }

        /// <summary>
        /// 作者.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "author", Required = Required.Default)]
        public PublisherInfo Publisher { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "publish_time", Required = Required.Default)]
        public int PublishTime { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ctime", Required = Required.Default)]
        public int CreateTime { get; set; }

        /// <summary>
        /// 文章状态参数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stats", Required = Required.Default)]
        public ArticleStats Stats { get; set; }

        /// <summary>
        /// 字数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "words", Required = Required.Default)]
        public int WordCount { get; set; }

        /// <summary>
        /// 文章封面列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "origin_image_urls", Required = Required.Default)]
        public List<string> CoverUrls { get; set; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_like", Required = Required.Default)]
        public bool IsLike { get; set; }

        /// <summary>
        /// 是否为原创，0-非原创，1-原创.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "original", Required = Required.Default)]
        public int IsOriginal { get; set; }
    }
}
