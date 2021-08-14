// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 搜索建议响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SearchSquareResponse : List<SearchSquareItem>
    {
    }

    /// <summary>
    /// 搜索推荐条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SearchSquareItem
    {
        /// <summary>
        /// 类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 响应代码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "data", Required = Required.Default)]
        public SearchSquareData Data { get; set; }
    }

    /// <summary>
    /// 搜索推荐的具体条目数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SearchSquareData
    {
        /// <summary>
        /// 响应代码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "trackid", Required = Required.Default)]
        public string TrackId { get; set; }

        /// <summary>
        /// 搜索建议条目.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<SearchRecommendItem> List { get; set; }

        /// <summary>
        /// 具体显示标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }
    }

    /// <summary>
    /// 搜索建议条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SearchRecommendItem
    {
        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "keyword", Required = Required.Default)]
        public string Keyword { get; set; }

        /// <summary>
        /// 显示名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "show_name", Required = Required.Default)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 要显示的图标.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "icon", Required = Required.Default)]
        public string Icon { get; set; }

        /// <summary>
        /// 排序.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "position", Required = Required.Default)]
        public int Position { get; set; }

        /// <summary>
        /// 标题（在搜索推荐中有效）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 建议类型（在搜索推荐中有效，通常为guess）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }

        /// <summary>
        /// 建议的Id（在搜索推荐中有效，通常为标签Id）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }
    }
}
