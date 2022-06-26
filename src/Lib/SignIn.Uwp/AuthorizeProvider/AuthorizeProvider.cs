// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;

namespace Bili.SignIn.Uwp
{
    /// <summary>
    /// 授权模块.
    /// </summary>
    public partial class AuthorizeProvider : IAuthorizeProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeProvider"/> class.
        /// </summary>
        /// <param name="md5Toolkit">MD5工具箱.</param>
        /// <param name="settingsToolkit">设置工具箱.</param>
        public AuthorizeProvider(
            IMD5Toolkit md5Toolkit,
            ISettingsToolkit settingsToolkit)
        {
            _md5Toolkit = md5Toolkit;
            _settingsToolkit = settingsToolkit;
            State = AuthorizeState.SignedOut;
            RetrieveAuthorizeResult();
            _guid = Guid.NewGuid().ToString("N");
        }

        /// <inheritdoc/>
        public event EventHandler<AuthorizeStateChangedEventArgs> StateChanged;

        /// <summary>
        /// 在二维码状态发生改变时发生.
        /// </summary>
        public event EventHandler<Tuple<QRCodeStatus, TokenInfo>> QRCodeStatusChanged;

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
        public string CurrentUserId { get; private set; }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> GenerateAuthorizedQueryDictionaryAsync(
            Dictionary<string, string> queryParameters,
            RequestClientType clientType,
            bool needToken = false)
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
                queryParameters.Add(ServiceConstants.Query.Platform, "web");
                queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowMilliSeconds().ToString());
            }
            else if (clientType == RequestClientType.Login)
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.LoginKey);
                queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowMilliSeconds().ToString());
            }
            else
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.AndroidKey);
                queryParameters.Add(ServiceConstants.Query.MobileApp, "android");
                queryParameters.Add(ServiceConstants.Query.Platform, "android");
                queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowSeconds().ToString());
            }

            var token = string.Empty;
            if (await IsTokenValidAsync())
            {
                token = _tokenInfo.AccessToken;
            }
            else if (needToken)
            {
                token = await GetTokenAsync();
            }

            if (!string.IsNullOrEmpty(token))
            {
                queryParameters.Add(ServiceConstants.Query.AccessKey, token);
            }
            else if (needToken)
            {
                throw new OperationCanceledException("需要令牌，但获取访问令牌失败.");
            }

            var sign = GenerateSign(queryParameters);
            queryParameters.Add(ServiceConstants.Query.Sign, sign);
            return queryParameters;
        }

        /// <inheritdoc/>
        public async Task<string> GenerateAuthorizedQueryStringAsync(Dictionary<string, string> queryParameters, RequestClientType clientType, bool needToken = true)
        {
            var parameters = await GenerateAuthorizedQueryDictionaryAsync(queryParameters, clientType, needToken);
            var queryList = parameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();
            var query = string.Join('&', queryList);
            return query;
        }

        /// <inheritdoc/>
        public async Task<string> GetTokenAsync()
        {
            try
            {
                if (_tokenInfo != null)
                {
                    if (await IsTokenValidAsync())
                    {
                        State = AuthorizeState.SignedIn;
                        return _tokenInfo.AccessToken;
                    }
                    else
                    {
                        var tokenInfo = await InternalRefreshTokenAsync();
                        if (tokenInfo != null)
                        {
                            SaveAuthorizeResult(tokenInfo);
                            return tokenInfo.AccessToken;
                        }
                    }
                }
                else
                {
                    var result = await ShowAccountManagementPaneAndGetResultAsync();
                    SaveAuthorizeResult(result.TokenInfo);
                    return result.TokenInfo.AccessToken;
                }
            }
            catch (Exception)
            {
                StopQRLoginListener();
                await SignOutAsync();
                throw;
            }

            return default;
        }

        /// <inheritdoc/>
        public async Task<bool> TrySignInAsync()
        {
            if (await IsTokenValidAsync() || State != AuthorizeState.SignedOut)
            {
                return true;
            }

            State = AuthorizeState.Loading;

            var token = await GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                await SignOutAsync();
                return false;
            }

            return true;
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
        public async Task<bool> IsTokenValidAsync(bool isNetworkVerify = false)
        {
            var isLocalValid = _tokenInfo != null &&
                !string.IsNullOrEmpty(_tokenInfo.AccessToken) &&
                _lastAuthorizeTime != null &&
                (DateTimeOffset.Now - _lastAuthorizeTime).TotalSeconds < _tokenInfo.ExpiresIn;

            var result = isLocalValid && isNetworkVerify
                ? await NetworkVerifyTokenAsync()
                : isLocalValid;

            return result;
        }
    }
}
