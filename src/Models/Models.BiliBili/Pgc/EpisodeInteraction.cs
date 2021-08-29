// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 分集交互信息（包括用户投币，点赞，收藏等信息）.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EpisodeInteraction
    {
        /// <summary>
        /// 投币数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "coin_number", Required = Required.Default)]
        public int CoinNumber { get; set; }

        /// <summary>
        /// 是否收藏.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favorite", Required = Required.Default)]
        public int IsFavorite { get; set; }

        /// <summary>
        /// 是否点赞.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public int IsLike { get; set; }
    }
}
