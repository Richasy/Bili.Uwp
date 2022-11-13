// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.SignIn.Desktop
{
    /// <summary>
    /// 用户登录对话框.
    /// </summary>
    public sealed partial class SignInDialog : ContentDialog
    {
        private readonly TaskCompletionSource<AuthorizeResult> _taskCompletionSource;
        private readonly AuthorizeProvider _authorizeProvider;

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

        private void OnClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
            => _authorizeProvider.QRCodeStatusChanged -= OnQRCodeStatusChanged;

        private async void OnOpenedAsync(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            await LoadQRCodeAsync();
            _authorizeProvider.QRCodeStatusChanged += OnQRCodeStatusChanged;
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

        private void OnCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => _taskCompletionSource.SetCanceled();

        private async Task LoadQRCodeAsync()
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

        private async void OnRefreshQRButtonClickAsync(object sender, RoutedEventArgs e)
            => await LoadQRCodeAsync();
    }
}
