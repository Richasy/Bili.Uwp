// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 直播播放信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppPlayInformation
    {
        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "room_id", Required = Required.Default)]
        public int RoomId { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uid", Required = Required.Default)]
        public long UserId { get; set; }

        /// <summary>
        /// 直播状态，1表示正在直播.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_status", Required = Required.Default)]
        public int LiveStatus { get; set; }

        /// <summary>
        /// 直播时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_time", Required = Required.Default)]
        public long LiveTime { get; set; }

        /// <summary>
        /// 播放信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "playurl_info", Required = Required.Default)]
        public LiveAppPlayUrlInfo PlayUrlInfo { get; set; }
    }

    /// <summary>
    /// 直播播放地址信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppPlayUrlInfo
    {
        /// <summary>
        /// 播放信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "playurl", Required = Required.Default)]
        public LiveAppPlayData PlayUrl { get; set; }
    }

    /// <summary>
    /// 直播播放链接.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppPlayData
    {
        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
        public int Cid { get; set; }

        /// <summary>
        /// 清晰度列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "g_qn_desc", Required = Required.Default)]
        public List<LiveAppQualityDescription> Descriptions { get; set; }

        /// <summary>
        /// 播放流.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stream", Required = Required.Default)]
        public List<LiveAppPlayStream> StreamList { get; set; }
    }

    /// <summary>
    /// 清晰度描述.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppQualityDescription
    {
        /// <summary>
        /// 清晰度标识.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "qn", Required = Required.Default)]
        public int Quality { get; set; }

        /// <summary>
        /// 描述.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// HDR 标识.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hdr_desc", Required = Required.Default)]
        public string HDRSign { get; set; }
    }

    /// <summary>
    /// 直播播放流.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppPlayStream
    {
        /// <summary>
        /// 协议名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "protocol_name", Required = Required.Default)]
        public string ProtocolName { get; set; }

        /// <summary>
        /// 格式列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format", Required = Required.Default)]
        public List<LiveAppPlayFormat> FormatList { get; set; }
    }

    /// <summary>
    /// 直播播放格式.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppPlayFormat
    {
        /// <summary>
        /// 格式名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format_name", Required = Required.Default)]
        public string FormatName { get; set; }

        /// <summary>
        /// 分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "codec", Required = Required.Default)]
        public List<LiveAppPlayCodec> CodecList { get; set; }
    }

    /// <summary>
    /// 直播播放解码信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppPlayCodec
    {
        /// <summary>
        /// 解码名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "codec_name", Required = Required.Default)]
        public string CodecName { get; set; }

        /// <summary>
        /// 当前清晰度标识.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "current_qn", Required = Required.Default)]
        public int CurrentQuality { get; set; }

        /// <summary>
        /// 支持的清晰度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "accept_qn", Required = Required.Default)]
        public List<int> AcceptQualities { get; set; }

        /// <summary>
        /// 基础链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "base_url", Required = Required.Default)]
        public string BaseUrl { get; set; }

        /// <summary>
        /// 播放地址列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url_info", Required = Required.Default)]
        public List<LiveAppPlayUrl> Urls { get; set; }

        /// <summary>
        /// 杜比类型，0-关闭, 1-开启.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dolby_type", Required = Required.Default)]
        public int DolbyType { get; set; }
    }

    /// <summary>
    /// 直播播放地址拼接信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveAppPlayUrl
    {
        /// <summary>
        /// 域名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "host", Required = Required.Default)]
        public string Host { get; set; }

        /// <summary>
        /// 后缀.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "extra", Required = Required.Default)]
        public string Extra { get; set; }

        /// <summary>
        /// 流的有效时间，通常为1个小时.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stream_ttl", Required = Required.Default)]
        public int StreamTTL { get; set; }
    }
}
