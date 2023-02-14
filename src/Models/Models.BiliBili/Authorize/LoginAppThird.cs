// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili
{
    /// <summary>
    /// 登录时请求appThird响应信息.
    /// </summary>
    public class LoginAppThird
    {
        /// <summary>
        /// api域.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "api_host", Required = Required.Default)]
        public string ApiHost { get; set; }

        /// <summary>
        /// 是否登录.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "has_login", Required = Required.Default)]
        public int HasLogin { get; set; }

        /// <summary>
        /// 是否直接登录.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "direct_login", Required = Required.Default)]
        public int DirectLogin { get; set; }

        /// <summary>
        /// 确认链接.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "confirm_uri", Required = Required.Default)]
        public string ConfirmUri { get; set; }
    }
}
