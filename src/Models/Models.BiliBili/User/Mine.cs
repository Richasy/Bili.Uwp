// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 我的基本数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Mine
    {
        /// <summary>
        /// 用户ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public int Mid { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 用户头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 硬币数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "coin", Required = Required.Default)]
        public double CoinNumber { get; set; }

        /// <summary>
        /// B币数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bcoin", Required = Required.Default)]
        public int BcoinCount { get; set; }

        /// <summary>
        /// 性别，0-保密，1-男性，2-女性.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sex", Required = Required.Default)]
        public int Sex { get; set; }

        /// <summary>
        /// 等级.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "level", Required = Required.Default)]
        public int Level { get; set; }

        /// <summary>
        /// 动态数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dynamic", Required = Required.Default)]
        public int DynamicCount { get; set; }

        /// <summary>
        /// 关注数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "following", Required = Required.Default)]
        public int FollowCount { get; set; }

        /// <summary>
        /// 粉丝数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follower", Required = Required.Default)]
        public int FollowerCount { get; set; }
    }
}
