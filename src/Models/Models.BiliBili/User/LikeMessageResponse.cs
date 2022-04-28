// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 点赞的消息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LikeMessageResponse
    {
        /// <summary>
        /// 最新消息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "lastest", Required = Required.Default)]
        public LikeMesageLatest Latest { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total", Required = Required.Default)]
        public LikeMessageTotal Total { get; set; }
    }

    /// <summary>
    /// 最新的点赞消息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LikeMesageLatest
    {
        /// <summary>
        /// 消息列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<MessageItem> Items { get; set; }

        /// <summary>
        /// 上次查看时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "last_view_at", Required = Required.Default)]
        public int LastViewTime { get; set; }
    }

    /// <summary>
    /// 点赞消息条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LikeMessageItem : MessageItem
    {
        /// <summary>
        /// 该消息内包含的点赞人信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "users", Required = Required.Default)]
        public List<MessageUser> Users { get; set; }

        /// <summary>
        /// 点赞消息详情.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "item", Required = Required.Default)]
        public LikeMessageItemDetail Item { get; set; }

        /// <summary>
        /// 点赞人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "counts", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 点赞时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like_time", Required = Required.Default)]
        public int LikeTime { get; set; }

        /// <summary>
        /// 是否是最新消息（应用内赋值）.
        /// </summary>
        public bool IsLatest { get; set; }
    }

    /// <summary>
    /// 点赞消息详情.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LikeMessageItemDetail : MessageItemDetail
    {
        /// <summary>
        /// 条目Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "item_id", Required = Required.Default)]
        public long ItemId { get; set; }

        /// <summary>
        /// 描述.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }
    }

    /// <summary>
    /// 历史点赞消息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LikeMessageTotal
    {
        /// <summary>
        /// 偏移指针.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cursor", Required = Required.Default)]
        public MessageCursor Cursor { get; set; }

        /// <summary>
        /// 消息列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<LikeMessageItem> Items { get; set; }
    }
}
