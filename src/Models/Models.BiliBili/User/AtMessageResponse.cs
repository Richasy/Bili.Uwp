// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// @我的消息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AtMessageResponse
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
        public List<AtMessageItem> Items { get; set; }
    }

    /// <summary>
    /// @我的消息条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AtMessageItem : MessageItem
    {
        /// <summary>
        /// 用户.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "user", Required = Required.Default)]
        public MessageUser User { get; set; }

        /// <summary>
        /// 消息详情.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "item", Required = Required.Default)]
        public AtMessageItemDetail Item { get; set; }

        /// <summary>
        /// @我的时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "at_time", Required = Required.Default)]
        public long AtTime { get; set; }
    }

    /// <summary>
    /// @我的消息条目详情.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AtMessageItemDetail : MessageItemDetail
    {
        /// <summary>
        /// 主题Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subject_id", Required = Required.Default)]
        public long SubjectId { get; set; }

        /// <summary>
        /// 源对象Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "source_id", Required = Required.Default)]
        public long SourceId { get; set; }

        /// <summary>
        /// 源内容.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "source_content", Required = Required.Default)]
        public string SourceContent { get; set; }

        /// <summary>
        /// At的人的信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "at_details", Required = Required.Default)]
        public List<MessageUser> AtDetails { get; set; }
    }
}
