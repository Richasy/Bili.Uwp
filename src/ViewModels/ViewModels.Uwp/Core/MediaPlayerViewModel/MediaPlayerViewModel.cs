// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Live;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;
using Windows.Media;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel : ViewModelBase, IMediaPlayerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerViewModel"/> class.
        /// </summary>
        public MediaPlayerViewModel(
            IPlayerProvider playerProvider,
            ILiveProvider liveProvider,
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            INumberToolkit numberToolkit,
            IAppToolkit appToolkit,
            IAccountViewModel accountViewModel,
            INavigationViewModel navigationViewModel,
            ISubtitleModuleViewModel subtitleModuleViewModel,
            IDanmakuModuleViewModel danmakuModuleViewModel,
            IInteractionModuleViewModel interactionModuleViewModel,
            ICallerViewModel callerViewModel,
            IAppViewModel appViewModel,
            CoreDispatcher dispatcher)
        {
            _playerProvider = playerProvider;
            _liveProvider = liveProvider;
            _resourceToolkit = resourceToolkit;
            _settingsToolkit = settingsToolkit;
            _numberToolkit = numberToolkit;
            _appToolkit = appToolkit;
            _accountViewModel = accountViewModel;
            _callerViewModel = callerViewModel;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;
            _dispatcher = dispatcher;
            SubtitleViewModel = subtitleModuleViewModel;
            DanmakuViewModel = danmakuModuleViewModel;
            InteractionViewModel = interactionModuleViewModel;

            ApplicationView.GetForCurrentView().VisibleBoundsChanged += OnViewVisibleBoundsChanged;
            InteractionViewModel.NoMoreChoices += OnInteractionModuleNoMoreChoices;
            PropertyChanged += OnPropertyChanged;

            Volume = _settingsToolkit.ReadLocalSetting(SettingNames.Volume, 100d);
            PlaybackRate = _settingsToolkit.ReadLocalSetting(SettingNames.PlaybackRate, 1d);

            Formats = new ObservableCollection<FormatInformation>();
            PlaybackRates = new ObservableCollection<IPlaybackRateItemViewModel>();

            ReloadCommand = new AsyncRelayCommand(LoadAsync);
            ChangePartCommand = new AsyncRelayCommand<VideoIdentifier>(ChangePartAsync);
            ResetProgressHistoryCommand = new RelayCommand(ResetProgressHistory);
            ChangeLiveAudioOnlyCommand = new AsyncRelayCommand<bool>(ChangeLiveAudioOnlyAsync);
            ChangeFormatCommand = new AsyncRelayCommand<FormatInformation>(ChangeFormatAsync);
            ShowNextVideoTipCommand = new RelayCommand(ShowNextVideoTip);
            PlayNextCommand = new RelayCommand(PlayNextVideo);
            SelectInteractionChoiceCommand = new RelayCommand<InteractionInformation>(SelectInteractionChoice);
            BackToInteractionVideoStartCommand = new RelayCommand(BackToInteractionVideoStart);

            PlayPauseCommand = new AsyncRelayCommand(PlayPauseAsync);
            ForwardSkipCommand = new AsyncRelayCommand(ForwardSkipAsync);
            BackwardSkipCommand = new AsyncRelayCommand(BackwardSkipAsync);
            ChangePlayRateCommand = new AsyncRelayCommand<double>(ChangePlayRateAsync);
            ChangeVolumeCommand = new RelayCommand<double>(ChangeVolume);
            ToggleFullScreenCommand = new RelayCommand(ToggleFullScreenMode);
            ToggleFullWindowCommand = new RelayCommand(ToggleFullWindowMode);
            ToggleCompactOverlayCommand = new RelayCommand(ToggleCompactOverlayMode);
            ExitFullPlayerCommand = new RelayCommand(ExitFullPlayer);
            ScreenShotCommand = new AsyncRelayCommand(ScreenShotAsync);
            ChangeProgressCommand = new RelayCommand<double>(ChangeProgress);
            StartTempQuickPlayCommand = new AsyncRelayCommand(StartTempQuickPlayAsync);
            StopTempQuickPlayCommand = new AsyncRelayCommand(StopTempQuickPlayAsync);
            JumpToLastProgressCommand = new RelayCommand(JumpToLastProgress);
            ReportViewProgressCommand = new AsyncRelayCommand(ReportViewProgressAsync);
            IncreasePlayRateCommand = new RelayCommand(IncreasePlayRate);
            DecreasePlayRateCommand = new RelayCommand(DecreasePlayRate);
            IncreaseVolumeCommand = new RelayCommand(IncreaseVolume);
            DecreaseVolumeCommand = new RelayCommand(DecreaseVolume);
            BackToDefaultModeCommand = new RelayCommand(BackToDefaultMode);
            ClearSourceProgressCommand = new RelayCommand(ClearSourceProgress);
            ClearCommand = new RelayCommand(Reset);

            AttachIsRunningToAsyncCommand(p => IsReloading = p, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayException, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(LogException, ReportViewProgressCommand);

            InitializeTimers();
        }

        /// <inheritdoc/>
        public void SetVideoData(VideoPlayerView data)
        {
            _viewData = data;
            _videoType = VideoType.Video;
            ReloadCommand.ExecuteAsync(null);
        }

        /// <inheritdoc/>
        public void SetPgcData(PgcPlayerView view, EpisodeInformation episode)
        {
            _viewData = view;
            _currentEpisode = episode;
            _videoType = VideoType.Pgc;
            ReloadCommand.ExecuteAsync(null);
        }

        /// <inheritdoc/>
        public void SetLiveData(LivePlayerView data)
        {
            _viewData = data;
            _videoType = VideoType.Live;
            ReloadCommand.ExecuteAsync(null);
        }

        /// <inheritdoc/>
        public async void ActiveDisplay()
        {
            if (_displayRequest != null)
            {
                return;
            }

            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _displayRequest = new DisplayRequest();
                _displayRequest.RequestActive();
            });
        }

        /// <inheritdoc/>
        public async void ReleaseDisplay()
        {
            if (_displayRequest == null)
            {
                return;
            }

            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _displayRequest?.RequestRelease();
                _displayRequest = null;
            });
        }

        /// <summary>
        /// 设置播放下一个内容的动作.
        /// </summary>
        /// <param name="action">动作.</param>
        public void SetPlayNextAction(Action action)
            => _playNextAction = action;

        /// <inheritdoc/>
        public async void DisplayException(Exception exception)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                IsError = true;
                var msg = GetErrorMessage(exception);
                ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed)}\n{msg}";
            });

            LogException(exception);
        }

        private void Reset()
        {
            if (IsError)
            {
                // 说明解析出现了错误，可能是解码失败，此时应尝试重置清晰度.
                CurrentFormat = null;
            }

            ResetMediaData();
            ResetVideoData();
            ResetLiveData();
            InitializePlaybackRates();
            ResetPlayer();
        }

        private async Task LoadAsync()
        {
            Reset();
            if (_videoType == VideoType.Video)
            {
                await LoadVideoAsync();
            }
            else if (_videoType == VideoType.Pgc)
            {
                await LoadEpisodeAsync();
            }
            else if (_videoType == VideoType.Live)
            {
                await LoadLiveAsync();
            }

            InitializeSmtc();
        }

        private async Task ChangePartAsync(VideoIdentifier part)
        {
            if (_videoType == VideoType.Video)
            {
                await ChangeVideoPartAsync(part);
            }
            else if (_videoType == VideoType.Pgc)
            {
                await ChangeEpisodeAsync(part);
            }
        }

        private async Task ChangeFormatAsync(FormatInformation information)
        {
            var needResume = Status == PlayerStatus.Playing;
            _player.Pause();
            if (_videoType == VideoType.Video
                || _videoType == VideoType.Pgc)
            {
                await SelectVideoFormatAsync(information);
            }
            else if (_videoType == VideoType.Live)
            {
                await SelectLiveFormatAsync(information);
            }

            if (needResume)
            {
                _player.Play();
            }
        }

        private void ResetProgressHistory()
        {
            if (_videoType == VideoType.Video && _viewData is VideoPlayerView videoView)
            {
                videoView.Progress = null;
            }
            else if (_videoType == VideoType.Pgc && _viewData is PgcPlayerView pgcView)
            {
                pgcView.Progress = null;
            }
        }

        private void InitializePlayer()
        {
            var playerType = _settingsToolkit.ReadLocalSetting(SettingNames.PlayerType, PlayerType.Native);

            if (_player == null)
            {
                _player = _videoType == VideoType.Live
                    ? Locator.Instance.GetService<IFFmpegPlayerViewModel>()
                    : playerType switch
                    {
                        PlayerType.FFmpeg => Locator.Instance.GetService<IFFmpegPlayerViewModel>(),
                        _ => Locator.Instance.GetService<INativePlayerViewModel>(),
                    };

                _player.MediaOpened += OnMediaOpened;
                _player.MediaEnded += OnMediaEnded;
                _player.MediaPlayerChanged += OnMediaPlayerChanged;
                _player.PositionChanged += OnMediaPositionChanged;
                _player.StateChanged += OnMediaStateChanged;
            }
        }

        private void InitializeSmtc()
        {
            _systemMediaTransportControls = SystemMediaTransportControls.GetForCurrentView();
            _systemMediaTransportControls.IsEnabled = true;
            _systemMediaTransportControls.IsPlayEnabled = true;
            _systemMediaTransportControls.IsPauseEnabled = true;
            _systemMediaTransportControls.ButtonPressed -= OnSystemControlsButtonPressedAsync;
            _systemMediaTransportControls.ButtonPressed += OnSystemControlsButtonPressedAsync;
        }

        partial void OnPlaybackRateChanged(double value)
            => ChangePlayRateCommand?.ExecuteAsync(value);

        partial void OnDisplayModeChanged(PlayerDisplayMode value)
        {
            InitializeDisplayModeText();
            if (_navigationViewModel.IsPlayViewShown)
            {
                _appViewModel.IsShowTitleBar = value == PlayerDisplayMode.Default;
            }

            _navigationViewModel.RemoveBackStack(Models.Enums.App.BackBehavior.PlayerModeChange);
            if (value != PlayerDisplayMode.Default)
            {
                _navigationViewModel.AddBackStack(Models.Enums.App.BackBehavior.PlayerModeChange, async _ =>
                {
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        DisplayMode = PlayerDisplayMode.Default;
                    });
                });
            }

            CheckExitFullPlayerButtonVisibility();
        }

        partial void OnIsShowMediaTransportChanged(bool value)
        {
            if (_appViewModel.IsXbox)
            {
                _navigationViewModel.RemoveBackStack(Models.Enums.App.BackBehavior.PlayerPopupShown);
                if (value)
                {
                    _navigationViewModel.AddBackStack(Models.Enums.App.BackBehavior.PlayerPopupShown, async _ =>
                    {
                        await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            IsShowMediaTransport = false;
                        });
                    });
                }
            }

            CheckExitFullPlayerButtonVisibility();
        }

        partial void OnIsLoopChanged(bool value)
            => _player?.SetLoop(value);

        partial void OnIsErrorChanged(bool value)
            => CheckExitFullPlayerButtonVisibility();
    }
}
