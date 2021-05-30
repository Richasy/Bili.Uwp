// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models
{
    /// <summary>
    /// 哔哩哔哩服务器返回的数据响应结构类型.
    /// </summary>
    /// <typeparam name="T"><see cref="Data"/>对应的类型.</typeparam>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ServerResponse<T>
    {
        /// <summary>
        /// 响应代码.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "code", Required = Required.Default)]
        public int Code { get; set; }

        /// <summary>
        /// 响应消息.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message", Required = Required.Default)]
        public string Message { get; set; }

        /// <summary>
        /// TTL.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ttl", Required = Required.Default)]
        public int TTL { get; set; }

        /// <summary>
        /// 响应返回的数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "data", Required = Required.Default)]
        public T Data { get; set; }

        /// <summary>
        /// 响应结果是否为成功.
        /// </summary>
        /// <returns>成功或失败.</returns>
        public bool IsSuccess()
        {
            return Code == 0;
        }
    }
}
