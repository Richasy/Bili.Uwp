// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 用户搜索条目.
    /// </summary>
    public class UserSearchItem : SearchItemBase
    {
        /// <summary>
        /// 签名/个人介绍.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sign", Required = Required.Default)]
        public string Sign { get; set; }

        /// <summary>
        /// 粉丝数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fans", Required = Required.Default)]
        public int FollowerCount { get; set; }

        /// <summary>
        /// 等级.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "level", Required = Required.Default)]
        public int Level { get; set; }

        /// <summary>
        /// 大会员信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "vip", Required = Required.Default)]
        public Vip Vip { get; set; }

        /// <summary>
        /// 是否是UP主.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_up", Required = Required.Default)]
        public bool IsUp { get; set; }

        /// <summary>
        /// 投稿数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "archives", Required = Required.Default)]
        public int ArchiveCount { get; set; }

        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "roomid", Required = Required.Default)]
        public int RoomId { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public int UserId { get; set; }

        /// <summary>
        /// 直播间网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_link", Required = Required.Default)]
        public string LiveLink { get; set; }

        /// <summary>
        /// 用户关系.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "relation", Required = Required.Default)]
        public UserRelation Relation { get; set; }
    }
}
