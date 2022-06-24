// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.App.Other
{
    /// <summary>
    /// 问题模块.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class QuestionModule
    {
        /// <summary>
        /// 模块标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 模块名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 问题列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "items", Required = Required.Default)]
        public List<QuestionItem> Questions { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is QuestionModule module && Id == module.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }

    /// <summary>
    /// 问题条目.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class QuestionItem
    {
        /// <summary>
        /// 问题标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        /// <summary>
        /// 问题标题.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 问题说明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "answer", Required = Required.Default)]
        public string Answer { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is QuestionItem item && Id == item.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }
}
