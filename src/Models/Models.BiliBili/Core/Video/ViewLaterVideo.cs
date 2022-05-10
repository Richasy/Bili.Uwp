// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 稍后再看视频.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ViewLaterVideo : VideoBase
    {
        /// <summary>
        /// 视频标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public int VideoId { get; set; }

        /// <summary>
        /// 稿件分P总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "videos", Required = Required.Default)]
        public int PartCount { get; set; }

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
        /// 转载或原创，1-转载，2-原创.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "copyright", Required = Required.Default)]
        public int Copyright { get; set; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pic", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 稿件创建时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ctime", Required = Required.Default)]
        public int CreateTime { get; set; }

        /// <summary>
        /// 视频描述.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 视频状态.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "state", Required = Required.Default)]
        public int State { get; set; }

        /// <summary>
        /// 视频发布者信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "owner", Required = Required.Default)]
        public PublisherInfo Publisher { get; set; }

        /// <summary>
        /// 视频参数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stat", Required = Required.Default)]
        public VideoStatusInfo StatusInfo { get; set; }

        /// <summary>
        /// 关联动态的文本内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dynamic", Required = Required.Default)]
        public string DynamicText { get; set; }

        /// <summary>
        /// 短链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "short_link_v2", Required = Required.Default)]
        public string ShortLink { get; set; }

        /// <summary>
        /// 分P Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
        public int PartId { get; set; }

        /// <summary>
        /// 播放进度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "progress", Required = Required.Default)]
        public int Progress { get; set; }

        /// <summary>
        /// 添加时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "add_at", Required = Required.Default)]
        public int AddTime { get; set; }

        /// <summary>
        /// 视频BVId.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bvid", Required = Required.Default)]
        public string BvId { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public int SeasonId { get; set; }
    }
}
