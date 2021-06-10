// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.App.Other.Models;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Lib.Uwp
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
            this._authenticationProvider = authProvider;
            InitHttpClient();
        }

        /// <inheritdoc/>
        public TimeSpan OverallTimeout
        {
            get
            {
                return this._httpClient.Timeout;
            }

            set
            {
                try
                {
                    this._httpClient.Timeout = value;
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
        public async Task<HttpRequestMessage> GetRequestMessageAsync(
            HttpMethod method,
            string url,
            Dictionary<string, string> queryParams = null,
            RequestClientType clientType = RequestClientType.Android)
        {
            HttpRequestMessage requestMessage;
            if (method == HttpMethod.Get || method == HttpMethod.Delete)
            {
                var query = await _authenticationProvider.GenerateAuthorizedQueryStringAsync(queryParams, clientType);
                url += $"?{query}";
                requestMessage = new HttpRequestMessage(method, url);
            }
            else
            {
                var query = await _authenticationProvider.GenerateAuthorizedQueryDictionaryAsync(queryParams, clientType);
                requestMessage = new HttpRequestMessage(method, url);
                requestMessage.Content = new FormUrlEncodedContent(query);
            }

            return requestMessage;
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return this.SendAsync(request, CancellationToken.None);
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await this.SendRequestAsync(request, cancellationToken);
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
    }
}
