// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 搜索条目基类.
    /// </summary>
    public class SearchItemBase
    {
        /// <summary>
        /// 追踪Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "trackid", Required = Required.Default)]
        public string TrackId { get; set; }

        /// <summary>
        /// 链接类型. bgm_media指动漫番剧内容，app_user指用户，video指视频.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "linktype", Required = Required.Default)]
        public string LinkType { get; set; }

        /// <summary>
        /// 排序.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "position", Required = Required.Default)]
        public int Position { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 导航链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uri", Required = Required.Default)]
        public string Uri { get; set; }

        /// <summary>
        /// 参数，通常指Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "param", Required = Required.Default)]
        public string Parameter { get; set; }

        /// <summary>
        /// 目标指向类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "goto", Required = Required.Default)]
        public string Goto { get; set; }
    }
}
