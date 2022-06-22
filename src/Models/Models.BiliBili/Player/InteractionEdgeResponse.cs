// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 互动视频选项响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InteractionEdgeResponse
    {
        /// <summary>
        /// 标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 选区列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "edges", Required = Required.Default)]
        public InteractionEdge Edges { get; set; }

        /// <summary>
        /// 隐藏变量.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hidden_vars", Required = Required.Default)]
        public List<InteractionHiddenVariable> HiddenVariables { get; set; }
    }

    /// <summary>
    /// 互动视频选取.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InteractionEdge
    {
        /// <summary>
        /// 互动视频问题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "questions", Required = Required.Default)]
        public List<InteractionQuestion> Questions { get; set; }
    }

    /// <summary>
    /// 互动视频问题.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InteractionQuestion
    {
        /// <summary>
        /// 选项列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "choices", Required = Required.Default)]
        public List<InteractionChoice> Choices { get; set; }
    }

    /// <summary>
    /// 互动视频选项.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InteractionChoice
    {
        /// <summary>
        /// 选项Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public long Id { get; set; }

        /// <summary>
        /// 条件语句.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "condition", Required = Required.Default)]
        public string Condition { get; set; }

        /// <summary>
        /// 对应分P Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cid", Required = Required.Default)]
        public int PartId { get; set; }

        /// <summary>
        /// 选项.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "option", Required = Required.Default)]
        public string Option { get; set; }
    }

    /// <summary>
    /// 隐藏变量.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InteractionHiddenVariable
    {
        /// <summary>
        /// 值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "value", Required = Required.Default)]
        public int Value { get; set; }

        /// <summary>
        /// 标识.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id_v2", Required = Required.Default)]
        public string Id { get; set; }
    }
}
