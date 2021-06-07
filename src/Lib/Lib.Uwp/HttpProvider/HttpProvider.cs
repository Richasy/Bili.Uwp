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
        public HttpRequestMessage GetRequestMessage(
            HttpMethod method,
            string url,
            Dictionary<string, object> queryParams = null,
            RequestClientType clientType = RequestClientType.Android)
        {
            if (queryParams == null)
            {
                queryParams = new Dictionary<string, object>();
            }

            queryParams.Add(ServiceConstants.Query.Build, ServiceConstants.BuildNumber);
            if (clientType == RequestClientType.IOS)
            {
                queryParams.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.IOSKey);
                queryParams.Add(ServiceConstants.Query.MobileApp, "iphone");
                queryParams.Add(ServiceConstants.Query.Platform, "ios");
                queryParams.Add(ServiceConstants.Query.TimeStamp, GetNowSeconds());
            }
            else if (clientType == RequestClientType.Web)
            {
                queryParams.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.WebKey);
                queryParams.Add(ServiceConstants.Query.TimeStamp, GetNowMilliSeconds());
            }
            else
            {
                queryParams.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.AndroidKey);
                queryParams.Add(ServiceConstants.Query.MobileApp, "android");
                queryParams.Add(ServiceConstants.Query.Platform, "android");
                queryParams.Add(ServiceConstants.Query.TimeStamp, GetNowSeconds());
            }

            var query = _authenticationProvider.GenerateAuthorizedQueryStringAsync(queryParams);
            url += $"?{query}";

            return new HttpRequestMessage(method, url);
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
