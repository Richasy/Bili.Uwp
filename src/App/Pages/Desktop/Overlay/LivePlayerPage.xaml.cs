// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Local;
using Bili.ViewModels.Uwp.Live;
using Windows.Media.Playback;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 直播播放页面.
    /// </summary>
    public sealed partial class LivePlayerPage : LivePlayerPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlayerPage"/> class.
        /// </summary>
        public LivePlayerPage()
        {
            InitializeComponent();
            ViewModel.MediaPlayerViewModel.MediaPlayerChanged += OnMediaPlayerChanged;
        }

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
            => ViewModel.ClearCommand.Execute().Subscribe();

        private void OnMediaPlayerChanged(object sender, MediaPlayer e)
            => PlayerElement.SetMediaPlayer(e);

        private void OnLiveOnlyAudioToggledAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var isAudioOnly = LiveAudioOnlySwitch.IsOn;
            if (ViewModel.MediaPlayerViewModel.IsLiveAudioOnly != isAudioOnly)
            {
                ViewModel.MediaPlayerViewModel.ChangeLiveAudioOnlyCommand.Execute(isAudioOnly).Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="LivePlayerPage"/> 的基类.
    /// </summary>
    public class LivePlayerPageBase : AppPage<LivePlayerPageViewModel>
    {
    }
}
