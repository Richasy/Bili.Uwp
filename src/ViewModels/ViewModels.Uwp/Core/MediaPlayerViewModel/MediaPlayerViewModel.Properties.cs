// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using FFmpegInterop;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IFileToolkit _fileToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly AccountViewModel _accountViewModel;
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly FFmpegInteropConfig _liveConfig;

        private VideoType _videoType;
        private object _viewData;
        private VideoIdentifier _currentPart;
        private MediaInformation _mediaInformation;
        private SegmentInformation _video;
        private SegmentInformation _audio;
        private MediaPlayer _mediaPlayer;
        private MediaPlaybackItem _playbackItem;
        private FFmpegInteropMSS _interopMSS;
        private TimeSpan _lastReportProgress;
        private TimeSpan _initializeProgress;
        private double _originalPlayRate;

        private DispatcherTimer _progressTimer;
        private DispatcherTimer _heartBeatTimer;
        private DispatcherTimer _subtitleTimer;

        /// <summary>
        /// 媒体播放器改变.
        /// </summary>
        public event EventHandler<MediaPlayer> MediaPlayerChanged;

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 改变分P的命令.
        /// </summary>
        public ReactiveCommand<VideoIdentifier, Unit> ChangePartCommand { get; }

        /// <summary>
        /// 视频格式集合.
        /// </summary>
        public ObservableCollection<FormatInformation> Formats { get; }

        /// <summary>
        /// 播放器状态.
        /// </summary>
        [Reactive]
        public PlayerStatus Status { get; set; }

        /// <summary>
        /// 播放器显示模式.
        /// </summary>
        [Reactive]
        public PlayerDisplayMode DisplayMode { get; set; }

        /// <summary>
        /// 当前的视频格式.
        /// </summary>
        [Reactive]
        public FormatInformation CurrentFormat { get; set; }

        /// <summary>
        /// 音量.
        /// </summary>
        [Reactive]
        public double Volume { get; set; }

        /// <summary>
        /// 播放速率.
        /// </summary>
        [Reactive]
        public double PlaybackRate { get; set; }

        /// <summary>
        /// 是否循环播放.
        /// </summary>
        [Reactive]
        public bool IsLoop { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 是否显示历史记录提示.
        /// </summary>
        [Reactive]
        public bool IsShowProgressTip { get; set; }

        /// <summary>
        /// 进度提示.
        /// </summary>
        [Reactive]
        public string ProgressTip { get; set; }
    }
}
