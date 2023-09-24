// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.App.Other;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Microsoft.QueryStringDotNET;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.SignIn.Uwp
{
    /// <summary>
    /// 用户登录对话框.
    /// </summary>
    public sealed partial class SignInDialog : ContentDialog
    {
        /// <summary>
        /// <see cref="IsShowWebView"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowWebViewProperty =
            DependencyProperty.Register(nameof(IsShowWebView), typeof(bool), typeof(SignInDialog), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="IsShowSwitchButton"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowSwitchButtonProperty =
            DependencyProperty.Register(nameof(IsShowSwitchButton), typeof(bool), typeof(SignInDialog), new PropertyMetadata(default));

        private readonly TaskCompletionSource<AuthorizeResult> _taskCompletionSource;
        private readonly AuthorizeProvider _authorizeProvider;
        private LoginType _loginType;
        private string _sessionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignInDialog"/> class.
        /// </summary>
        internal SignInDialog(
            TaskCompletionSource<AuthorizeResult> taskCompletionSource,
            AuthorizeProvider authorizeProvider)
        {
            InitializeComponent();
            _authorizeProvider = authorizeProvider;
            _taskCompletionSource = taskCompletionSource;
            Opened += OnOpenedAsync;
            Closed += OnClosed;
        }

        /// <summary>
        /// 是否显示网页视图.
        /// </summary>
        public bool IsShowWebView
        {
            get { return (bool)GetValue(IsShowWebViewProperty); }
            set { SetValue(IsShowWebViewProperty, value); }
        }

        /// <summary>
        /// 是否显示登录切换按钮.
        /// </summary>
        public bool IsShowSwitchButton
        {
            get { return (bool)GetValue(IsShowSwitchButtonProperty); }
            set { SetValue(IsShowSwitchButtonProperty, value); }
        }

        private void OnClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
            => _authorizeProvider.QRCodeStatusChanged -= OnQRCodeStatusChanged;

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
                    Hide();
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
            IsShowWebView = false;
            _taskCompletionSource.SetCanceled();
        }

        private async Task HandlePasswordLoginAsync(Dictionary<string, string> loginParamters = null)
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
                    if (loginParamters == null)
                    {
                        _sessionId = Guid.NewGuid().ToString("N");
                        loginParamters = new Dictionary<string, string>
                        {
                            { Query.LoginSessionId, _sessionId },
                        };
                    }

                    IsPrimaryButtonEnabled = false;
                    var result = await _authorizeProvider?.InternalLoginAsync(userName, password, additionalParams: loginParamters);
                    if (HandleAuthorizeResult(result))
                    {
                        Hide();
                    }
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
                }

                IsPrimaryButtonEnabled = true;
            }
        }

        private async Task LoadQRCodeAsync()
        {
            HideQRTip();
            QRLoadingRing.IsActive = true;
            var bitmapImage = await _authorizeProvider.GetQRImageAsync();
            if (bitmapImage != null)
            {
                QRCodeImage.Source = bitmapImage;
                _authorizeProvider.StartQRLoginListener();
                QRTipBlock.Visibility = Visibility.Visible;
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
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            switch (type)
            {
                case LoginType.Password:
                    _authorizeProvider.StopQRLoginListener();
                    PasswordLoginContainer.Visibility = Visibility.Visible;
                    QRLoginContainer.Visibility = Visibility.Collapsed;
                    PrimaryButtonText = resourceToolkit.GetLocaleString(LanguageNames.SignIn);
                    TipBlock.Text = resourceToolkit.GetLocaleString(LanguageNames.PasswordLoginTip);
                    SwitchActionButton.Content = resourceToolkit.GetLocaleString(LanguageNames.SwitchToQRLogin);
                    break;
                case LoginType.QRCode:
                    PasswordLoginContainer.Visibility = Visibility.Collapsed;
                    QRLoginContainer.Visibility = Visibility.Visible;
                    PrimaryButtonText = string.Empty;
                    TipBlock.Text = resourceToolkit.GetLocaleString(LanguageNames.QRLoginTip);
                    SwitchActionButton.Content = resourceToolkit.GetLocaleString(LanguageNames.SwitchToPasswordLogin);
                    IsShowWebView = false;
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
            AuthorizeProgressBar.Visibility = Visibility.Collapsed;
            IsPrimaryButtonEnabled = true;
            UserNameBox.IsEnabled = true;
            PasswordBox.IsEnabled = true;
        }

        private void ShowError(LanguageNames name)
        {
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            var msg = resourceToolkit.GetLocaleString(name);
            ShowError(msg);
        }

        private void HideError()
            => ErrorBlock.Visibility = Visibility.Collapsed;

        private void ShowQRTip(LanguageNames name)
        {
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
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
            => await SwitchLoginTypeAsync(_loginType == LoginType.Password ? LoginType.QRCode : LoginType.Password);

        private async void OnRefreshQRButtonClickAsync(object sender, RoutedEventArgs e)
            => await LoadQRCodeAsync();

        private void OnSessionWebViewNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var query = QueryString.Parse(args.Uri.Query.TrimStart('?'));
            if (query.TryGetValue("access_key", out var token))
            {
                // 登录成功.
                IsShowWebView = false;
                query.TryGetValue("mid", out var mid);
                var tokenResult = new AuthorizeResult()
                {
                    Status = 0,
                    TokenInfo = new TokenInfo
                    {
                        AccessToken = token,
                        Mid = Convert.ToInt64(mid),
                        ExpiresIn = Convert.ToInt32(DateTimeOffset.Now.AddDays(14).ToUnixTimeSeconds()),
                    },
                };

                _taskCompletionSource.SetResult(tokenResult);
                Hide();
            }
            else if (IsRedirectUrl(args.Uri.AbsoluteUri))
            {
                AuthorizeProgressBar.Visibility = Visibility.Visible;
                IsPrimaryButtonEnabled = false;
                UserNameBox.IsEnabled = false;
                PasswordBox.IsEnabled = false;
                IsShowWebView = false;
            }
        }

        private bool HandleAuthorizeResult(AuthorizeResult result)
        {
            if (result.Status == 0)
            {
                _taskCompletionSource.SetResult(result);
            }
            else if (result.Status == 1 || result.Status == 2)
            {
                // 需要安全验证
                IsShowWebView = true;
                SessionWebView.Navigate(new Uri(result.Url));
            }

            return result.Status == 0;
        }

        private async void OnSessionWebViewNavigationCompletedAsync(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (IsRedirectUrl(args.Uri.AbsoluteUri))
            {
                var uri = await _authorizeProvider.GetConfirmUriAsync();
                if (!string.IsNullOrEmpty(uri))
                {
                    SessionWebView.Navigate(new Uri(uri));
                }
                else
                {
                    ShowError(LanguageNames.LoginFailed);
                }
            }
        }

        private bool IsRedirectUrl(string url)
            => url == "https://passport.bilibili.com/ajax/miniLogin/redirect"
            || url == "https://www.bilibili.com/";
    }
}
