// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 排行榜视频信息.
    /// </summary>
    public class RankVideo : VideoBase
    {
        /// <summary>
        /// 视频的Aid.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public int Aid { get; set; }

        /// <summary>
        /// 分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tid", Required = Required.Default)]
        public int PartitionId { get; set; }

        /// <summary>
        /// 分区名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tname", Required = Required.Default)]
        public string PartitionName { get; set; }

        /// <summary>
        /// 视频类型，1-原创，2-转载.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "copyright", Required = Required.Default)]
        public int Copyright { get; set; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pic", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 投稿时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ctime", Required = Required.Default)]
        public int ContributeTime { get; set; }

        /// <summary>
        /// 视频说明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 视频作者信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "owner", Required = Required.Default)]
        public PublisherInfo PublisherInfo { get; set; }

        /// <summary>
        /// 视频播放信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stat", Required = Required.Default)]
        public VideoStatusInfo Status { get; set; }

        /// <summary>
        /// 视频第1P的Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
        public int Cid { get; set; }

        /// <summary>
        /// 用于分享的短链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "short_link", Required = Required.Default)]
        public string ShortLink { get; set; }

        /// <summary>
        /// 视频的Bvid.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bvid", Required = Required.Default)]
        public string Bvid { get; set; }

        /// <summary>
        /// 综合评分.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "score", Required = Required.Default)]
        public int Score { get; set; }
    }
}
