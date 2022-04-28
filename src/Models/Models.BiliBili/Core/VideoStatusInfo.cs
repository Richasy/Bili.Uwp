// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 视频状态信息.
    /// </summary>
    public class VideoStatusInfo
    {
        /// <summary>
        /// 视频的Aid.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public int Aid { get; set; }

        /// <summary>
        /// 视频播放数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "view", Required = Required.Default)]
        public long PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmaku", Required = Required.Default)]
        public long DanmakuCount { get; set; }

        /// <summary>
        /// 视频评论数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reply", Required = Required.Default)]
        public long ReplyCount { get; set; }

        /// <summary>
        /// 视频收藏数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favorite", Required = Required.Default)]
        public long FavoriteCount { get; set; }

        /// <summary>
        /// 投币数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "coin", Required = Required.Default)]
        public long CoinCount { get; set; }

        /// <summary>
        /// 分享数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share", Required = Required.Default)]
        public long ShareCount { get; set; }

        /// <summary>
        /// 当前排名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "now_rank", Required = Required.Default)]
        public int CurrentRanking { get; set; }

        /// <summary>
        /// 历史最高排名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "his_rank", Required = Required.Default)]
        public int HistoryRanking { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public long LikeCount { get; set; }

        /// <summary>
        /// 点踩数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dislike", Required = Required.Default)]
        public long DislikeCount { get; set; }
    }
}
