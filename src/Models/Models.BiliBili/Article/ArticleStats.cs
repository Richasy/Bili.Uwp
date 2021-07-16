// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 文章参数.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ArticleStats
    {
        /// <summary>
        /// 阅读次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "view", Required = Required.Default)]
        public int ViewCount { get; set; }

        /// <summary>
        /// 收藏次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favorite", Required = Required.Default)]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// 点赞次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public int LikeCount { get; set; }

        /// <summary>
        /// 点踩次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dislike", Required = Required.Default)]
        public int DislikeCount { get; set; }

        /// <summary>
        /// 评论数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reply", Required = Required.Default)]
        public int ReplyCount { get; set; }

        /// <summary>
        /// 分享次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share", Required = Required.Default)]
        public int ShareCount { get; set; }

        /// <summary>
        /// 硬币数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "coin", Required = Required.Default)]
        public int CoinCount { get; set; }

        /// <summary>
        /// 动态转发次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dynamic", Required = Required.Default)]
        public int DynamicCount { get; set; }
    }
}
