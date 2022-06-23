// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Live;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Common;
using FFmpegInteropX;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.Media.Playback;
using Windows.System.Display;
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
        private readonly IAppToolkit _appToolkit;
        private readonly AccountViewModel _accountViewModel;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly AppViewModel _appViewModel;
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly MediaSourceConfig _liveConfig;
        private readonly DisplayRequest _displayRequest;

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
        private FFmpegMediaSource _interopMSS;
        private TimeSpan _lastReportProgress;
        private TimeSpan _initializeProgress;
        private TimeSpan _interactionProgress;
        private bool _isInteractionProgressChanged;
        private Action _playNextAction;

        private DispatcherTimer _unitTimer;
        private DispatcherTimer _progressTimer;

        private double _originalPlayRate;
        private double _originalDanmakuSpeed;

        /// <summary>
        /// 媒体播放器改变.
        /// </summary>
        public event EventHandler<MediaPlayer> MediaPlayerChanged;

        /// <summary>
        /// 请求显示临时信息.
        /// </summary>
        public event EventHandler<string> RequestShowTempMessage;

        /// <summary>
        /// 媒体播放结束.
        /// </summary>
        public event EventHandler MediaEnded;

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <summary>
        /// 视频格式集合.
        /// </summary>
        public ObservableCollection<FormatInformation> Formats { get; }

        /// <summary>
        /// 播放速率的预设集合.
        /// </summary>
        public ObservableCollection<PlaybackRateItemViewModel> PlaybackRates { get; }

        /// <summary>
        /// 字幕模块视图模型.
        /// </summary>
        public SubtitleModuleViewModel SubtitleViewModel { get; }

        /// <summary>
        /// 字幕模块视图模型.
        /// </summary>
        public DanmakuModuleViewModel DanmakuViewModel { get; }

        /// <summary>
        /// 互动视频模块视图模型.
        /// </summary>
        public InteractionModuleViewModel InteractionViewModel { get; }

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
        [Reactive]
        public string CompactOverlayText { get; set; }

        /// <summary>
        /// 是否显示互动播放进度条.
        /// </summary>
        [Reactive]
        public bool IsShowInteractionProgress { get; set; }

        /// <summary>
        /// [互动进度条] 当前已播放的秒数.
        /// </summary>
        [Reactive]
        public double InteractionProgressSeconds { get; set; }

        /// <summary>
        /// [互动进度条] 当前已播放的秒数的可读文本.
        /// </summary>
        [Reactive]
        public string InteractionProgressText { get; set; }

        /// <summary>
        /// 是否显示媒体传输控件.
        /// </summary>
        [Reactive]
        public bool IsShowMediaTransport { get; set; }

        /// <summary>
        /// 显示的下一个视频提示文本.
        /// </summary>
        [Reactive]
        public string NextVideoTipText { get; set; }

        /// <summary>
        /// 是否显示下一个视频提醒.
        /// </summary>
        [Reactive]
        public bool IsShowNextVideoTip { get; set; }

        /// <summary>
        /// 自动播放下一个视频的倒计时秒数.
        /// </summary>
        [Reactive]
        public double NextVideoCountdown { get; set; }

        /// <summary>
        /// 是否为互动视频.
        /// </summary>
        [Reactive]
        public bool IsInteractionVideo { get; set; }

        /// <summary>
        /// 是否显示互动视频选项.
        /// </summary>
        [Reactive]
        public bool IsShowInteractionChoices { get; set; }

        /// <summary>
        /// 互动视频是否已结束.
        /// </summary>
        [Reactive]
        public bool IsInteractionEnd { get; set; }

        /// <summary>
        /// 是否正在缓冲.
        /// </summary>
        [Reactive]
        public bool IsBuffering { get; set; }

        /// <summary>
        /// 媒体是否暂停.
        /// </summary>
        [Reactive]
        public bool IsMediaPause { get; set; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        [Reactive]
        public string Cover { get; set; }

        /// <summary>
        /// 是否可以播放下一个分集.
        /// </summary>
        [Reactive]
        public bool CanPlayNextPart { get; set; }
    }
}
