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
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Windows.Media;
using Windows.System.Display;

namespace Bili.ViewModels.Workspace.Core
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
        private readonly DispatcherQueue _dispatcher;

        private IPlayerViewModel _player;
        private VideoType _videoType;
        private object _viewData;
        private bool _isInPrivate;
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

        [ObservableProperty]
        private bool _isReloading;

        [ObservableProperty]
        private PlayerStatus _status;

        [ObservableProperty]
        private PlayerDisplayMode _displayMode;

        [ObservableProperty]
        private FormatInformation _currentFormat;

        [ObservableProperty]
        private double _volume;

        [ObservableProperty]
        private double _playbackRate;

        [ObservableProperty]
        private double _maxPlaybackRate;

        [ObservableProperty]
        private double _playbackRateStep;

        [ObservableProperty]
        private double _durationSeconds;

        [ObservableProperty]
        private double _progressSeconds;

        [ObservableProperty]
        private string _progressText;

        [ObservableProperty]
        private string _durationText;

        [ObservableProperty]
        private bool _isLoop;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isShowProgressTip;

        [ObservableProperty]
        private string _progressTip;

        [ObservableProperty]
        private bool _isLiveAudioOnly;

        [ObservableProperty]
        private string _fullScreenText;

        [ObservableProperty]
        private string _fullWindowText;

        [ObservableProperty]
        private string _compactOverlayText;

        [ObservableProperty]
        private bool _isShowMediaTransport;

        [ObservableProperty]
        private string _nextVideoTipText;

        [ObservableProperty]
        private bool _isShowNextVideoTip;

        [ObservableProperty]
        private double _nextVideoCountdown;

        [ObservableProperty]
        private double _progressTipCountdown;

        [ObservableProperty]
        private bool _isInteractionVideo;

        [ObservableProperty]
        private bool _isShowInteractionChoices;

        [ObservableProperty]
        private bool _isInteractionEnd;

        [ObservableProperty]
        private bool _isBuffering;

        [ObservableProperty]
        private bool _isMediaPause;

        [ObservableProperty]
        private string _cover;

        [ObservableProperty]
        private bool _canPlayNextPart;

        [ObservableProperty]
        private bool _isShowExitFullPlayerButton;

        /// <inheritdoc/>
        public event EventHandler<object> MediaPlayerChanged;

        /// <inheritdoc/>
        public event EventHandler<string> RequestShowTempMessage;

        /// <inheritdoc/>
        public event EventHandler MediaEnded;

        /// <inheritdoc/>
        public event EventHandler<VideoIdentifier> InternalPartChanged;

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
    }
}
