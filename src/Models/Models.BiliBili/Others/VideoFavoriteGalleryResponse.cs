// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 视频收藏概览响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class VideoFavoriteGalleryResponse
    {
        /// <summary>
        /// 收藏夹列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "space_infos", Required = Required.Default)]
        public List<FavoriteFolder> FavoriteFolderList { get; set; }

        /// <summary>
        /// 默认收藏夹.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "default_folder", Required = Required.Default)]
        public VideoFavoriteListResponse DefaultFavoriteList { get; set; }
    }

    /// <summary>
    /// 视频默认收藏夹.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class VideoFavoriteListResponse
    {
        /// <summary>
        /// 收藏夹信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "folder_detail", Required = Required.Default)]
        public FavoriteListDetail Detail { get; set; }

        /// <summary>
        /// 收藏夹信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "info", Required = Required.Default)]
        public FavoriteListDetail Information { get; set; }

        /// <summary>
        /// 收藏夹的媒体列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "medias", Required = Required.Default)]
        public List<FavoriteMedia> Medias { get; set; }

        /// <summary>
        /// 是否有更多.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "has_more", Required = Required.Default)]
        public bool HasMore { get; set; }
    }

    /// <summary>
    /// 收藏夹详情.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteListDetail
    {
        /// <summary>
        /// 收藏夹完整ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public long Id { get; set; }

        /// <summary>
        /// 收藏夹原始ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fid", Required = Required.Default)]
        public long OriginId { get; set; }

        /// <summary>
        /// 用户ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public long Mid { get; set; }

        /// <summary>
        /// 收藏夹标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 创建收藏夹的用户信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "upper", Required = Required.Default)]
        public PublisherInfo Publisher { get; set; }

        /// <summary>
        /// 说明/备注.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "intro", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ctime", Required = Required.Default)]
        public int CreateTime { get; set; }

        /// <summary>
        /// 收藏时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mtime", Required = Required.Default)]
        public int CollectTime { get; set; }

        /// <summary>
        /// 收藏夹收藏状态，1-已收藏，0-未收藏.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fav_state", Required = Required.Default)]
        public int FavoriteState { get; set; }

        /// <summary>
        /// 内容数目.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "media_count", Required = Required.Default)]
        public int MediaCount { get; set; }

        /// <summary>
        /// 查看次数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "view_count", Required = Required.Default)]
        public int ViewCount { get; set; }
    }

    /// <summary>
    /// 收藏夹媒体.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteMedia
    {
        /// <summary>
        /// 媒体Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public long Id { get; set; }

        /// <summary>
        /// 媒体类型，2-视频，12-音频，21-视频合集.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
        public int Type { get; set; }

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
        /// 媒体说明文本.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "intro", Required = Required.Default)]
        public string Description { get; set; }

        /// <summary>
        /// 页码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "page", Required = Required.Default)]
        public int Page { get; set; }

        /// <summary>
        /// 时长.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "duration", Required = Required.Default)]
        public int Duration { get; set; }

        /// <summary>
        /// 发布者.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "upper", Required = Required.Default)]
        public PublisherInfo Publisher { get; set; }

        /// <summary>
        /// 是否有效，0-有效，1-无效.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "attr", Required = Required.Default)]
        public int IsValid { get; set; }

        /// <summary>
        /// 用户交互数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cnt_info", Required = Required.Default)]
        public FavoriteMediaStat Stat { get; set; }

        /// <summary>
        /// 网址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "link", Required = Required.Default)]
        public string Link { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ctime", Required = Required.Default)]
        public long CreateTime { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "pubtime", Required = Required.Default)]
        public long PublishTime { get; set; }

        /// <summary>
        /// 收藏时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fav_time", Required = Required.Default)]
        public long FavoriteTime { get; set; }

        /// <summary>
        /// Bv Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "bvid", Required = Required.Default)]
        public string BvId { get; set; }

        /// <summary>
        /// 社区信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ugc", Required = Required.Default)]
        public FavoriteUgcInformation UgcInformation { get; set; }
    }

    /// <summary>
    /// 收藏夹媒体用户交互数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteMediaStat
    {
        /// <summary>
        /// 收藏数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "collect", Required = Required.Default)]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// 播放数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "play", Required = Required.Default)]
        public int PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "danmaku", Required = Required.Default)]
        public int DanmakuCount { get; set; }
    }

    /// <summary>
    /// 收藏夹UGC内容信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteUgcInformation
    {
        /// <summary>
        /// 首个分P Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "first_cid", Required = Required.Default)]
        public int FirstCid { get; set; }
    }

    /// <summary>
    /// 收藏夹分类.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteFolder
    {
        /// <summary>
        /// 收藏夹所属分类Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public long Id { get; set; }

        /// <summary>
        /// 分类名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 媒体列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mediaListResponse", Required = Required.Default)]
        public FavoriteMediaList MediaList { get; set; }
    }

    /// <summary>
    /// 收藏夹媒体列表.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteMediaList
    {
        /// <summary>
        /// 个数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 媒体列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<FavoriteListDetail> List { get; set; }

        /// <summary>
        /// 是否有更多.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "has_more", Required = Required.Default)]
        public bool HasMore { get; set; }
    }
}
