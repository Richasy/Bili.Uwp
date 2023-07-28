// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Bili.Models.BiliBili;
using Bili.Models.gRPC;
using Newtonsoft.Json;

namespace Bili.Lib
{
    /// <summary>
    /// 用于网络请求模块的内部方法.
    /// </summary>
    public partial class HttpProvider
    {
        private static string _buvid;
        private readonly IAuthorizeProvider _authenticationProvider;
        private bool _disposedValue;
        private CookieContainer _cookieContainer;

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
                await Task.Run(async () =>
                {
                    response = await HttpClient.SendAsync(request, cancellationToken);
                });
                await ThrowIfHasExceptionAsync(response);
            }
            catch (TaskCanceledException exception)
            {
                throw new ServiceException(
                        new ServerResponse
                        {
                            Code = 408,
                            Message = ServiceConstants.Messages.RequestTimedOut,
                            IsHttpError = true,
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
                            IsHttpError = true,
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
            if (!_disposedValue)
            {
                if (disposing)
                {
                    HttpClient?.Dispose();
                }

                HttpClient = null;
                _disposedValue = true;
            }
        }

        private static string GetBuvid()
        {
            if (string.IsNullOrEmpty(_buvid))
            {
                var macAddress = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault();
                var buvidObj = new Buvid(macAddress);
                _buvid = buvidObj.Generate();
            }

            return _buvid;
        }

        private void InitHttpClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.None,
                UseCookies = true,
                CookieContainer = _cookieContainer,
            };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = false, NoStore = false };
            client.DefaultRequestHeaders.Add("accept", ServiceConstants.DefaultAcceptString);
            this.HttpClient = client;
        }

        private async Task ThrowIfHasExceptionAsync(HttpResponseMessage response)
        {
            ServerResponse errorResponse = null;
            try
            {
                if (response.Content.Headers.ContentType?.MediaType.Contains("image") ?? false)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }
                }
                else if (response.Content.Headers.ContentType?.MediaType == ServiceConstants.Headers.GRPCContentType)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    if (bytes.Length < 5)
                    {
                        errorResponse = new ServerResponse { Message = ServiceConstants.Messages.NoData };
                        throw new ServiceException(errorResponse, response.Headers, response.StatusCode);
                    }

                    return;
                }
                else if (response.Content.Headers.ContentType?.MediaType != ServiceConstants.Headers.JsonContentType)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }
                }

                var errorResponseStr = await response.Content.ReadAsStringAsync();
                errorResponse = JsonConvert.DeserializeObject<ServerResponse>(errorResponseStr);
                if (errorResponse?.Code == 0 || string.IsNullOrEmpty(errorResponseStr))
                {
                    return;
                }
            }
            catch (Exception)
            {
            }

            if (errorResponse == null || !response.IsSuccessStatusCode)
            {
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                {
                    errorResponse = new ServerResponse
                    {
                        Message = ServiceConstants.Messages.NotFound,
                        IsHttpError = true,
                    };
                }
                else
                {
                    errorResponse = new ServerResponse
                    {
                        Message = ServiceConstants.Messages.UnexpectedExceptionResponse,
                        IsHttpError = true,
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
