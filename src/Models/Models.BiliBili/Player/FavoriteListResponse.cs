// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 收藏夹列表响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteListResponse
    {
        /// <summary>
        /// 收藏夹总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 收藏夹列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<FavoriteMeta> List { get; set; }
    }

    /// <summary>
    /// 收藏夹元数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FavoriteMeta
    {
        /// <summary>
        /// 收藏夹完整Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public long Id { get; set; }

        /// <summary>
        /// 收藏夹原始Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fid", Required = Required.Default)]
        public long FolderId { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public long UserId { get; set; }

        /// <summary>
        /// 收藏夹标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 查询的视频是否在该收藏夹内，0-不存在，1-存在.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fav_state", Required = Required.Default)]
        public int FavoriteState { get; set; }

        /// <summary>
        /// 媒体数目.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "media_count", Required = Required.Default)]
        public int MediaCount { get; set; }
    }
}
