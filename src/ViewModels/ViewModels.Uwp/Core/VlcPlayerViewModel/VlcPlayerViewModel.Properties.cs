// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.App.Args;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using LibVLCSharp.Shared;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// VLC 播放器视图模型.
    /// </summary>
    public sealed partial class VlcPlayerViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly AppViewModel _appViewModel;
        private readonly CoreDispatcher _dispatcher;

        private SegmentInformation _video;
        private SegmentInformation _audio;
        private HttpRandomAccessStream _videoStream;
        private HttpRandomAccessStream _audioStream;
        private MediaInput _videoInput;
        private MediaInput _audioInput;
        private Media _videoMedia;
        private Media _audioMedia;
        private MediaPlayer _videoPlayer;
        private MediaPlayer _audioPlayer;

        /// <inheritdoc/>
        public event EventHandler MediaOpened;

        /// <inheritdoc/>
        public event EventHandler<Models.App.Args.MediaStateChangedEventArgs> StateChanged;

        /// <inheritdoc/>
        public event EventHandler<MediaPositionChangedEventArgs> PositionChanged;

        /// <inheritdoc/>
        public event EventHandler<object> MediaPlayerChanged;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <inheritdoc/>
        public TimeSpan Position => GetPosition();

        /// <inheritdoc/>
        public TimeSpan Duration => GetDuration();

        /// <inheritdoc/>
        public double Volume => Convert.ToDouble(_videoPlayer?.Volume ?? 100d);

        /// <inheritdoc/>
        public double PlayRate => _videoPlayer?.Rate ?? -1d;

        /// <inheritdoc/>
        public PlayerStatus Status { get; set; }

        /// <inheritdoc/>
        public bool IsLoop { get; set; }

        /// <inheritdoc/>
        public bool IsPlayerReady => _videoPlayer != null && _videoPlayer.WillPlay;
    }
}
