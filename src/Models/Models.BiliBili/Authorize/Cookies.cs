using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
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
