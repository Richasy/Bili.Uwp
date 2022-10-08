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
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.Media;
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
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly IAppToolkit _appToolkit;
        private readonly IAccountViewModel _accountViewModel;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly ICallerViewModel _callerViewModel;
        private readonly IAppViewModel _appViewModel;
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;

        private IPlayerViewModel _player;
        private VideoType _videoType;
        private object _viewData;
        private VideoIdentifier _currentPart;
        private EpisodeInformation _currentEpisode;
        private LivePlaylineInformation _currentPlayline;
        private LiveMediaInformation _liveMediaInformation;
        private MediaInformation _mediaInformation;
        private SegmentInformation _video;
        private SegmentInformation _audio;
        private TimeSpan _lastReportProgress;
        private TimeSpan _initializeProgress;
        private Action _playNextAction;
        private SystemMediaTransportControls _systemMediaTransportControls;
        private DisplayRequest _displayRequest;

        private DispatcherTimer _unitTimer;
        private DispatcherTimer _progressTimer;

        private double _originalPlayRate;
        private double _originalDanmakuSpeed;
        private double _presetVolumeHoldTime;

        /// <inheritdoc/>
        public event EventHandler<object> MediaPlayerChanged;

        /// <inheritdoc/>
        public event EventHandler<string> RequestShowTempMessage;

        /// <inheritdoc/>
        public event EventHandler MediaEnded;

        /// <inheritdoc/>
        public event EventHandler<VideoIdentifier> InternalPartChanged;

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <inheritdoc/>
        public ObservableCollection<FormatInformation> Formats { get; }

        /// <inheritdoc/>
        public ObservableCollection<IPlaybackRateItemViewModel> PlaybackRates { get; }

        /// <inheritdoc/>
        public ISubtitleModuleViewModel SubtitleViewModel { get; }

        /// <inheritdoc/>
        public IDanmakuModuleViewModel DanmakuViewModel { get; }

        /// <inheritdoc/>
        public IInteractionModuleViewModel InteractionViewModel { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public PlayerStatus Status { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public PlayerDisplayMode DisplayMode { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public FormatInformation CurrentFormat { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double Volume { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double PlaybackRate { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double MaxPlaybackRate { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double PlaybackRateStep { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double DurationSeconds { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double ProgressSeconds { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ProgressText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string DurationText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsLoop { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowProgressTip { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ProgressTip { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsLiveAudioOnly { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string FullScreenText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string FullWindowText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string CompactOverlayText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowMediaTransport { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string NextVideoTipText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowNextVideoTip { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double NextVideoCountdown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double ProgressTipCountdown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsInteractionVideo { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowInteractionChoices { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsInteractionEnd { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsBuffering { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsMediaPause { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string Cover { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool CanPlayNextPart { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowExitFullPlayerButton { get; set; }
    }
}
