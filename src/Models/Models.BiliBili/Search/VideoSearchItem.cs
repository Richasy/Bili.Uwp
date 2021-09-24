// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 视频搜索结果条目.
    /// </summary>
    public class VideoSearchItem : SearchItemBase
    {
        /// <summary>
        /// 播放数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play", Required = Required.Default)]
        public int PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmaku", Required = Required.Default)]
        public int DanmakuCount { get; set; }

        /// <summary>
        /// 作者.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "author", Required = Required.Default)]
        public string Author { get; set; }

        /// <summary>
        /// 说明文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 时长.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "duration", Required = Required.Default)]
        public string Duration { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public int UserId { get; set; }

        /// <summary>
        /// 头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 分享数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share", Required = Required.Default)]
        public ShareData Share { get; set; }

        /// <summary>
        /// 分享数据.
        /// </summary>
        public class ShareData
        {
            /// <summary>
            /// 类型.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
            public string Type { get; set; }

            /// <summary>
            /// 视频数据.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "video", Required = Required.Default)]
            public Video Video { get; set; }
        }

        /// <summary>
        /// 视频基本数据.
        /// </summary>
        public class Video
        {
            /// <summary>
            /// BV Id.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bvid", Required = Required.Default)]
            public string BvId { get; set; }

            /// <summary>
            /// 分P Id.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
            public int Cid { get; set; }

            /// <summary>
            /// 短链接.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "short_link", Required = Required.Default)]
            public string ShortLink { get; set; }
        }
    }
}
