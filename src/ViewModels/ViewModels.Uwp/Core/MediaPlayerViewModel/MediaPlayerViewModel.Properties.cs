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
        private readonly DisplayRequest _displayRequest;

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
        private bool _disposedValue;

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
        [Reactive]
        public PlayerStatus Status { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public PlayerDisplayMode DisplayMode { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public FormatInformation CurrentFormat { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double Volume { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double PlaybackRate { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double MaxPlaybackRate { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double PlaybackRateStep { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double DurationSeconds { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double ProgressSeconds { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ProgressText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string DurationText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsLoop { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowProgressTip { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ProgressTip { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsLiveAudioOnly { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string FullScreenText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string FullWindowText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string CompactOverlayText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowMediaTransport { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string NextVideoTipText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowNextVideoTip { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double NextVideoCountdown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double ProgressTipCountdown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsInteractionVideo { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowInteractionChoices { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsInteractionEnd { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsBuffering { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsMediaPause { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string Cover { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool CanPlayNextPart { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowExitFullPlayerButton { get; set; }
    }
}
