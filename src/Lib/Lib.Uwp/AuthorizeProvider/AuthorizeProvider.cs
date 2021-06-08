// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<string> GenerateAuthorizedQueryStringAsync(Dictionary<string, object> queryParameters)
        {
            var query = string.Empty;
            if (!string.IsNullOrEmpty(_accessToken))
            {
                queryParameters.Add(ServiceConstants.Query.AccessToken, _accessToken);
            }
            else
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
            var signQuery = query + $"&{apiSecret}";
            var sign = _md5Toolkit.GetMd5String(signQuery).ToLower();
            return query + $"&sign={sign}";
        }

        /// <inheritdoc/>
        public async Task<string> GetTokenAsync(bool silentOnly = false)
        {
            // TODO: 检查当前Token的时效.
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectionProfile == null)
            {
                // 目前不在线.
                return null;
            }

            try
            {
                var result = await ShowAccountManagementPaneAndGetResultAsync();

                _accessToken = result;
                return result;
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
            if (!string.IsNullOrEmpty(_accessToken) || State != AuthorizeState.SignedOut)
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

            if (!string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = null;
            }

            State = AuthorizeState.SignedOut;
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<bool> TrySilentSignInAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && State == AuthorizeState.SignedIn)
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
    }
}
