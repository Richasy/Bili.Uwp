// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other.Models;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
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
        private readonly TaskCompletionSource<AuthorizeResult> _taskCompletionSource;
        private readonly AuthorizeProvider _authorizeProvider;
        private LoginType _loginType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountLoginDialog"/> class.
        /// </summary>
        internal AccountLoginDialog(TaskCompletionSource<AuthorizeResult> taskCompletionSource)
        {
            this.InitializeComponent();
            this.Opened += OnOpenedAsync;
            this.Closed += OnClosed;
            _authorizeProvider = ServiceLocator.Instance.GetService<IAuthorizeProvider>() as AuthorizeProvider;
            _taskCompletionSource = taskCompletionSource;
        }

        private void OnClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            _authorizeProvider.QRCodeStatusChanged -= OnQRCodeStatusChanged;
        }

        private async void OnOpenedAsync(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            _authorizeProvider.QRCodeStatusChanged += OnQRCodeStatusChanged;
            await SwitchLoginTypeAsync(LoginType.QRCode);
        }

        private void OnQRCodeStatusChanged(object sender, System.Tuple<QRCodeStatus, TokenInfo> e)
        {
            switch (e.Item1)
            {
                case QRCodeStatus.Expiried:
                    ShowQRTip(LanguageNames.QRCodeExpired);
                    _authorizeProvider.StopQRLoginListener();
                    break;
                case QRCodeStatus.Success:
                    _authorizeProvider.StopQRLoginListener();
                    _taskCompletionSource.SetResult(new AuthorizeResult { TokenInfo = e.Item2 });
                    this.Hide();
                    break;
                case QRCodeStatus.Failed:
                    ShowQRTip(LanguageNames.LoginFailed);
                    _authorizeProvider.StopQRLoginListener();
                    break;
                default:
                    break;
            }
        }

        private async void OnPrimaryButtonClickAsync(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            if (_loginType == LoginType.Password)
            {
                await HandlePasswordLoginAsync();
            }
        }

        private void OnCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _taskCompletionSource.SetCanceled();
        }

        private async Task HandlePasswordLoginAsync()
        {
            var userName = UserNameBox.Text;
            var password = PasswordBox.Password;
            HideError();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ShowError(LanguageNames.ValidUserNameOrPasswordTip);
            }
            else
            {
                try
                {
                    IsPrimaryButtonEnabled = false;
                    var result = await _authorizeProvider?.InternalLoginAsync(userName, password);
                    _taskCompletionSource.SetResult(result);
                    this.Hide();
                }
                catch (ServiceException se)
                {
                    switch (se.Error.Code)
                    {
                        case -105:
                            // 需要验证码，转入扫码登录步骤.
                            ShowError(LanguageNames.NeedQRLogin);
                            await SwitchLoginTypeAsync(LoginType.QRCode);
                            break;
                        case -629:
                            // 账号或密码错误.
                            ShowError(LanguageNames.InvalidUserNameOrPassword);
                            break;
                        default:
                            ShowError(se.Error?.Message ?? se.ToString());
                            break;
                    }

                    IsPrimaryButtonEnabled = true;
                }
            }
        }

        private async Task LoadQRCodeAsync(bool isShowTip = false)
        {
            HideQRTip();
            QRLoadingRing.IsActive = true;
            var imgSource = await _authorizeProvider.GetQRImageAsync();
            if (imgSource != null)
            {
                QRCodeImage.Source = imgSource;
                _authorizeProvider.StartQRLoginListener();
            }
            else
            {
                ShowQRTip(LanguageNames.FailedToLoadQRCode);
            }

            QRLoadingRing.IsActive = false;
        }

        private async Task SwitchLoginTypeAsync(LoginType type)
        {
            _loginType = type;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            switch (type)
            {
                case LoginType.Password:
                    PasswordLoginContainer.Visibility = Visibility.Visible;
                    QRLoginContainer.Visibility = Visibility.Collapsed;
                    PrimaryButtonText = resourceToolkit.GetLocaleString(LanguageNames.SignIn);
                    TipBlock.Text = resourceToolkit.GetLocaleString(LanguageNames.PasswordLoginTip);
                    SwitchActionButton.Content = resourceToolkit.GetLocaleString(LanguageNames.SwitchToQRLogin);
                    (_authorizeProvider as AuthorizeProvider).StartQRLoginListener();
                    break;
                case LoginType.QRCode:
                    PasswordLoginContainer.Visibility = Visibility.Collapsed;
                    QRLoginContainer.Visibility = Visibility.Visible;
                    PrimaryButtonText = string.Empty;
                    TipBlock.Text = resourceToolkit.GetLocaleString(LanguageNames.QRLoginTip);
                    SwitchActionButton.Content = resourceToolkit.GetLocaleString(LanguageNames.SwitchToPasswordLogin);
                    await LoadQRCodeAsync();
                    break;
                default:
                    break;
            }
        }

        private void ShowError(string msg)
        {
            ErrorBlock.Visibility = Visibility.Visible;
            ErrorBlock.Text = msg;
        }

        private void ShowError(LanguageNames name)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            var msg = resourceToolkit.GetLocaleString(name);
            ShowError(msg);
        }

        private void HideError()
        {
            ErrorBlock.Visibility = Visibility.Collapsed;
        }

        private void ShowQRTip(LanguageNames name)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            var msg = resourceToolkit.GetLocaleString(name);
            QRMaskContainer.Visibility = Visibility.Visible;
            QRTipBlock.Text = msg;
        }

        private void HideQRTip()
        {
            QRMaskContainer.Visibility = Visibility.Collapsed;
            QRTipBlock.Text = string.Empty;
        }

        private async void OnSwitchButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await SwitchLoginTypeAsync(_loginType == LoginType.Password ? LoginType.QRCode : LoginType.Password);
        }

        private async void OnRefreshQRButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await LoadQRCodeAsync();
        }
    }
}
