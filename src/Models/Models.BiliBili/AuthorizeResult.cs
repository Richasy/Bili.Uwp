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

    /// <summary>
    /// 令牌信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TokenInfo
    {
        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public long Mid { get; set; }

        /// <summary>
        /// 访问令牌.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "access_token", Required = Required.Default)]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "refresh_token", Required = Required.Default)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "expires_in", Required = Required.Default)]
        public int ExpiresIn { get; set; }
    }

    /// <summary>
    /// Cookies.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Cookies
    {
        /// <summary>
        /// 名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 值.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "value", Required = Required.Default)]
        public string Value { get; set; }

        /// <summary>
        /// 仅限http.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "http_only", Required = Required.Default)]
        public int HttpOnly { get; set; }

        /// <summary>
        /// 过期时间.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "expires", Required = Required.Default)]
        public int Expires { get; set; }
    }

    /// <summary>
    /// Cookie信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CookieInfo
    {
        /// <summary>
        /// Cookie列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cookies", Required = Required.Default)]
        public List<Cookies> Cookies { get; set; }

        /// <summary>
        /// 域名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "domains", Required = Required.Default)]
        public List<string> Domains { get; set; }
    }
}
