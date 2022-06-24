// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 粉丝列表响应结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RelatedUserResponse
    {
        /// <summary>
        /// 粉丝列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "list", Required = Required.Default)]
        public List<RelatedUser> UserList { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total", Required = Required.Default)]
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 相关的用户，用于粉丝或关注.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RelatedUser
    {
        /// <summary>
        /// 用户ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public int Mid { get; set; }

        /// <summary>
        /// 关注方式，0-未关注，2-已关注，3-已互粉.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "attribute", Required = Required.Default)]
        public int Attribute { get; set; }

        /// <summary>
        /// 成为粉丝/关注用户的时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mtime", Required = Required.Default)]
        public int StartTime { get; set; }

        /// <summary>
        /// 是否为特别关注，0-是，1-否.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "special", Required = Required.Default)]
        public int Special { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uname", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "face", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 个性签名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sign", Required = Required.Default)]
        public string Sign { get; set; }

        /// <summary>
        /// 会员信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "vip", Required = Required.Default)]
        public Vip Vip { get; set; }
    }

    /// <summary>
    /// 关注分组.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RelatedTag
    {
        /// <summary>
        /// 分组Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tagid", Required = Required.Default)]
        public int TagId { get; set; }

        /// <summary>
        /// 名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "count", Required = Required.Default)]
        public int Count { get; set; }

        /// <summary>
        /// 说明.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tip", Required = Required.Default)]
        public string Tip { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is RelatedTag tag && TagId == tag.TagId;

        /// <inheritdoc/>
        public override int GetHashCode() => 1948533646 + TagId.GetHashCode();
    }
}
