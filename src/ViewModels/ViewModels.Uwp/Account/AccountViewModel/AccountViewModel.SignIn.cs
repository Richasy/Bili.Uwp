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
            State = AuthorizeState.Loading;
            try
            {
                var isTokenValid = await _authorizeProvider.IsTokenValidAsync(true);
                if (isTokenValid)
                {
                    isSuccess = true;
                    State = AuthorizeState.SignedIn;
                    HandleLogged();
                }
                else if (!isSlientOnly)
                {
                    isSuccess = await _authorizeProvider.TrySignInAsync();
                }
                else
                {
                    HandleLoggingFailed(new OperationCanceledException());
                }
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(ex);
            }

            return isSuccess;
        }

        private void HandleLogged()
        {
            if (State == AuthorizeState.SignedIn)
            {
                LoadMyProfileCommand.ExecuteAsync(null).ContinueWith(async t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            IsConnected = true;
                            await InitializeFixedItemAsync();
                            State = AuthorizeState.SignedIn;
                        });
                    }
                    else
                    {
                        await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            await SignOutAsync();
                        });
                    }
                });
            }
        }

        private void HandleLoggingFailed(Exception exception)
        {
            // 它仅在用户未登录时触发.
            if (State != AuthorizeState.SignedIn)
            {
                Reset();
                State = AuthorizeState.SignedOut;

                if (exception is ServiceException serviceEx
                    && (!serviceEx.IsHttpError()))
                {
                    SignOutCommand.ExecuteAsync(null);
                }
            }
        }

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
        {
            State = e.NewState;
            switch (e.NewState)
            {
                case AuthorizeState.SignedIn:
                    HandleLogged();
                    break;
                case AuthorizeState.SignedOut:
                    if (!_isRequestLogout)
                    {
                        HandleLoggingFailed(new OperationCanceledException("请求失败"));
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
