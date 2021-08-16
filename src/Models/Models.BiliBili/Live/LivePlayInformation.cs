// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 直播播放信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LivePlayInformation
    {
        /// <summary>
        /// 当前播放清晰度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "current_quality", Required = Required.Default)]
        public int CurrentQuality { get; set; }

        /// <summary>
        /// 支持的播放清晰度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "accept_quality", Required = Required.Default)]
        public List<string> AcceptQuality { get; set; }

        /// <summary>
        /// 清晰度列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "quality_description", Required = Required.Default)]
        public List<LiveQualityDescription> QualityDescriptions { get; set; }

        /// <summary>
        /// 播放地址列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "durl", Required = Required.Default)]
        public List<LivePlayLine> PlayLines { get; set; }
    }

    /// <summary>
    /// 直播播放地址响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LivePlayUrlResponse
    {
        /// <summary>
        /// 当前播放清晰度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play_url", Required = Required.Default)]
        public LivePlayInformation Information { get; set; }
    }

    /// <summary>
    /// 直播播放地址.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LivePlayLine
    {
        /// <summary>
        /// 播放地址列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string Url { get; set; }

        /// <summary>
        /// 未知.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "length", Required = Required.Default)]
        public int Length { get; set; }

        /// <summary>
        /// 排序.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "order", Required = Required.Default)]
        public int Order { get; set; }

        /// <summary>
        /// 流类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stream_type", Required = Required.Default)]
        public int StreamType { get; set; }

        /// <summary>
        /// P2P类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "p2p_type", Required = Required.Default)]
        public int P2PType { get; set; }
    }
}
