// Copyright (c) Richasy. All rights reserved.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.App.Other.Models;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 用于网络请求模块的内部方法.
    /// </summary>
    public partial class HttpProvider
    {
        private readonly IAuthorizeProvider _authenticationProvider;
        private HttpClient _httpClient;
        private bool disposedValue;

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal async Task<HttpResponseMessage> SendRequestAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await this._httpClient.SendAsync(request, cancellationToken);
                await ThrowIfHasExceptionAsync(response);
            }
            catch (TaskCanceledException exception)
            {
                throw new ServiceException(
                        new ServerResponse
                        {
                            Message = ServiceConstants.Messages.RequestTimedOut,
                        },
                        exception);
            }
            catch (ServiceException exception)
            {
                throw exception;
            }
            catch (Exception exception)
            {
                throw new ServiceException(
                        new ServerResponse
                        {
                            Message = ServiceConstants.Messages.UnexpectedExceptionOnSend,
                        },
                        exception);
            }

            return response;
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        /// <param name="disposing">Is it disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this._httpClient != null)
                    {
                        this._httpClient.Dispose();
                    }
                }

                this._httpClient = null;
                disposedValue = true;
            }
        }

        private void InitHttpClient()
        {
            var handler = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.None };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true };
            client.DefaultRequestHeaders.Add("accept", ServiceConstants.DefaultAcceptString);
            this._httpClient = client;
        }

        private async Task ThrowIfHasExceptionAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            ServerResponse errorResponse = null;
            try
            {
                var errorResponseStr = await response.Content.ReadAsStringAsync();
                errorResponse = JsonConvert.DeserializeObject<ServerResponse>(errorResponseStr);
            }
            catch (Exception)
            {
            }

            if (errorResponse == null)
            {
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                {
                    errorResponse = new ServerResponse { Message = ServiceConstants.Messages.NotFound };
                }
                else
                {
                    errorResponse = new ServerResponse
                    {
                        Message = ServiceConstants.Messages.UnexpectedExceptionResponse,
                    };
                }
            }

            if (response.Content?.Headers.ContentType?.MediaType == ServiceConstants.Headers.JsonContentType)
            {
                var rawResponseBody = await response.Content.ReadAsStringAsync();

                throw new ServiceException(
                    errorResponse,
                    response.Headers,
                    response.StatusCode,
                    rawResponseBody,
                    null);
            }
            else
            {
                // 将响应头和状态代码传递给ServiceException。
                // System.Net.HttpStatusCode不支持RFC 6585，附加HTTP状态代码。
                // 节流状态代码429是在RFC 6586中。状态码429将被传递过去。
                throw new ServiceException(errorResponse, response.Headers, response.StatusCode);
            }
        }
    }
}
