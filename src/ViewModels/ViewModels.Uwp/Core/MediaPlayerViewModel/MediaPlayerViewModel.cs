// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Live;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using Splat;
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
            CoreDispatcher dispatcher,
            DisplayRequest displayRequest)
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
            _displayRequest = displayRequest;
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

            ReloadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
            ChangePartCommand = ReactiveCommand.CreateFromTask<VideoIdentifier>(ChangePartAsync);
            ResetProgressHistoryCommand = ReactiveCommand.Create(ResetProgressHistory);
            ClearCommand = ReactiveCommand.Create(Reset);
            ChangeLiveAudioOnlyCommand = ReactiveCommand.CreateFromTask<bool>(ChangeLiveAudioOnlyAsync);
            ChangeFormatCommand = ReactiveCommand.CreateFromTask<FormatInformation>(ChangeFormatAsync);
            ShowNextVideoTipCommand = ReactiveCommand.Create(ShowNextVideoTip);
            PlayNextCommand = ReactiveCommand.Create(PlayNextVideo);
            SelectInteractionChoiceCommand = ReactiveCommand.Create<InteractionInformation>(SelectInteractionChoice);
            BackToInteractionVideoStartCommand = ReactiveCommand.Create(BackToInteractionVideoStart);

            PlayPauseCommand = ReactiveCommand.CreateFromTask(PlayPauseAsync);
            ForwardSkipCommand = ReactiveCommand.CreateFromTask(ForwardSkipAsync);
            BackwardSkipCommand = ReactiveCommand.CreateFromTask(BackwardSkipAsync);
            ChangePlayRateCommand = ReactiveCommand.CreateFromTask<double>(ChangePlayRateAsync);
            ChangeVolumeCommand = ReactiveCommand.Create<double>(ChangeVolume);
            ToggleFullScreenCommand = ReactiveCommand.Create(ToggleFullScreenMode);
            ToggleFullWindowCommand = ReactiveCommand.Create(ToggleFullWindowMode);
            ToggleCompactOverlayCommand = ReactiveCommand.Create(ToggleCompactOverlayMode);
            ExitFullPlayerCommand = ReactiveCommand.Create(ExitFullPlayer);
            ScreenShotCommand = ReactiveCommand.CreateFromTask(ScreenShotAsync);
            ChangeProgressCommand = ReactiveCommand.Create<double>(ChangeProgress);
            StartTempQuickPlayCommand = ReactiveCommand.CreateFromTask(StartTempQuickPlayAsync);
            StopTempQuickPlayCommand = ReactiveCommand.CreateFromTask(StopTempQuickPlayAsync);
            JumpToLastProgressCommand = ReactiveCommand.Create(JumpToLastProgress);
            ReportViewProgressCommand = ReactiveCommand.CreateFromTask(ReportViewProgressAsync);
            IncreasePlayRateCommand = ReactiveCommand.Create(IncreasePlayRate);
            DecreasePlayRateCommand = ReactiveCommand.Create(DecreasePlayRate);
            IncreaseVolumeCommand = ReactiveCommand.Create(IncreaseVolume);
            DecreaseVolumeCommand = ReactiveCommand.Create(DecreaseVolume);
            BackToDefaultModeCommand = ReactiveCommand.Create(BackToDefaultMode);
            ClearSourceProgressCommand = ReactiveCommand.Create(ClearSourceProgress);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading);

            ReloadCommand.ThrownExceptions.Subscribe(DisplayException);
            ReportViewProgressCommand.ThrownExceptions.Subscribe(LogException);

            InitializeTimers();

            this.WhenAnyValue(p => p.PlaybackRate)
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(ChangePlayRateCommand);

            this.WhenAnyValue(p => p.DisplayMode)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    InitializeDisplayModeText();
                    if (_navigationViewModel.IsPlayViewShown)
                    {
                        _appViewModel.IsShowTitleBar = x == PlayerDisplayMode.Default;
                    }

                    _navigationViewModel.RemoveBackStack(Models.Enums.App.BackBehavior.PlayerModeChange);
                    if (x != PlayerDisplayMode.Default)
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
                });

            this.WhenAnyValue(p => p.IsShowMediaTransport)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    if (_appViewModel.IsXbox)
                    {
                        _navigationViewModel.RemoveBackStack(Models.Enums.App.BackBehavior.PlayerPopupShown);
                        if (x)
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
                });

            this.WhenAnyValue(p => p.IsLoop)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    _player?.SetLoop(x);
                });

            this.WhenAnyValue(p => p.IsError)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => CheckExitFullPlayerButtonVisibility());
        }

        /// <inheritdoc/>
        public void SetVideoData(VideoPlayerView data)
        {
            _viewData = data;
            _videoType = VideoType.Video;
            ReloadCommand.Execute().Subscribe();
        }

        /// <inheritdoc/>
        public void SetPgcData(PgcPlayerView view, EpisodeInformation episode)
        {
            _viewData = view;
            _currentEpisode = episode;
            _videoType = VideoType.Pgc;
            ReloadCommand.Execute().Subscribe();
        }

        /// <inheritdoc/>
        public void SetLiveData(LivePlayerView data)
        {
            _viewData = data;
            _videoType = VideoType.Live;
            ReloadCommand.Execute().Subscribe();
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
            ResetPlayer();
            ResetVideoData();
            ResetLiveData();
            InitializePlaybackRates();
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
                    ? Locator.Current.GetService<IFFmpegPlayerViewModel>()
                    : playerType switch
                    {
                        PlayerType.FFmpeg => Locator.Current.GetService<IFFmpegPlayerViewModel>(),
                        _ => Locator.Current.GetService<INativePlayerViewModel>(),
                    };
            }

            _player.MediaOpened += OnMediaOpened;
            _player.MediaPlayerChanged += OnMediaPlayerChanged;
            _player.PositionChanged += OnMediaPositionChanged;
            _player.StateChanged += OnMediaStateChanged;
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
    }
}
