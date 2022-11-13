// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Uwp.App.Pages.Base;
using Bili.Models.Data.Local;
using Windows.UI.Xaml.Navigation;

namespace Bili.Uwp.App.Pages.Desktop.Overlay
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

        private void OnLiveOnlyAudioToggledAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var isAudioOnly = LiveAudioOnlySwitch.IsOn;
            if (ViewModel.MediaPlayerViewModel.IsLiveAudioOnly != isAudioOnly)
            {
                ViewModel.MediaPlayerViewModel.ChangeLiveAudioOnlyCommand.Execute(isAudioOnly);
            }
        }
    }
}
