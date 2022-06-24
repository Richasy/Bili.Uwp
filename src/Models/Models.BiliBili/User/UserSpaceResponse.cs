// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 用户空间信息响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceResponse
    {
        /// <summary>
        /// 用户信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "card", Required = Required.Default)]
        public UserSpaceInformation User { get; set; }

        /// <summary>
        /// 直播信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "live", Required = Required.Default)]
        public UserSpaceLive Live { get; set; }

        /// <summary>
        /// 视频集.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "archive", Required = Required.Default)]
        public UserSpaceVideoSet VideoSet { get; set; }

        /// <summary>
        /// 文章集.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "article", Required = Required.Default)]
        public UserSpaceArticleSet ArticleSet { get; set; }
    }

    /// <summary>
    /// 用户空间信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceInformation
    {
        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public string UserId { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string UserName { get; set; }

        /// <summary>
        /// 性别.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sex", Required = Required.Default)]
        public string Sex { get; set; }

        /// <summary>
        /// 头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 粉丝数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fans", Required = Required.Default)]
        public int FollowerCount { get; set; }

        /// <summary>
        /// 关注数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "attention", Required = Required.Default)]
        public int FollowCount { get; set; }

        /// <summary>
        /// 个性签名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sign", Required = Required.Default)]
        public string Sign { get; set; }

        /// <summary>
        /// 等级信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "level_info", Required = Required.Default)]
        public UserSpaceLevelInformation LevelInformation { get; set; }

        /// <summary>
        /// 大会员信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "vip", Required = Required.Default)]
        public Vip Vip { get; set; }

        /// <summary>
        /// 关系.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "relation", Required = Required.Default)]
        public UserRelation Relation { get; set; }

        /// <summary>
        /// 点赞信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "likes", Required = Required.Default)]
        public UserSpaceLikeInformation LikeInformation { get; set; }
    }

    /// <summary>
    /// 用户空间获赞信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceLikeInformation
    {
        /// <summary>
        /// 点赞数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like_num", Required = Required.Default)]
        public int LikeCount { get; set; }
    }

    /// <summary>
    /// 用户空间的等级信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceLevelInformation
    {
        /// <summary>
        /// 用户当前等级.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "current_level", Required = Required.Default)]
        public int CurrentLevel { get; set; }

        /// <summary>
        /// 当前经验值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "current_exp", Required = Required.Default)]
        public int CurrentExperience { get; set; }

        /// <summary>
        /// 达到下一等级所需经验值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "next_exp", Required = Required.Default)]
        public string NextExperience { get; set; }
    }

    /// <summary>
    /// 用户空间的直播信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceLive
    {
        /// <summary>
        /// 直播间状态，0-未开播，1-正在直播.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "roomStatus", Required = Required.Default)]
        public int RoomStatus { get; set; }

        /// <summary>
        /// 直播状态，0-未开播，1-正在直播，2-轮播.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "liveStatus", Required = Required.Default)]
        public int LiveStatus { get; set; }

        /// <summary>
        /// 直播地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string Url { get; set; }

        /// <summary>
        /// 直播标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 在线观看人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "online", Required = Required.Default)]
        public int ViewerCount { get; set; }

        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "roomid", Required = Required.Default)]
        public int RoomId { get; set; }
    }

    /// <summary>
    /// 用户空间视频集.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceVideoSet
    {
        /// <summary>
        /// 视频总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 视频列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "item", Required = Required.Default)]
        public List<UserSpaceVideoItem> List { get; set; }
    }

    /// <summary>
    /// 用户空间视频条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceVideoItem
    {
        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 分区名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tname", Required = Required.Default)]
        public string PartitionName { get; set; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 视频Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "param", Required = Required.Default)]
        public string Id { get; set; }

        /// <summary>
        /// 目标类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "goto", Required = Required.Default)]
        public string Goto { get; set; }

        /// <summary>
        /// 时长.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "duration", Required = Required.Default)]
        public int Duration { get; set; }

        /// <summary>
        /// 是否为合作视频.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_cooperation", Required = Required.Default)]
        public bool IsCooperation { get; set; }

        /// <summary>
        /// 是否为PGC内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_pgc", Required = Required.Default)]
        public bool IsPgc { get; set; }

        /// <summary>
        /// 是否为直播回放.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_live_playback", Required = Required.Default)]
        public bool IsLivePlayback { get; set; }

        /// <summary>
        /// 播放次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play", Required = Required.Default)]
        public int PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmaku", Required = Required.Default)]
        public int DanmakuCount { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ctime", Required = Required.Default)]
        public int CreateTime { get; set; }

        /// <summary>
        /// 发布者名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "author", Required = Required.Default)]
        public string PublisherName { get; set; }

        /// <summary>
        /// Bv Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bvid", Required = Required.Default)]
        public string Bvid { get; set; }

        /// <summary>
        /// 首个分P的Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "first_cid", Required = Required.Default)]
        public int FirstCid { get; set; }
    }

    /// <summary>
    /// 用户空间文章集.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSpaceArticleSet
    {
        /// <summary>
        /// 文章个数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 文章列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "item", Required = Required.Default)]
        public List<Article> List { get; set; }
    }
}
