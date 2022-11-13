// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Base;
using Bili.Models.Data.Local;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

namespace Bili.Desktop.App.Pages.Xbox.Overlay
{
    /// <summary>
    /// 直播播放页面.
    /// </summary>
    public sealed partial class LivePlayerPage : LivePlayerPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlayerPage"/> class.
        /// </summary>
        public LivePlayerPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PlaySnapshot shot)
            {
                ViewModel.SetSnapshot(shot);
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
            => ViewModel.ClearCommand.Execute(null);

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.GamepadLeftShoulder)
            {
                // 后退
                ViewModel.MediaPlayerViewModel.BackwardSkipCommand.ExecuteAsync(null);
            }
            else if (e.Key == Windows.System.VirtualKey.GamepadRightShoulder)
            {
                // 跳进
                ViewModel.MediaPlayerViewModel.ForwardSkipCommand.ExecuteAsync(null);
            }
            else if (e.Key == Windows.System.VirtualKey.GamepadB)
            {
                // 关闭控制器.
                if (ViewModel.MediaPlayerViewModel.IsShowMediaTransport)
                {
                    ViewModel.MediaPlayerViewModel.IsShowMediaTransport = false;
                    e.Handled = true;
                }
            }
        }

        private void OnOnlyAudioButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var isAudioOnly = OnlyAudioButton.IsChecked.Value;
            if (ViewModel.MediaPlayerViewModel.IsLiveAudioOnly != isAudioOnly)
            {
                ViewModel.MediaPlayerViewModel.ChangeLiveAudioOnlyCommand.Execute(isAudioOnly);
            }
        }

        private void OnOpenInBroswerButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            OpenInBroswerButton.IsChecked = false;
        }
    }
}
