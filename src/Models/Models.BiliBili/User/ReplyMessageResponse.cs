// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 评论消息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ReplyMessageResponse
    {
        /// <summary>
        /// 偏移指针.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cursor", Required = Required.Default)]
        public MessageCursor Cursor { get; set; }

        /// <summary>
        /// 条目列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<ReplyMessageItem> Items { get; set; }

        /// <summary>
        /// 上次查看时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "last_view_at", Required = Required.Default)]
        public int LastViewTime { get; set; }
    }

    /// <summary>
    /// 评论消息条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ReplyMessageItem : MessageItem
    {
        /// <summary>
        /// 评论的用户.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "user", Required = Required.Default)]
        public MessageUser User { get; set; }

        /// <summary>
        /// 评论消息详情.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "item", Required = Required.Default)]
        public ReplyMessageItemDetail Item { get; set; }

        /// <summary>
        /// 评论人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "counts", Required = Required.Default)]
        public int Counts { get; set; }

        /// <summary>
        /// 是否为多人评论.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_multi", Required = Required.Default)]
        public int IsMultiple { get; set; }

        /// <summary>
        /// 评论时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reply_time", Required = Required.Default)]
        public int ReplyTime { get; set; }
    }

    /// <summary>
    /// 评论消息条目详情.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ReplyMessageItemDetail : AtMessageItemDetail
    {
        /// <summary>
        /// 描述.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "desc", Required = Required.Default)]
        public string Description { get; set; }
    }
}
