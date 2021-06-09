// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other.Models;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 用户登录对话框.
    /// </summary>
    public sealed partial class AccountLoginDialog : ContentDialog
    {
        readonly TaskCompletionSource<AuthorizeResult> _taskCompletionSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountLoginDialog"/> class.
        /// </summary>
        internal AccountLoginDialog(TaskCompletionSource<AuthorizeResult> taskCompletionSource)
        {
            this.InitializeComponent();
            _taskCompletionSource = taskCompletionSource;
        }

        private async void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            var userName = UserNameBox.Text;
            var password = PasswordBox.Password;
            var captcha = CaptchaBox.Text;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            HideError();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ShowError(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.ValidUserNameOrPasswordTip));
            }
            else if (CaptchaContainer.Visibility == Visibility.Visible && string.IsNullOrEmpty(captcha))
            {
                ShowError(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.CaptchaIsEmpty));
            }
            else
            {
                try
                {
                    var authProvider = ServiceLocator.Instance.GetService<IAuthorizeProvider>();
                    var result = (authProvider as AuthorizeProvider)?.InternalLoginAsync(userName, password, captcha);
                }
                catch (ServiceException se)
                {
                    switch (se.Error.Code)
                    {
                        case -2100:
                            break;
                        case -105:
                            break;
                        case -449:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ShowError(string msg)
        {
            ErrorBlock.Visibility = Visibility.Visible;
            ErrorBlock.Text = msg;
        }

        private void HideError()
        {
            ErrorBlock.Visibility = Visibility.Collapsed;
        }
    }
}
