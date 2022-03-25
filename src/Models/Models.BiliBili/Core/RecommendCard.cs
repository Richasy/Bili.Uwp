// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 推荐卡片的定义.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RecommendCard
    {
        /// <summary>
        /// 卡片类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "card_type", Required = Required.Default)]
        public string CardType { get; set; }

        /// <summary>
        /// 处理卡片的程序.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "card_goto", Required = Required.Default)]
        public string CardGoto { get; set; }

        /// <summary>
        /// 卡片参数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "args", Required = Required.Default)]
        public RecommendCardArgs CardArgs { get; set; }

        /// <summary>
        /// 偏移值标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "idx", Required = Required.Default)]
        public int Index { get; set; }

        /// <summary>
        /// 上下文菜单项列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "three_point_v2", Required = Required.Default)]
        public List<RecommendContextMenuItem> ContextMenuItems { get; set; }

        /// <summary>
        /// 需要播放的类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "goto", Required = Required.Default)]
        public string Goto { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 导航地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uri", Required = Required.Default)]
        public string NavigateUri { get; set; }

        /// <summary>
        /// 视频或番剧的Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "param", Required = Required.Default)]
        public string Parameter { get; set; }

        /// <summary>
        /// 播放参数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "player_args", Required = Required.Default)]
        public PlayerArgs PlayerArgs { get; set; }

        /// <summary>
        /// 播放数文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover_left_text_2", Required = Required.Default)]
        public string PlayCountText { get; set; }

        /// <summary>
        /// 状态副文本，对于视频来说是弹幕数，对于番剧来说是点赞数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover_left_text_3", Required = Required.Default)]
        public string SubStatusText { get; set; }

        /// <summary>
        /// 时长文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover_left_text_1", Required = Required.Default)]
        public string DurationText { get; set; }

        /// <summary>
        /// 推荐原因.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "top_rcmd_reason", Required = Required.Default)]
        public string RecommendReason { get; set; }

        /// <summary>
        /// 是否可以播放，1-可以.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "can_play", Required = Required.Default)]
        public int CanPlay { get; set; }

        /// <summary>
        /// 说明文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 遮罩内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mask", Required = Required.Default)]
        public RecommendCardMask Mask { get; set; }
    }

    /// <summary>
    /// 推荐卡片的参数.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RecommendCardArgs
    {
        /// <summary>
        /// 发布者Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "up_id", Required = Required.Default)]
        public int PublisherId { get; set; }

        /// <summary>
        /// 发布者名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "up_name", Required = Required.Default)]
        public string PublisherName { get; set; }

        /// <summary>
        /// 分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rid", Required = Required.Default)]
        public int PartitionId { get; set; }

        /// <summary>
        /// 分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rname", Required = Required.Default)]
        public string PartitionName { get; set; }

        /// <summary>
        /// 子分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tid", Required = Required.Default)]
        public int SubPartitionId { get; set; }

        /// <summary>
        /// 子分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tname", Required = Required.Default)]
        public string SubPartitionName { get; set; }

        /// <summary>
        /// 视频Aid.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public int Aid { get; set; }
    }

    /// <summary>
    /// 播放器参数.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayerArgs
    {
        /// <summary>
        /// 视频的Aid.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public int Aid { get; set; }

        /// <summary>
        /// 视频第一个分P的Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
        public int Cid { get; set; }

        /// <summary>
        /// 视频类型，一般为av.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }

        /// <summary>
        /// 视频时长 (秒).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "duration", Required = Required.Default)]
        public int Duration { get; set; }
    }

    /// <summary>
    /// 推荐视频的上下文菜单内容.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RecommendContextMenuItem
    {
        /// <summary>
        /// 显示标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 菜单项类型。watch_later:稍后再看. feedback:反馈. dislike:不喜欢.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subtitle", Required = Required.Default)]
        public string Subtitle { get; set; }

        /// <summary>
        /// 原因列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reasons", Required = Required.Default)]
        public List<RecommendDislikeReason> Reasons { get; set; }
    }

    /// <summary>
    /// 推荐视频的不喜欢原因.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RecommendDislikeReason
    {
        /// <summary>
        /// 原因标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 显示的文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 提示文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "toast", Required = Required.Default)]
        public string Tip { get; set; }
    }

    /// <summary>
    /// 推荐卡片的遮罩内容.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RecommendCardMask
    {
        /// <summary>
        /// 推荐卡片的头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "avatar", Required = Required.Default)]
        public RecommendAvatar Avatar { get; set; }
    }

    /// <summary>
    /// 推荐卡片的头像.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RecommendAvatar
    {
        /// <summary>
        /// 头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Text", Required = Required.Default)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "up_id", Required = Required.Default)]
        public int UserId { get; set; }
    }
}
