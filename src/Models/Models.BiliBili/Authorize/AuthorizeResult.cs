// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 授权结果.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AuthorizeResult
    {
        /// <summary>
        /// 状态码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Required.Default)]
        public int Status { get; set; }

        /// <summary>
        /// 错误消息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message", Required = Required.Default)]
        public string Message { get; set; }

        /// <summary>
        /// 导航地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string Url { get; set; }

        /// <summary>
        /// 授权令牌信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "token_info", Required = Required.Default)]
        public TokenInfo TokenInfo { get; set; }

        /// <summary>
        /// Cookie信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cookie_info", Required = Required.Default)]
        public CookieInfo CookieInfo { get; set; }

        /// <summary>
        /// SSO.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "sso", Required = Required.Default)]
        public List<string> SSO { get; set; }
    }
}
