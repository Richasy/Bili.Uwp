// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 视频基类型.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PartitionVideo : VideoBase
    {
        /// <summary>
        /// 视频封面图片地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 视频播放地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uri", Required = Required.Default)]
        public string Uri { get; set; }

        /// <summary>
        /// 参数，通常指代视频ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "param", Required = Required.Default)]
        public string Parameter { get; set; }

        /// <summary>
        /// 视频类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "goto", Required = Required.Default)]
        public string Type { get; set; }

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

        /// <summary>
        /// 视频播放数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play", Required = Required.Default)]
        public int PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmaku", Required = Required.Default)]
        public int DanmakuCount { get; set; }

        /// <summary>
        /// 视频评论数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reply", Required = Required.Default)]
        public int ReplyCount { get; set; }

        /// <summary>
        /// 视频收藏数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favourite", Required = Required.Default)]
        public int FavouriteCount { get; set; }

        /// <summary>
        /// 所属分区ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rid", Required = Required.Default)]
        public int PartitionId { get; set; }

        /// <summary>
        /// 所属分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rname", Required = Required.Default)]
        public string PartitionName { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public int LikeCount { get; set; }
    }
}
