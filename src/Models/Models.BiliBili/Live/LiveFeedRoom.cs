// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 直播源推荐中我关注的直播间.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedRoom : LiveRoomBase
    {
        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "roomid", Required = Required.Default)]
        public int RoomId { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uname", Required = Required.Default)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string UserAvatar { get; set; }

        /// <summary>
        /// 直播开始时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_time", Required = Required.Default)]
        public int LiveStartTime { get; set; }

        /// <summary>
        /// 显示分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area", Required = Required.Default)]
        public string DisplayAreaId { get; set; }

        /// <summary>
        /// 显示分区名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_name", Required = Required.Default)]
        public string DisplayAreaName { get; set; }

        /// <summary>
        /// 分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_v2_id", Required = Required.Default)]
        public int AreaId { get; set; }

        /// <summary>
        /// 分区名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_v2_name", Required = Required.Default)]
        public string AreaName { get; set; }

        /// <summary>
        /// 父分区名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_v2_parent_name", Required = Required.Default)]
        public string ParentAreaName { get; set; }

        /// <summary>
        /// 父分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_v2_parent_id", Required = Required.Default)]
        public int ParentAreaId { get; set; }

        /// <summary>
        /// 直播标签名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_tag_name", Required = Required.Default)]
        public string LiveTagName { get; set; }

        /// <summary>
        /// 是否为特别关注，0-否，1-是.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "special_attention", Required = Required.Default)]
        public int SpecialAttention { get; set; }

        /// <summary>
        /// 是否官方认证，0-否，1-是.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "official_verify", Required = Required.Default)]
        public int OfficialVerify { get; set; }
    }

    /// <summary>
    /// 直播源关注用户列表.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedFollowUserList
    {
        /// <summary>
        /// 列表数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<LiveFeedRoom> List { get; set; }
    }
}
