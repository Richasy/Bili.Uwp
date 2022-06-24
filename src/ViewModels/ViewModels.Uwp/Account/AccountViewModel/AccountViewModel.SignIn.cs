// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.Enums;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 账户视图模型.
    /// </summary>
    public sealed partial class AccountViewModel
    {
        /// <summary>
        /// 尝试启动用户登录流程.
        /// </summary>
        /// <param name="isSlientOnly">是否只静默登录.</param>
        /// <returns><see cref="Task"/>.</returns>
        private async Task<bool> InternalSignInAsync(bool isSlientOnly = false)
        {
            var isSuccess = false;
            try
            {
                var isTokenValid = await _authorizeProvider.IsTokenValidAsync(true);
                if (isTokenValid)
                {
                    isSuccess = true;
                    await HandleLoggedAsync();
                }
                else if (_appViewModel.IsNetworkAvaliable && !isSlientOnly)
                {
                    isSuccess = await _authorizeProvider.TrySignInAsync();
                }
                else
                {
                    await HandleLoggingFailedAsync(new OperationCanceledException());
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                await HandleLoggingFailedAsync(ex);
            }

            return isSuccess;
        }

        private async Task HandleLoggedAsync()
        {
            if (State != AuthorizeState.SignedIn)
            {
                IsConnected = true;
                await GetMyProfileAsync();
                await InitializeFixedItemAsync();
                State = AuthorizeState.SignedIn;
            }
        }

        private async Task HandleLoggingFailedAsync(Exception exception)
        {
            // 它仅在用户未登录时触发.
            if (State != AuthorizeState.SignedIn)
            {
                Reset();
                State = AuthorizeState.SignedOut;

                if (exception is ServiceException serviceEx
                    && (!serviceEx.IsHttpError()))
                {
                    await SignOutAsync();
                }
            }
        }

        private async void OnAuthorizeStateChangedAsync(object sender, AuthorizeStateChangedEventArgs e)
        {
            State = e.NewState;
            switch (e.NewState)
            {
                case AuthorizeState.SignedIn:
                    await HandleLoggedAsync();
                    break;
                case AuthorizeState.SignedOut:
                    if (!_isRequestLogout)
                    {
                        await HandleLoggingFailedAsync(new OperationCanceledException("请求失败"));
                    }
                    else
                    {
                        Reset();
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
