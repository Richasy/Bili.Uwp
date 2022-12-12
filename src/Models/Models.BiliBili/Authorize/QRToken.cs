// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Bili.Models.BiliBili.Authorize
{
    /// <summary>
    /// 通过扫码登录获得的令牌信息.
    /// </summary>
    public sealed class QRToken
    {
        /// <summary>
        /// 扫码状态信息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message", Required = Required.Default)]
        public string Message { get; set; }

        /// <summary>
        /// 刷新令牌.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "refresh_token", Required = Required.Default)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 登录时间（毫秒时间戳）.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "timestamp", Required = Required.Default)]
        public long Timestamp { get; set; }

        /// <summary>
        /// 扫码结果代号.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "code", Required = Required.Default)]
        public int Code { get; set; }

        /// <summary>
        /// 游戏分站跨域登录 url.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Required.Default)]
        public string GameUrl { get; set; }
    }
}
