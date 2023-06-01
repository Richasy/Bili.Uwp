// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bili.Models.Enums;
using Google.Protobuf;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 用于进行网络请求.
    /// </summary>
    public interface IHttpProvider
    {
        /// <summary>
        /// 内部的超时时长设置，默认为100秒.
        /// </summary>
        TimeSpan OverallTimeout { get; set; }

        /// <summary>
        /// 网络客户端.
        /// </summary>
        HttpClient HttpClient { get; }

        /// <summary>
        /// 获取 <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="method">请求方法.</param>
        /// <param name="url">请求地址.</param>
        /// <param name="queryParams">查询参数.</param>
        /// <param name="type">需要模拟的设备类型.</param>
        /// <param name="needToken">是否需要令牌.</param>
        /// <param name="additionalQuery">附加查询参数.</param>
        /// <param name="needCookie">是否需要cookie.</param>
        /// <param name="needAppKey">是否需要appKey.</param>
        /// <param name="forceNoToken">是否强制不AccessKey.</param>
        /// <returns><see cref="HttpRequestMessage"/>.</returns>
        Task<HttpRequestMessage> GetRequestMessageAsync(HttpMethod method, string url, Dictionary<string, string> queryParams = null, RequestClientType type = RequestClientType.Android, bool needToken = false, string additionalQuery = "", bool needCookie = false, bool needAppKey = false, bool forceNoToken = false);

        /// <summary>
        /// 获取 <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="url">请求地址.</param>
        /// <param name="grpcMessage">gRPC信息.</param>
        /// <param name="needToken">是否需要令牌.</param>
        /// <returns><see cref="HttpRequestMessage"/>.</returns>
        Task<HttpRequestMessage> GetRequestMessageAsync(string url, IMessage grpcMessage, bool needToken = false);

        /// <summary>
        /// 发送请求.
        /// </summary>
        /// <param name="request">需要发送的 <see cref="HttpRequestMessage"/>.</param>
        /// <returns>返回的 <see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        /// <summary>
        /// 发送请求.
        /// </summary>
        /// <param name="request">需要发送的 <see cref="HttpRequestMessage"/>.</param>
        /// <param name="cancellationToken">请求的 <see cref="CancellationToken"/>.</param>
        /// <returns>返回的 <see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        /// <summary>
        /// 解析响应.
        /// </summary>
        /// <param name="response">得到的 <see cref="HttpResponseMessage"/>.</param>
        /// <typeparam name="T">需要转换的目标类型.</typeparam>
        /// <returns>转换结果.</returns>
        Task<T> ParseAsync<T>(HttpResponseMessage response);

        /// <summary>
        /// 解析响应，并选择满足条件的目标类型返回.
        /// </summary>
        /// <param name="response">得到的 <see cref="HttpResponseMessage"/>.</param>
        /// <param name="condition">条件.</param>
        /// <typeparam name="T1">需要转换的目标类型1.</typeparam>
        /// <typeparam name="T2">需要转换的目标类型2.</typeparam>
        /// <returns>转换结果.</returns>
        Task<object> ParseAsync<T1, T2>(HttpResponseMessage response, Func<string, bool> condition);

        /// <summary>
        /// 解析响应.
        /// </summary>
        /// <param name="response">得到的 <see cref="HttpResponseMessage"/>.</param>
        /// <param name="parser">对应gRPC类型的转换器.</param>
        /// <typeparam name="T">需要转换的gRPC目标类型.</typeparam>
        /// <returns>转换结果.</returns>
        Task<T> ParseAsync<T>(HttpResponseMessage response, MessageParser<T> parser)
            where T : IMessage<T>;
    }
}
