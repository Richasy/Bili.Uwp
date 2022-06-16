// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体播放器.
    /// </summary>
    public sealed class BiliMediaPlayer : ReactiveControl<MediaPlayerViewModel>
    {
        private MediaPlayerElement _mediaPlayerElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiliMediaPlayer"/> class.
        /// </summary>
        public BiliMediaPlayer() => DefaultStyleKey = typeof(BiliMediaPlayer);

        internal override void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MediaPlayerViewModel oldVM)
            {
                oldVM.MediaPlayerChanged -= OnMediaPlayerChanged;
            }

            var vm = e.NewValue as MediaPlayerViewModel;
            vm.MediaPlayerChanged -= OnMediaPlayerChanged;
            vm.MediaPlayerChanged += OnMediaPlayerChanged;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _mediaPlayerElement = GetTemplateChild("MediaPlayerElement") as MediaPlayerElement;
        }

        private void OnMediaPlayerChanged(object sender, MediaPlayer e)
            => _mediaPlayerElement.SetMediaPlayer(e);
    }
}
