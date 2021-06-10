// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other.Models;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 用户登录对话框.
    /// </summary>
    public sealed partial class AccountLoginDialog : ContentDialog
    {
        private readonly TaskCompletionSource<AuthorizeResult> _taskCompletionSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountLoginDialog"/> class.
        /// </summary>
        internal AccountLoginDialog(TaskCompletionSource<AuthorizeResult> taskCompletionSource)
        {
            this.InitializeComponent();
            _taskCompletionSource = taskCompletionSource;
        }

        private async void OnPrimaryButtonClickAsync(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            var userName = UserNameBox.Text;
            var password = PasswordBox.Password;
            var captcha = CaptchaBox.Text;
            HideError();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ShowError(LanguageNames.ValidUserNameOrPasswordTip);
            }
            else if (CaptchaContainer.Visibility == Visibility.Visible && string.IsNullOrEmpty(captcha))
            {
                ShowError(LanguageNames.CaptchaIsEmpty);
            }
            else
            {
                try
                {
                    IsPrimaryButtonEnabled = false;
                    var authProvider = ServiceLocator.Instance.GetService<IAuthorizeProvider>();
                    var result = await (authProvider as AuthorizeProvider)?.InternalLoginAsync(userName, password, captcha);
                    _taskCompletionSource.SetResult(result);
                }
                catch (ServiceException se)
                {
                    var isHandled = false;
                    switch (se.Error.Code)
                    {
                        case -105:
                            // 需要验证码.
                            await ShowCaptchaAsync();
                            isHandled = true;
                            break;
                        case -629:
                            // 账号或密码错误.
                            ShowError(LanguageNames.InvalidUserNameOrPassword);
                            isHandled = true;
                            break;
                        default:
                            break;
                    }

                    if (!isHandled)
                    {
                        _taskCompletionSource.SetException(se);
                        this.Hide();
                    }
                    else
                    {
                        IsPrimaryButtonEnabled = true;
                    }
                }
            }
        }

        private async Task ShowCaptchaAsync()
        {
            CaptchaContainer.Visibility = Visibility.Visible;
            ShowError(LanguageNames.InputCaptchaTip);
            var authProvider = ServiceLocator.Instance.GetService<IAuthorizeProvider>() as AuthorizeProvider;
            var imageStream = await authProvider?.GetCaptchaImageAsync();
            var bitmap = new BitmapImage();
            CaptchaImage.Source = bitmap;
            await bitmap.SetSourceAsync(imageStream.AsRandomAccessStream());
        }

        private void ShowError(LanguageNames name)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            ErrorBlock.Visibility = Visibility.Visible;
            ErrorBlock.Text = resourceToolkit.GetLocaleString(name);
        }

        private void HideError()
        {
            ErrorBlock.Visibility = Visibility.Collapsed;
        }
    }
}
