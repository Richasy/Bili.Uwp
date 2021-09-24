// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 未读消息情况.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UnreadMessage
    {
        /// <summary>
        /// @我的.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "at", Required = Required.Default)]
        public int At { get; set; }

        /// <summary>
        /// 聊天消息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "chat", Required = Required.Default)]
        public int Chat { get; set; }

        /// <summary>
        /// 点赞.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "like", Required = Required.Default)]
        public int Like { get; set; }

        /// <summary>
        /// 回复.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reply", Required = Required.Default)]
        public int Reply { get; set; }

        /// <summary>
        /// 系统消息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sys_msg", Required = Required.Default)]
        public int SystemMessage { get; set; }
    }
}
