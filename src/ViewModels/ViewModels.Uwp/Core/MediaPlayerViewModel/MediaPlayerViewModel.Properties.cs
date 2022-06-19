// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Live;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Common;
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
        private readonly ILiveProvider _liveProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IFileToolkit _fileToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly AccountViewModel _accountViewModel;
        private readonly AppViewModel _appViewModel;
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly FFmpegInteropConfig _liveConfig;

        private VideoType _videoType;
        private object _viewData;
        private VideoIdentifier _currentPart;
        private EpisodeInformation _currentEpisode;
        private LivePlaylineInformation _currentPlayline;
        private LiveMediaInformation _liveMediaInformation;
        private MediaInformation _mediaInformation;
        private SegmentInformation _video;
        private SegmentInformation _audio;
        private MediaPlayer _mediaPlayer;
        private MediaPlaybackItem _playbackItem;
        private FFmpegInteropMSS _interopMSS;
        private TimeSpan _lastReportProgress;
        private TimeSpan _initializeProgress;

        private DispatcherTimer _progressTimer;
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
        /// 改变播放器状态的命令.
        /// </summary>
        public ReactiveCommand<PlayerStatus, Unit> ChangePlayerStatusCommand { get; }

        /// <summary>
        /// 重置播放历史的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ResetProgressHistoryCommand { get; }

        /// <summary>
        /// 清除播放数据的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <summary>
        /// 改变直播源是否仅有音频的命令.
        /// </summary>
        public ReactiveCommand<bool, Unit> ChangeLiveAudioOnlyCommand { get; }

        /// <summary>
        /// 改变清晰度/视频格式命令.
        /// </summary>
        public ReactiveCommand<FormatInformation, Unit> ChangeFormatCommand { get; }

        /// <summary>
        /// 播放/暂停命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> PlayPauseCommand { get; }

        /// <summary>
        /// 跳进命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ForwardSkipCommand { get; }

        /// <summary>
        /// 改变播放速率的命令.
        /// </summary>
        public ReactiveCommand<double, Unit> ChangePlayRateCommand { get; }

        /// <summary>
        /// 改变音量的命令.
        /// </summary>
        public ReactiveCommand<double, Unit> ChangeVolumeCommand { get; }

        /// <summary>
        /// 进入/退出全屏状态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleFullScreenCommand { get; }

        /// <summary>
        /// 进入/退出全窗口状态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleFullWindowCommand { get; }

        /// <summary>
        /// 进入/退出小窗状态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleCompactOverlayCommand { get; }

        /// <summary>
        /// 截图命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ScreenShotCommand { get; }

        /// <summary>
        /// 视频格式集合.
        /// </summary>
        public ObservableCollection<FormatInformation> Formats { get; }

        /// <summary>
        /// 播放速率的预设集合.
        /// </summary>
        public ObservableCollection<PlaybackRateItemViewModel> PlaybackRates { get; }

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
        /// 最大播放速率.
        /// </summary>
        [Reactive]
        public double MaxPlaybackRate { get; set; }

        /// <summary>
        /// 播放速率调整间隔.
        /// </summary>
        [Reactive]
        public double PlaybackRateStep { get; set; }

        /// <summary>
        /// 视频时长秒数.
        /// </summary>
        [Reactive]
        public double DurationSeconds { get; set; }

        /// <summary>
        /// 当前已播放的秒数.
        /// </summary>
        [Reactive]
        public double ProgressSeconds { get; set; }

        /// <summary>
        /// 当前已播放的秒数的可读文本.
        /// </summary>
        [Reactive]
        public string ProgressText { get; set; }

        /// <summary>
        /// 视频时长秒数的可读文本.
        /// </summary>
        [Reactive]
        public string DurationText { get; set; }

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

        /// <summary>
        /// 是否仅播放直播音频.
        /// </summary>
        [Reactive]
        public bool IsLiveAudioOnly { get; set; }

        /// <summary>
        /// 全屏提示文本.
        /// </summary>
        [Reactive]
        public string FullScreenText { get; set; }

        /// <summary>
        /// 全窗口提示文本.
        /// </summary>
        [Reactive]
        public string FullWindowText { get; set; }

        /// <summary>
        /// 小窗提示文本.
        /// </summary>
        public string CompactOverlayText { get; set; }
    }
}
