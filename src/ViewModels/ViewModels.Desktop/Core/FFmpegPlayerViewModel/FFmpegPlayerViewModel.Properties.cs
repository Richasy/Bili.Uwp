// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Args;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.Input;
using FFmpegInteropX;
using Microsoft.UI.Dispatching;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Web.Http;

namespace Bili.ViewModels.Desktop.Core
{
    /// <summary>
    /// 使用 FFmpeg 的播放器视图模型.
    /// </summary>
    public sealed partial class FFmpegPlayerViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly DispatcherQueue _dispatcherQueue;
        private readonly MediaSourceConfig _liveConfig;
        private readonly MediaSourceConfig _videoConfig;

        private SegmentInformation _video;
        private SegmentInformation _audio;
        private FFmpegMediaSource _videoFFSource;
        private FFmpegMediaSource _audioFFSource;
        private HttpClient _videoHttpClient;
        private HttpClient _audioHttpClient;
        private HttpRandomAccessStream _videoStream;
        private HttpRandomAccessStream _audioStream;
        private MediaPlayer _videoPlayer;
        private MediaPlayer _audioPlayer;
        private MediaPlaybackItem _videoPlaybackItem;
        private MediaPlaybackItem _audioPlaybackItem;
        private MediaTimelineController _mediaTimelineController;
        private MediaPlaybackSession _videoCurrentSession;
        private int _liveRetryCount;
        private int _videoRetryCount;
        private bool _shouldPreventSkip;

        /// <inheritdoc/>
        public event EventHandler MediaOpened;

        /// <inheritdoc/>
        public event EventHandler MediaEnded;

        /// <inheritdoc/>
        public event EventHandler<MediaStateChangedEventArgs> StateChanged;

        /// <inheritdoc/>
        public event EventHandler<MediaPositionChangedEventArgs> PositionChanged;

        /// <inheritdoc/>
        public event EventHandler<object> MediaPlayerChanged;

        /// <inheritdoc/>
        public IRelayCommand ClearCommand { get; }

        /// <inheritdoc/>
        public TimeSpan Position => _videoCurrentSession?.Position ?? TimeSpan.Zero;

        /// <inheritdoc/>
        public TimeSpan Duration => _videoFFSource != null
            ? _videoFFSource.Duration
            : _videoPlayer?.PlaybackSession?.NaturalDuration ?? TimeSpan.Zero;

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
