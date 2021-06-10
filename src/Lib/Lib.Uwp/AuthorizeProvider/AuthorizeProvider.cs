// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.Enums;
using Windows.Networking.Connectivity;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 授权模块.
    /// </summary>
    public partial class AuthorizeProvider : IAuthorizeProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeProvider"/> class.
        /// </summary>
        public AuthorizeProvider()
        {
            ServiceLocator.Instance.LoadService(out _md5Toolkit)
                                   .LoadService(out _settingsToolkit);

            State = AuthorizeState.SignedOut;
            RetrieveAuthorizeResult();
        }

        /// <inheritdoc/>
        public event EventHandler<AuthorizeStateChangedEventArgs> StateChanged;

        /// <inheritdoc/>
        public AuthorizeState State
        {
            get => _state;
            protected set
            {
                var oldState = _state;
                var newState = value;
                if (oldState != newState)
                {
                    _state = newState;
                    StateChanged?.Invoke(this, new AuthorizeStateChangedEventArgs(oldState, newState));
                }
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> GenerateAuthorizedQueryDictionaryAsync(Dictionary<string, string> queryParameters, RequestClientType clientType)
        {
            if (queryParameters == null)
            {
                queryParameters = new Dictionary<string, string>();
            }

            queryParameters.Add(ServiceConstants.Query.Build, ServiceConstants.BuildNumber);
            if (clientType == RequestClientType.IOS)
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.IOSKey);
                queryParameters.Add(ServiceConstants.Query.MobileApp, "iphone");
                queryParameters.Add(ServiceConstants.Query.Platform, "ios");
                queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowSeconds().ToString());
            }
            else if (clientType == RequestClientType.Web)
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.WebKey);
                queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowMilliSeconds().ToString());
            }
            else
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.AndroidKey);
                queryParameters.Add(ServiceConstants.Query.MobileApp, "android");
                queryParameters.Add(ServiceConstants.Query.Platform, "android");
                queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowSeconds().ToString());
            }

            var query = string.Empty;
            if (IsTokenValid())
            {
                queryParameters.Add(ServiceConstants.Query.AccessToken, _tokenInfo.AccessToken);
            }
            else if (_tokenInfo != null)
            {
                await GetTokenAsync();
            }

            var queryList = queryParameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();

            var apiKey = queryParameters[ServiceConstants.Query.AppKey].ToString();
            var apiSecret = string.Empty;
            if (apiKey == ServiceConstants.Keys.IOSKey)
            {
                apiSecret = ServiceConstants.Keys.IOSSecret;
            }
            else if (apiKey == ServiceConstants.Keys.AndroidKey)
            {
                apiSecret = ServiceConstants.Keys.AndroidSecret;
            }
            else
            {
                apiSecret = ServiceConstants.Keys.WebSecret;
            }

            query = string.Join('&', queryList);
            var signQuery = query + apiSecret;
            var sign = _md5Toolkit.GetMd5String(signQuery).ToLower();

            queryParameters.Add("sign", sign);
            return queryParameters;
        }

        /// <inheritdoc/>
        public async Task<string> GenerateAuthorizedQueryStringAsync(Dictionary<string, string> queryParameters, RequestClientType clientType)
        {
            var parameters = await GenerateAuthorizedQueryDictionaryAsync(queryParameters, clientType);
            var queryList = parameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();
            var query = string.Join('&', queryList);
            return query;
        }

        /// <inheritdoc/>
        public async Task<string> GetTokenAsync(bool silentOnly = false)
        {
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectionProfile == null)
            {
                // 目前不在线.
                return null;
            }

            try
            {
                if (_tokenInfo != null)
                {
                    if (IsTokenValid())
                    {
                        return _tokenInfo.AccessToken;
                    }
                    else
                    {
                        // TODO: Refresh token.
                    }
                }
                else
                {
                    var result = await ShowAccountManagementPaneAndGetResultAsync();
                    SaveAuthorizeResult(result);
                    return result.TokenInfo.AccessToken;
                }
            }
            catch (Exception)
            {
            }

            await SignOutAsync();
            return null;
        }

        /// <inheritdoc/>
        public async Task SignInAsync()
        {
            if (IsTokenValid() || State != AuthorizeState.SignedOut)
            {
                return;
            }

            State = AuthorizeState.Loading;

            var token = await GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                await SignOutAsync();
            }
        }

        /// <inheritdoc/>
        public Task SignOutAsync()
        {
            State = AuthorizeState.Loading;

            _settingsToolkit.DeleteLocalSetting(SettingNames.BiliUserId);
            _settingsToolkit.DeleteLocalSetting(SettingNames.AuthorizeResult);

            if (_tokenInfo != null)
            {
                _tokenInfo = null;
            }

            State = AuthorizeState.SignedOut;
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<bool> TrySilentSignInAsync()
        {
            if (IsTokenValid() && State == AuthorizeState.SignedIn)
            {
                return true;
            }

            State = AuthorizeState.Loading;

            var token = await GetTokenAsync(true);

            if (token == null)
            {
                State = AuthorizeState.SignedOut;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取验证码图片流.
        /// </summary>
        /// <returns>图片数据流.</returns>
        public async Task<Stream> GetCaptchaImageAsync()
        {
            var uri = ServiceConstants.Api.Passport.Captcha + $"?ts={GetNowSeconds()}";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
            var response = await httpProvider.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
