// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 文章收藏夹响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ArticleFavoriteListResponse
    {
        /// <summary>
        /// 总条目数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 文章列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<FavoriteArticleItem> Items { get; set; }
    }

    /// <summary>
    /// 收藏的文章.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteArticleItem
    {
        /// <summary>
        /// 文章Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 横幅图片.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "banner_url", Required = Required.Default)]
        public string Banner { get; set; }

        /// <summary>
        /// 发布者名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string PublisherName { get; set; }

        /// <summary>
        /// 图片链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "image_urls", Required = Required.Default)]
        public List<string> Images { get; set; }

        /// <summary>
        /// 提要.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "summary", Required = Required.Default)]
        public string Summary { get; set; }

        /// <summary>
        /// 收藏时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favorite_time", Required = Required.Default)]
        public int CollectTime { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uri", Required = Required.Default)]
        public string Url { get; set; }

        /// <summary>
        /// 发布者名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "up_mid", Required = Required.Default)]
        public int PublisherId { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }
    }
}
