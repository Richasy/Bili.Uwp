// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// Dash播放信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayerInformation
    {
        /// <summary>
        /// 视频清晰度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "quality", Required = Required.Default)]
        public int Quality { get; set; }

        /// <summary>
        /// 当前视频格式.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format", Required = Required.Default)]
        public string Format { get; set; }

        /// <summary>
        /// 视频时长.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "timelength", Required = Required.Default)]
        public int Duration { get; set; }

        /// <summary>
        /// 支持的清晰度说明文本列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "accept_description", Required = Required.Default)]
        public List<string> AcceptDescription { get; set; }

        /// <summary>
        /// 支持的清晰度列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "accept_quality", Required = Required.Default)]
        public List<int> AcceptQualities { get; set; }

        /// <summary>
        /// 视频编码Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "video_codecid", Required = Required.Default)]
        public int CodecId { get; set; }

        /// <summary>
        /// Dash视频播放信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dash", Required = Required.Default)]
        public DashVideo VideoInformation { get; set; }

        /// <summary>
        /// Flv视频播放信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "durl", Required = Required.Default)]
        public List<FlvItem> FlvInformation { get; set; }

        /// <summary>
        /// 支持的视频格式列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "support_formats", Required = Required.Default)]
        public List<VideoFormat> SupportFormats { get; set; }
    }

    /// <summary>
    /// Dash视频信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class DashVideo
    {
        /// <summary>
        /// 视频时长.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "duration", Required = Required.Default)]
        public int Duration { get; set; }

        /// <summary>
        /// 最低缓冲时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "minBufferTime", Required = Required.Default)]
        public float MinBufferTime { get; set; }

        /// <summary>
        /// 不同清晰度的视频列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "video", Required = Required.Default)]
        public List<DashItem> Video { get; set; }

        /// <summary>
        /// 不同码率的音频列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "audio", Required = Required.Default)]
        public List<DashItem> Audio { get; set; }
    }

    /// <summary>
    /// Dash条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class DashItem
    {
        /// <summary>
        /// Dash Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 基链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "base_url", Required = Required.Default)]
        public string BaseUrl { get; set; }

        /// <summary>
        /// 备份链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "backup_url", Required = Required.Default)]
        public List<string> BackupUrl { get; set; }

        /// <summary>
        /// 媒体要求的带宽.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bandwidth", Required = Required.Default)]
        public int BandWidth { get; set; }

        /// <summary>
        /// 媒体格式.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mime_type", Required = Required.Default)]
        public string MimeType { get; set; }

        /// <summary>
        /// 媒体编码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "codecs", Required = Required.Default)]
        public string Codecs { get; set; }

        /// <summary>
        /// 媒体宽度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "width", Required = Required.Default)]
        public int Width { get; set; }

        /// <summary>
        /// 媒体高度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "height", Required = Required.Default)]
        public int Height { get; set; }

        /// <summary>
        /// 帧率.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "frame_rate", Required = Required.Default)]
        public string FrameRate { get; set; }

        /// <summary>
        /// 播放比例.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sar", Required = Required.Default)]
        public string Scale { get; set; }

        /// <summary>
        /// 分段的基础信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "segment_base", Required = Required.Default)]
        public SegmentBase SegmentBase { get; set; }

        /// <summary>
        /// 编码Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "codecid", Required = Required.Default)]
        public int CodecId { get; set; }
    }

    /// <summary>
    /// 分段基础信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SegmentBase
    {
        /// <summary>
        /// 起始位置.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "initialization", Required = Required.Default)]
        public string Initialization { get; set; }

        /// <summary>
        /// 索引范围.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "index_range", Required = Required.Default)]
        public string IndexRange { get; set; }
    }

    /// <summary>
    /// 支持的格式.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class VideoFormat
    {
        /// <summary>
        /// 清晰度标识.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "quality", Required = Required.Default)]
        public int Quality { get; set; }

        /// <summary>
        /// 格式.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format", Required = Required.Default)]
        public string Format { get; set; }

        /// <summary>
        /// 显示的说明文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "new_description", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 上标.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "superscript", Required = Required.Default)]
        public string Superscript { get; set; }
    }

    /// <summary>
    /// FLV视频条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FlvItem
    {
        /// <summary>
        /// 序号.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "order", Required = Required.Default)]
        public int Order { get; set; }

        /// <summary>
        /// 时长.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "length", Required = Required.Default)]
        public int Length { get; set; }

        /// <summary>
        /// 大小.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "size", Required = Required.Default)]
        public int Size { get; set; }

        /// <summary>
        /// 播放地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string Url { get; set; }

        /// <summary>
        /// 备用地址列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "backup_url", Required = Required.Default)]
        public List<string> BackupUrls { get; set; }
    }
}
