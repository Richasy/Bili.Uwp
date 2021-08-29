// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// PGC索引筛选条件响应.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcIndexConditionResponse
    {
        /// <summary>
        /// 筛选条件.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "filter", Required = Required.Default)]
        public List<PgcIndexFilter> FilterList { get; set; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "order", Required = Required.Default)]
        public List<PgcIndexOrder> OrderList { get; set; }
    }

    /// <summary>
    /// PGC索引筛选条件.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcIndexFilter
    {
        /// <summary>
        /// 筛选关键词.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "field", Required = Required.Default)]
        public string Field { get; set; }

        /// <summary>
        /// 筛选条目名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 可选值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "values", Required = Required.Default)]
        public List<PgcIndexFilterValue> Values { get; set; }
    }

    /// <summary>
    /// PGC索引筛选条件可选值.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcIndexFilterValue
    {
        /// <summary>
        /// 关键词.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "keyword", Required = Required.Default)]
        public string Keyword { get; set; }

        /// <summary>
        /// 显示名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }
    }

    /// <summary>
    /// PGC索引排序条件.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PgcIndexOrder
    {
        /// <summary>
        /// 排序关键词.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "field", Required = Required.Default)]
        public string Field { get; set; }

        /// <summary>
        /// 排序名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }
    }
}
