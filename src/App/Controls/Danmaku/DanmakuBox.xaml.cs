// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Richasy.Bili.ViewModels.Uwp.Common;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕输入控件.
    /// </summary>
    public sealed partial class DanmakuBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuBox"/> class.
        /// </summary>
        public DanmakuBox()
        {
            InitializeComponent();
            ViewModel = DanmakuViewModel.Instance;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DanmakuViewModel ViewModel { get; private set; }

        private async void OnDanmakuInputBoxSubmittedAsync(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                sender.IsEnabled = false;
                var result = await ViewModel.SendDanmakuAsync(args.QueryText);
                sender.IsEnabled = true;

                if (result)
                {
                    sender.Text = string.Empty;
                    (PlayerViewModel.Instance.BiliPlayer.TransportControls as BiliPlayerTransportControls).CheckPlayPauseButtonFocus();
                }
                else
                {
                    sender.Focus(Windows.UI.Xaml.FocusState.Programmatic);
                }
            }
        }

        private void OnSendFlyoutOpened(object sender, object e)
        {
            SendOptions.Initialize();
        }

        private void OnDanmakuInputBoxGotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => PlayerViewModel.Instance.IsFocusInputControl = true;
    }
}
