// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 我的信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MyInfo
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
        /// 用户签名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sign", Required = Required.Default)]
        public string Sign { get; set; }

        /// <summary>
        /// 硬币数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "coins", Required = Required.Default)]
        public double Coins { get; set; }

        /// <summary>
        /// 生日，格式为YYYY-MM-DD.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "birthday", Required = Required.Default)]
        public string Birthday { get; set; }

        /// <summary>
        /// 头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 性别，0-保密，1-男性，2-女性.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sex", Required = Required.Default)]
        public int Sex { get; set; }

        /// <summary>
        /// 账户等级.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "level", Required = Required.Default)]
        public int Level { get; set; }

        /// <summary>
        /// 封禁状态，0-正常，1-被封.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "silence", Required = Required.Default)]
        public int IsBlocking { get; set; }

        /// <summary>
        /// 大会员信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "vip", Required = Required.Default)]
        public Vip VIP { get; set; }
    }

    /// <summary>
    /// 大会员信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Vip
    {
        /// <summary>
        /// 大会员类型，0-非会员，1-月度大会员，2-年度及以上大会员.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public int Type { get; set; }

        /// <summary>
        /// 会员状态，0-无，1-有.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Required.Default)]
        public int Status { get; set; }

        /// <summary>
        /// 会员过期时间（毫秒Unix时间戳）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "due_date", Required = Required.Default)]
        public long DueDate { get; set; }
    }
}
