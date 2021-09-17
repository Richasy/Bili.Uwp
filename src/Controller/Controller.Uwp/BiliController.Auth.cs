﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 授权验证部分.
    /// </summary>
    public partial class BiliController
    {
        private bool _isRequestLogout = false;

        /// <summary>
        /// 尝试启动用户登录流程.
        /// </summary>
        /// <param name="isSlientOnly">是否只静默登录.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task TrySignInAsync(bool isSlientOnly = false)
        {
            try
            {
                var isTokenValid = await _authorizeProvider.IsTokenValidAsync(true);
                if (isTokenValid)
                {
                    Logged?.Invoke(this, EventArgs.Empty);
                }
                else if (IsNetworkAvailable && !isSlientOnly)
                {
                    await _authorizeProvider.SignInAsync();
                }
                else
                {
                    LoggedFailed?.Invoke(this, new Exception());
                }
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                LoggedFailed?.Invoke(this, ex);
            }
        }

        /// <summary>
        /// 用户登出.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SignOutAsync()
        {
            this._isRequestLogout = true;
            await _authorizeProvider.SignOutAsync();
        }

        private void OnAuthenticationStateChanged(object sender, AuthorizeStateChangedEventArgs e)
        {
            if (e.NewState == AuthorizeState.SignedOut && !_isRequestLogout)
            {
                LoggedFailed?.Invoke(this, new Exception("请求失败"));
            }

            switch (e.NewState)
            {
                case AuthorizeState.SignedIn:
                    Logged?.Invoke(this, EventArgs.Empty);
                    break;
                case AuthorizeState.SignedOut:
                    LoggedOut?.Invoke(this, EventArgs.Empty);
                    this._isRequestLogout = false;
                    break;
                default:
                    break;
            }
        }
    }
}
