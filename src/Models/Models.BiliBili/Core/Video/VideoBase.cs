// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 视频基类型.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class VideoBase
    {
        /// <summary>
        /// 视频标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 视频发布时间 (Unix时间戳).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pubdate", Required = Required.Default)]
        public int PublishDateTime { get; set; }

        /// <summary>
        /// 视频时长 (秒).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "duration", Required = Required.Default)]
        public int Duration { get; set; }
    }
}
