// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// PGC内容详情.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcDisplayInformation
    {
        /// <summary>
        /// 演员表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "actor", Required = Required.Default)]
        public PgcStaff Actor { get; set; }

        /// <summary>
        /// 昵称，假名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "alias", Required = Required.Default)]
        public string Alias { get; set; }

        /// <summary>
        /// 所属地区.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "areas", Required = Required.Default)]
        public List<PgcArea> Areas { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string BadgeText { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 动态副标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dynamic_subtitle", Required = Required.Default)]
        public string DynamicSubtitle { get; set; }

        /// <summary>
        /// 评价，说明文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "evaluate", Required = Required.Default)]
        public string Evaluate { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 媒体Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "media_id", Required = Required.Default)]
        public int MediaId { get; set; }

        /// <summary>
        /// 模式，用途不明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mode", Required = Required.Default)]
        public int Mode { get; set; }

        /// <summary>
        /// 模块.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "modules", Required = Required.Default)]
        public List<PgcDetailModule> Modules { get; set; }

        /// <summary>
        /// 原名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "origin_name", Required = Required.Default)]
        public string OriginName { get; set; }

        /// <summary>
        /// 发布信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "publish", Required = Required.Default)]
        public PgcPublishInformation PublishInformation { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public int SeasonId { get; set; }

        /// <summary>
        /// 剧集标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_title", Required = Required.Default)]
        public string SeasonTitle { get; set; }

        /// <summary>
        /// 系列.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "series", Required = Required.Default)]
        public PgcSeries Series { get; set; }

        /// <summary>
        /// 分享标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share_copy", Required = Required.Default)]
        public string ShareTitle { get; set; }

        /// <summary>
        /// 分享链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share_url", Required = Required.Default)]
        public string ShareUrl { get; set; }

        /// <summary>
        /// 短链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "short_link", Required = Required.Default)]
        public string ShortLink { get; set; }

        /// <summary>
        /// 矩形封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "square_cover", Required = Required.Default)]
        public string SquareCover { get; set; }

        /// <summary>
        /// 参与人员，制作信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "staff", Required = Required.Default)]
        public PgcStaff Staff { get; set; }

        /// <summary>
        /// 详情互动数据（包括投币数，观看数等）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "stat", Required = Required.Default)]
        public PgcInformationStat InformationStat { get; set; }

        /// <summary>
        /// 状态（数值含义不明）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Required.Default)]
        public int Status { get; set; }

        /// <summary>
        /// 索引筛选列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "styles", Required = Required.Default)]
        public List<PgcIndex> IndexList { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subtitle", Required = Required.Default)]
        public string Subtitle { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 总集数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total", Required = Required.Default)]
        public int TotalCount { get; set; }

        /// <summary>
        /// 类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public int Type { get; set; }

        /// <summary>
        /// 类型说明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type_desc", Required = Required.Default)]
        public string TypeDescription { get; set; }

        /// <summary>
        /// 类型名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type_name", Required = Required.Default)]
        public string TypeName { get; set; }

        /// <summary>
        /// 用户状态，包括是否已追番及播放进度等.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "user_status", Required = Required.Default)]
        public PgcUserStatus UserStatus { get; set; }

        /// <summary>
        /// 动态标签页.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "activity_tab", Required = Required.Default)]
        public PgcActivityTab ActivityTab { get; set; }

        /// <summary>
        /// 评分.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "rating", Required = Required.Default)]
        public PgcRating Rating { get; set; }

        /// <summary>
        /// 演职人员.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "celebrity", Required = Required.Default)]
        public List<PgcCelebrity> Celebrity { get; set; }

        /// <summary>
        /// 警示信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dialog", Required = Required.Default)]
        public PgcPlayerDialog Warning { get; set; }
    }

    /// <summary>
    /// 播放器警告.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcPlayerDialog
    {
        /// <summary>
        /// 警告代号.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "code", Required = Required.Default)]
        public int Code { get; set; }

        /// <summary>
        /// 警告信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "msg", Required = Required.Default)]
        public string Message { get; set; }

        /// <summary>
        /// 警告类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public string Type { get; set; }
    }

    /// <summary>
    /// 发布详情.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcPublishInformation
    {
        /// <summary>
        /// 是否已完结，0-未完结，1-已完结.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_finish", Required = Required.Default)]
        public int IsFinish { get; set; }

        /// <summary>
        /// 是否已开始连载.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_started", Required = Required.Default)]
        public int IsStarted { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pub_time", Required = Required.Default)]
        public string PublishTime { get; set; }

        /// <summary>
        /// 显示的可读发布时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pub_time_show", Required = Required.Default)]
        public string DisplayPublishTime { get; set; }

        /// <summary>
        /// 显示的可读发布日期.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "release_date_show", Required = Required.Default)]
        public string DisplayReleaseDate { get; set; }

        /// <summary>
        /// 显示的连载进度.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "time_length_show", Required = Required.Default)]
        public string DisplayProgress { get; set; }

        /// <summary>
        /// 未知发布时间. 0-已知，1-未知.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "unknow_pub_date", Required = Required.Default)]
        public int UnknowPublishDate { get; set; }
    }

    /// <summary>
    /// PGC单集信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcEpisodeDetail
    {
        /// <summary>
        /// 视频Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public int Aid { get; set; }

        /// <summary>
        /// BV Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bvid", Required = Required.Default)]
        public string BvId { get; set; }

        /// <summary>
        /// 分P Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
        public int PartId { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 时长.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "duration", Required = Required.Default)]
        public int Duration { get; set; }

        /// <summary>
        /// 分集索引.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ep_index", Required = Required.Default)]
        public int Index { get; set; }

        /// <summary>
        /// 分集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 长标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "long_title", Required = Required.Default)]
        public string LongTitle { get; set; }

        /// <summary>
        /// 发布时间（秒）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pub_time", Required = Required.Default)]
        public int PublishTime { get; set; }

        /// <summary>
        /// 是否为PV？ 0-不是，1-是.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pv", Required = Required.Default)]
        public int IsPV { get; set; }

        /// <summary>
        /// 当前分块内的索引.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "section_index", Required = Required.Default)]
        public int SectionIndex { get; set; }

        /// <summary>
        /// 分享标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share_copy", Required = Required.Default)]
        public string ShareTitle { get; set; }

        /// <summary>
        /// 分享链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share_url", Required = Required.Default)]
        public string ShareUrl { get; set; }

        /// <summary>
        /// 短链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "short_link", Required = Required.Default)]
        public string ShortLink { get; set; }

        /// <summary>
        /// 状态.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Required.Default)]
        public int Status { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subtitle", Required = Required.Default)]
        public string Subtitle { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 报告信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "report", Required = Required.Default)]
        public PgcModuleReport Report { get; set; }
    }

    /// <summary>
    /// PGC系列.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcSeries
    {
        /// <summary>
        /// 系列Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "series_id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "series_title", Required = Required.Default)]
        public string Title { get; set; }
    }

    /// <summary>
    /// PGC内容的用户交互数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcUserStatus
    {
        /// <summary>
        /// 是否已关注/追番. 0-没有，1-已追.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow", Required = Required.Default)]
        public int IsFollow { get; set; }

        /// <summary>
        /// 付费状态. 0-未付费，1-已付费.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pay", Required = Required.Default)]
        public int Pay { get; set; }

        /// <summary>
        /// 播放历史记录.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "progress", Required = Required.Default)]
        public PgcProgress Progress { get; set; }

        /// <summary>
        /// 是否需要大会员. 0-不需要，1-需要.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "vip", Required = Required.Default)]
        public int IsVip { get; set; }
    }

    /// <summary>
    /// PGC内容制作人员信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcStaff
    {
        /// <summary>
        /// 内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "info", Required = Required.Default)]
        public string Information { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }
    }

    /// <summary>
    /// PGC详情信息的社区数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcInformationStat
    {
        /// <summary>
        /// 投币数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "coins", Required = Required.Default)]
        public int CoinCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmakus", Required = Required.Default)]
        public int DanmakuCount { get; set; }

        /// <summary>
        /// 单集收藏数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favorite", Required = Required.Default)]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// 系列追番/收藏数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "favorites", Required = Required.Default)]
        public int SeriesFavoriteCount { get; set; }

        /// <summary>
        /// 追番/收藏显示文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "followers", Required = Required.Default)]
        public string FollowerDisplayText { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "likes", Required = Required.Default)]
        public int LikeCount { get; set; }

        /// <summary>
        /// 播放量显示文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play", Required = Required.Default)]
        public string PlayDisplayText { get; set; }

        /// <summary>
        /// 回复数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reply", Required = Required.Default)]
        public int ReplyCount { get; set; }

        /// <summary>
        /// 分享数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "share", Required = Required.Default)]
        public int ShareCount { get; set; }

        /// <summary>
        /// 播放次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "views", Required = Required.Default)]
        public int PlayCount { get; set; }
    }

    /// <summary>
    /// PGC内容历史记录.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcProgress
    {
        /// <summary>
        /// 最后一次播放的单集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "last_ep_id", Required = Required.Default)]
        public int LastEpisodeId { get; set; }

        /// <summary>
        /// 最后一次播放的单集索引.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "last_ep_index", Required = Required.Default)]
        public string LastEpisodeIndex { get; set; }

        /// <summary>
        /// 播放进度（秒）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "last_time", Required = Required.Default)]
        public int LastTime { get; set; }
    }

    /// <summary>
    /// PGC动态标签.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcActivityTab
    {
        /// <summary>
        /// 标签Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 显示名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "show_name", Required = Required.Default)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 全称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 类别（目前仅处理101）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public int Type { get; set; }
    }

    /// <summary>
    /// PGC内容评分.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcRating
    {
        /// <summary>
        /// 评分人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 综合评分.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "score", Required = Required.Default)]
        public double Score { get; set; }
    }

    /// <summary>
    /// PGC内容所属地区.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcArea
    {
        /// <summary>
        /// 地区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 属地名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }
    }

    /// <summary>
    /// PGC详情的模块.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcDetailModule
    {
        /// <summary>
        /// 数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "data", Required = Required.Default)]
        public PgcDetailModuleData Data { get; set; }

        /// <summary>
        /// 未知发布时间. 0-已知，1-未知.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 样式.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "style", Required = Required.Default)]
        public string Style { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }
    }

    /// <summary>
    /// PGC详情模块数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcDetailModuleData
    {
        /// <summary>
        /// 剧集列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "episodes", Required = Required.Default)]
        public List<PgcEpisodeDetail> Episodes { get; set; }

        /// <summary>
        /// 关联系列.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "seasons", Required = Required.Default)]
        public List<PgcSeason> Seasons { get; set; }

        /// <summary>
        /// 模块Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }
    }

    /// <summary>
    /// PGC剧集系列.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcSeason
    {
        /// <summary>
        /// 徽章文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "badge", Required = Required.Default)]
        public string Badge { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 是否是最新. 0-不是，1-是.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_new", Required = Required.Default)]
        public int IsNew { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 最新内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "new_ep", Required = Required.Default)]
        public PgcEpisode NewEpisode { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public int SeasonId { get; set; }

        /// <summary>
        /// 剧集标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_title", Required = Required.Default)]
        public string SeasonTitle { get; set; }

        /// <summary>
        /// 标题全称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }
    }

    /// <summary>
    /// PGC关联索引.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcIndex
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 导航地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string Url { get; set; }
    }

    /// <summary>
    /// 演职人员.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcCelebrity
    {
        /// <summary>
        /// 头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "avatar", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 说明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 演职员Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 演职员姓名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 简短介绍.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "short_desc", Required = Required.Default)]
        public string ShortDescription { get; set; }
    }

    /// <summary>
    /// PGC模块报告数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcModuleReport
    {
        /// <summary>
        /// Aid.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "aid", Required = Required.Default)]
        public string Aid { get; set; }

        /// <summary>
        /// 分集标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ep_title", Required = Required.Default)]
        public string EpisodeTitle { get; set; }

        /// <summary>
        /// 分集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "epid", Required = Required.Default)]
        public string EpisodeId { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public string SeasonId { get; set; }

        /// <summary>
        /// 剧集类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_type", Required = Required.Default)]
        public string SeasonType { get; set; }

        /// <summary>
        /// 分块Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "section_id", Required = Required.Default)]
        public string SectionId { get; set; }

        /// <summary>
        /// 分块类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "section_type", Required = Required.Default)]
        public string SectionType { get; set; }
    }
}
