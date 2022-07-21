// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.App.Args;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 原生播放器视图模型.
    /// </summary>
    public sealed partial class NativePlayerViewModel
    {
        private readonly IFileToolkit _fileToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly CoreDispatcher _dispatcher;

        private SegmentInformation _video;
        private SegmentInformation _audio;
        private MediaPlayer _videoPlayer;
        private MediaPlaybackItem _videoPlaybackItem;
        private HttpRandomAccessStream _liveStream;

        /// <inheritdoc/>
        public event EventHandler MediaOpened;

        /// <inheritdoc/>
        public event EventHandler<MediaStateChangedEventArgs> StateChanged;

        /// <inheritdoc/>
        public event EventHandler<MediaPositionChangedEventArgs> PositionChanged;

        /// <inheritdoc/>
        public event EventHandler<object> MediaPlayerChanged;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <inheritdoc/>
        public TimeSpan Position => _videoPlayer?.PlaybackSession?.Position ?? TimeSpan.Zero;

        /// <inheritdoc/>
        public TimeSpan Duration => _videoPlayer?.PlaybackSession?.NaturalDuration ?? TimeSpan.Zero;

        /// <inheritdoc/>
        public double Volume => (_videoPlayer?.Volume ?? 1d) * 100;

        /// <inheritdoc/>
        public double PlayRate => _videoPlayer?.PlaybackSession?.PlaybackRate ?? -1d;

        /// <inheritdoc/>
        public PlayerStatus Status { get; set; }

        /// <inheritdoc/>
        public bool IsLoop => _videoPlayer?.IsLoopingEnabled ?? false;

        /// <inheritdoc/>
        public bool IsPlayerReady => _videoPlayer != null && _videoPlayer.PlaybackSession != null;
    }
}
