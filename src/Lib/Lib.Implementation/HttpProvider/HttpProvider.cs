// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bili.Models.gRPC;
using Google.Protobuf;
using Newtonsoft.Json;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
{
    /// <summary>
    /// 用于网络请求的模块.
    /// </summary>
    public partial class HttpProvider : IHttpProvider, IDisposable
    {
        /// <summary>
        /// 创建 <see cref="HttpProvider"/> 的实例.
        /// </summary>
        /// <param name="authProvider">授权验证模块.</param>
        public HttpProvider(IAuthorizeProvider authProvider)
        {
            _authenticationProvider = authProvider;
            InitHttpClient();
        }

        /// <inheritdoc/>
        public TimeSpan OverallTimeout
        {
            get
            {
                return HttpClient.Timeout;
            }

            set
            {
                try
                {
                    HttpClient.Timeout = value;
                }
                catch (InvalidOperationException exception)
                {
                    throw new ServiceException(
                        new Models.BiliBili.ServerResponse
                        {
                            Message = ServiceConstants.Messages.OverallTimeoutCannotBeSet,
                        },
                        exception);
                }
            }
        }

        /// <inheritdoc/>
        public HttpClient HttpClient { get; private set; }

        /// <inheritdoc/>
        public async Task<HttpRequestMessage> GetRequestMessageAsync(
            HttpMethod method,
            string url,
            Dictionary<string, string> queryParams = null,
            RequestClientType clientType = RequestClientType.Android,
            bool needToken = true,
            string additionalQuery = "",
            bool needCookie = false,
            bool needAppKey = false,
            bool forceNoToken = false)
        {
            HttpRequestMessage requestMessage;

            if (method == HttpMethod.Get && needCookie)
            {
                if (needAppKey)
                {
                    var query = _authenticationProvider.GenerateAuthorizedQueryStringFirstSign(queryParams, clientType);
                    url += $"?{query}";
                }

                if (!string.IsNullOrEmpty(additionalQuery))
                {
                    url += $"&{additionalQuery}";
                }

                requestMessage = new HttpRequestMessage(method, url);
                var cookie = _authenticationProvider.GetCookieString();
                requestMessage.Headers.Add("Cookie", cookie);
            }
            else if (method == HttpMethod.Get || method == HttpMethod.Delete)
            {
                var query = await _authenticationProvider.GenerateAuthorizedQueryStringAsync(queryParams, clientType, needToken, forceNoToken);
                if (!string.IsNullOrEmpty(additionalQuery))
                {
                    query += $"&{additionalQuery}";
                }

                url += $"?{query}";
                requestMessage = new HttpRequestMessage(method, url);
            }
            else
            {
                var query = await _authenticationProvider.GenerateAuthorizedQueryDictionaryAsync(queryParams, clientType, needToken);
                requestMessage = new HttpRequestMessage(method, url);
                requestMessage.Content = new FormUrlEncodedContent(query);
            }

            return requestMessage;
        }

        /// <inheritdoc/>
        public async Task<HttpRequestMessage> GetRequestMessageAsync(string url, IMessage grpcMessage, bool needToken = false)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var isTokenValid = await _authenticationProvider.IsTokenValidAsync();
            var token = string.Empty;
            if (needToken || isTokenValid)
            {
                token = await _authenticationProvider.GetTokenAsync();
            }

            var grpcConfig = new GRPCConfig(token);
            var userAgent = $"bili-universal/62800300 "
                + $"os/ios model/{GRPCConfig.Model} mobi_app/iphone "
                + $"osVer/{GRPCConfig.OSVersion} "
                + $"network/{GRPCConfig.NetworkType} "
                + $"grpc-objc/1.32.0 grpc-c/12.0.0 (ios; cronet_http)";

            if (!string.IsNullOrEmpty(token))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(Headers.Identify, token);
            }

            requestMessage.Headers.Add(Headers.UserAgent, userAgent);
            requestMessage.Headers.Add(Headers.AppKey, GRPCConfig.MobileApp);
            requestMessage.Headers.Add(Headers.BiliDevice, grpcConfig.GetDeviceBin());
            requestMessage.Headers.Add(Headers.BiliFawkes, grpcConfig.GetFawkesreqBin());
            requestMessage.Headers.Add(Headers.BiliLocale, grpcConfig.GetLocaleBin());
            requestMessage.Headers.Add(Headers.BiliMeta, grpcConfig.GetMetadataBin());
            requestMessage.Headers.Add(Headers.BiliNetwork, grpcConfig.GetNetworkBin());
            requestMessage.Headers.Add(Headers.BiliRestriction, grpcConfig.GetRestrictionBin());
            requestMessage.Headers.Add(Headers.GRPCAcceptEncodingKey, Headers.GRPCAcceptEncodingValue);
            requestMessage.Headers.Add(Headers.GRPCTimeOutKey, Headers.GRPCTimeOutValue);
            requestMessage.Headers.Add(Headers.Envoriment, GRPCConfig.Envorienment);
            requestMessage.Headers.Add(Headers.TransferEncodingKey, Headers.TransferEncodingValue);
            requestMessage.Headers.Add(Headers.TEKey, Headers.TEValue);
            requestMessage.Headers.Add(Headers.Buvid, GetBuvid());

            var messageBytes = grpcMessage.ToByteArray();

            // 校验用?第五位为数组长度
            var stateBytes = new byte[] { 0, 0, 0, 0, (byte)messageBytes.Length };

            // 合并两个字节数组
            var bodyBytes = new byte[5 + messageBytes.Length];
            stateBytes.CopyTo(bodyBytes, 0);
            messageBytes.CopyTo(bodyBytes, 5);

            var byteArrayContent = new ByteArrayContent(bodyBytes);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(Headers.GRPCContentType);
            byteArrayContent.Headers.ContentLength = bodyBytes.Length;

            requestMessage.Content = byteArrayContent;
            return requestMessage;
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
            => SendAsync(request, CancellationToken.None);

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await SendRequestAsync(request, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }

        /// <inheritdoc/>
        public async Task<T> ParseAsync<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        /// <inheritdoc/>
        public async Task<T> ParseAsync<T>(HttpResponseMessage response, MessageParser<T> parser)
            where T : IMessage<T>
        {
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return parser.ParseFrom(bytes.Skip(5).ToArray());
        }

        /// <inheritdoc/>
        public async Task<object> ParseAsync<T1, T2>(HttpResponseMessage response, Func<string, bool> condition)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            if (condition(responseString))
            {
                return JsonConvert.DeserializeObject<T1>(responseString);
            }
            else
            {
                return JsonConvert.DeserializeObject<T2>(responseString);
            }
        }
    }
}
