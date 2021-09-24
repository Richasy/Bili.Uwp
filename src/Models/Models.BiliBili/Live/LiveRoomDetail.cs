// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 直播间详情.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveRoomDetail
    {
        /// <summary>
        /// 房间信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "room_info", Required = Required.Default)]
        public LiveRoomInformation RoomInformation { get; set; }

        /// <summary>
        /// 锚点信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "anchor_info", Required = Required.Default)]
        public LiveAnchorInformation AnchorInformation { get; set; }
    }

    /// <summary>
    /// 直播间信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveRoomInformation
    {
        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uid", Required = Required.Default)]
        public int UserId { get; set; }

        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "room_id", Required = Required.Default)]
        public int RoomId { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 标签.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tags", Required = Required.Default)]
        public string Tags { get; set; }

        /// <summary>
        /// 描述文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "description", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 在线观看人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "online", Required = Required.Default)]
        public int ViewerCount { get; set; }

        /// <summary>
        /// 直播状态：0-未开播，1-正在直播，2-轮播.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_status", Required = Required.Default)]
        public int LiveStatus { get; set; }

        /// <summary>
        /// 直播开始时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_start_time", Required = Required.Default)]
        public int LiveStartTime { get; set; }

        /// <summary>
        /// 直播间封禁状态，0-未封禁，1-已封禁.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "lock_status", Required = Required.Default)]
        public int LockStatus { get; set; }

        /// <summary>
        /// 封禁时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "lock_time", Required = Required.Default)]
        public int LockTime { get; set; }

        /// <summary>
        /// 隐藏状态，0-未隐藏，1-已隐藏.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hidden_status", Required = Required.Default)]
        public int HiddenStatus { get; set; }

        /// <summary>
        /// 隐藏时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hidden_time", Required = Required.Default)]
        public int HiddenTime { get; set; }

        /// <summary>
        /// 所属区域Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_id", Required = Required.Default)]
        public int AreaId { get; set; }

        /// <summary>
        /// 所属区域名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_name", Required = Required.Default)]
        public string AreaName { get; set; }

        /// <summary>
        /// 父分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "parent_area_id", Required = Required.Default)]
        public int ParentAreaId { get; set; }

        /// <summary>
        /// 父分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "parent_area_name", Required = Required.Default)]
        public string ParentAreaName { get; set; }

        /// <summary>
        /// 关键帧（截图）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "keyframe", Required = Required.Default)]
        public string Keyframe { get; set; }

        /// <summary>
        /// 特别关注类型，0-非特别关注，1-特别关注.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "special_type", Required = Required.Default)]
        public int SpecialFollowType { get; set; }
    }

    /// <summary>
    /// 直播间锚点信息.
    /// </summary>
    public class LiveAnchorInformation
    {
        /// <summary>
        /// 房主基本信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "base_info", Required = Required.Default)]
        public LiveUserBasicInformation UserBasicInformation { get; set; }

        /// <summary>
        /// 直播等级信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live_info", Required = Required.Default)]
        public LiveLevelInformation LevelInformation { get; set; }

        /// <summary>
        /// 直播关注信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "relation_info", Required = Required.Default)]
        public LiveRelationInformation RelationInformation { get; set; }

        /// <summary>
        /// 勋章信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "metal_info", Required = Required.Default)]
        public LiveMedalInformation MedalInformation { get; set; }
    }

    /// <summary>
    /// 直播用户基本信息.
    /// </summary>
    public class LiveUserBasicInformation
    {
        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uname", Required = Required.Default)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 性别.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "gender", Required = Required.Default)]
        public string Gender { get; set; }
    }

    /// <summary>
    /// 直播等级信息.
    /// </summary>
    public class LiveLevelInformation
    {
        /// <summary>
        /// 等级.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "level", Required = Required.Default)]
        public int Level { get; set; }
    }

    /// <summary>
    /// 直播关注信息.
    /// </summary>
    public class LiveRelationInformation
    {
        /// <summary>
        /// 关注人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "attention", Required = Required.Default)]
        public int AttentionCount { get; set; }
    }

    /// <summary>
    /// 直播勋章信息.
    /// </summary>
    public class LiveMedalInformation
    {
        /// <summary>
        /// 勋章名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "medal_name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 勋章Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "medal_id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 粉丝俱乐部（已有多少领取了勋章的粉丝）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fansclub", Required = Required.Default)]
        public int FansClub { get; set; }
    }
}
