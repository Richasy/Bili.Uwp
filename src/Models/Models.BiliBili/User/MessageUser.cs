// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 发出消息的用户.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MessageUser
    {
        /// <summary>
        /// 用户ID.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public long UserId { get; set; }

        /// <summary>
        /// 是否为粉丝，0-不是，1-是.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fans", Required = Required.Default)]
        public int IsFans { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "nickname", Required = Required.Default)]
        public string UserName { get; set; }

        /// <summary>
        /// 头像.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "avatar", Required = Required.Default)]
        public string Avatar { get; set; }

        /// <summary>
        /// 是否关注了该用户.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "follow", Required = Required.Default)]
        public bool IsFollow { get; set; }
    }
}
