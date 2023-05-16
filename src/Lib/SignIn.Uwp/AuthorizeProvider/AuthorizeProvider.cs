// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Windows.Web.Http.Filters;

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
            _isXbox = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
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
            bool needToken = false,
            bool forcrNoToken = false)
        {
            if (queryParameters == null)
            {
                queryParameters = new Dictionary<string, string>();
            }

            queryParameters.Add(ServiceConstants.Query.Build, ServiceConstants.BuildNumber);
            GenerateAppKey(queryParameters, clientType);

            var token = string.Empty;
            if (await IsTokenValidAsync() && !forcrNoToken)
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

            var sign = GenerateSign(queryParameters, clientType);
            queryParameters.Add(ServiceConstants.Query.Sign, sign);
            return queryParameters;
        }

        /// <inheritdoc/>
        public string GenerateAuthorizedQueryStringFirstSign(
            Dictionary<string, string> queryParameters,
            RequestClientType clientType)
        {
            if (queryParameters == null)
            {
                queryParameters = new Dictionary<string, string>();
            }

            // 先加盐再加appKey
            var sign = GenerateSign(queryParameters, clientType);
            queryParameters.Add(ServiceConstants.Query.Sign, sign);
            GenerateAppKey(queryParameters, clientType, true);
            var queryList = queryParameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();
            var query = string.Join('&', queryList);
            return query;
        }

        /// <inheritdoc/>
        public async Task<string> GenerateAuthorizedQueryStringAsync(Dictionary<string, string> queryParameters, RequestClientType clientType, bool needToken = true, bool forcrNoToken = false)
        {
            var parameters = await GenerateAuthorizedQueryDictionaryAsync(queryParameters, clientType, needToken, forcrNoToken);
            var queryList = parameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();
            var query = string.Join('&', queryList);
            return query;
        }

        /// <inheritdoc/>
        public string GetCookieString()
        {
            var fiter = new HttpBaseProtocolFilter();
            var cookies = fiter.CookieManager.GetCookies(new Uri(ApiConstants.CookieGetDomain));
            var cookieList = cookies.Select(x =>
            {
                return $"{x.Name}={x.Value}";
            });
            var result = string.Join(';', cookieList);
            return result;
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
            CurrentUserId = default;
            Locator.Instance.GetService<IAccountProvider>().UserId = 0;
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
