// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 横幅定义.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Banner
    {
        /// <summary>
        /// 横幅Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 横幅标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 横幅图片地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "image", Required = Required.Default)]
        public string Image { get; set; }

        /// <summary>
        /// 哈希特征值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hash", Required = Required.Default)]
        public string Hash { get; set; }

        /// <summary>
        /// 导航地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uri", Required = Required.Default)]
        public string NavigateUri { get; set; }

        /// <summary>
        /// 对应资源的Id值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "resource_id", Required = Required.Default)]
        public int ResourceId { get; set; }

        /// <summary>
        /// 请求Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "request_id", Required = Required.Default)]
        public string RequestId { get; set; }

        /// <summary>
        /// 是否为广告.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_ad", Required = Required.Default)]
        public bool IsAD { get; set; }

        /// <summary>
        /// 在集合中的索引值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "index", Required = Required.Default)]
        public int Index { get; set; }
    }
}
