// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 发布者信息.
    /// </summary>
    public class PublisherInfo
    {
        /// <summary>
        /// 视频发布者的Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public int Mid { get; set; }

        /// <summary>
        /// 视频发布者.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Publisher { get; set; }

        /// <summary>
        /// 视频发布者的头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string PublisherAvatar { get; set; }
    }
}
