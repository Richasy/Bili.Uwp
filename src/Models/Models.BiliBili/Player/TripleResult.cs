// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 一键三连结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TripleResult
    {
        /// <summary>
        /// 是否点赞.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public bool IsLike { get; set; }

        /// <summary>
        /// 是否投币.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "coin", Required = Required.Default)]
        public bool IsCoin { get; set; }

        /// <summary>
        /// 是否收藏.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fav", Required = Required.Default)]
        public bool IsFavorite { get; set; }

        /// <summary>
        /// 投币个数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "multiply", Required = Required.Default)]
        public int CoinNumber { get; set; }
    }
}
