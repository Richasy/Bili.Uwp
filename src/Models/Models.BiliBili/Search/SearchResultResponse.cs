// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 搜索结果响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SearchResultResponse
    {
        /// <summary>
        /// 追踪Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "trackid", Required = Required.Default)]
        public string TrackId { get; set; }

        /// <summary>
        /// 页码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "page", Required = Required.Default)]
        public int PageNumber { get; set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "keyword", Required = Required.Default)]
        public string Keyword { get; set; }
    }

    /// <summary>
    /// 综合搜索结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ComprehensiveSearchResultResponse : SearchResultResponse
    {
        /// <summary>
        /// 子模块列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "nav", Required = Required.Default)]
        public List<SearchSubModule> SubModuleList { get; set; }

        /// <summary>
        /// 条目列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "item", Required = Required.Default)]
        public List<VideoSearchItem> ItemList { get; set; }
    }

    /// <summary>
    /// 常规子模块搜索结果.
    /// </summary>
    /// <typeparam name="T">内容类型.</typeparam>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SubModuleSearchResultResponse<T> : SearchResultResponse
    {
        /// <summary>
        /// 条目列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<T> ItemList { get; set; }
    }

    /// <summary>
    /// 直播搜索结果.
    /// </summary>
    public class LiveSearchResultResponse : SearchResultResponse
    {
        /// <summary>
        /// 直播间结果.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_room", Required = Required.Default)]
        public LiveRoomSearchResult RoomResult { get; set; }
    }

    /// <summary>
    /// 搜索子模块.
    /// </summary>
    public class SearchSubModule
    {
        /// <summary>
        /// 显示标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 搜索结果总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total", Required = Required.Default)]
        public int Total { get; set; }

        /// <summary>
        /// 分页数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pages", Required = Required.Default)]
        public int PageCount { get; set; }

        /// <summary>
        /// 类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public int Type { get; set; }
    }

    /// <summary>
    /// 直播间搜索结果.
    /// </summary>
    public class LiveRoomSearchResult
    {
        /// <summary>
        /// 条目列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<LiveSearchItem> Items { get; set; }
    }
}
