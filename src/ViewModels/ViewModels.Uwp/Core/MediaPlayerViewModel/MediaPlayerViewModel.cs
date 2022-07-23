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
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Common;
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
    public sealed partial class MediaPlayerViewModel : ViewModelBase, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerViewModel"/> class.
        /// </summary>
        public MediaPlayerViewModel(
            IPlayerProvider playerProvider,
            ILiveProvider liveProvider,
            IResourceToolkit resourceToolkit,
            IFileToolkit fileToolkit,
            ISettingsToolkit settingsToolkit,
            INumberToolkit numberToolkit,
            IAppToolkit appToolkit,
            AccountViewModel accountViewModel,
            NavigationViewModel navigationViewModel,
            SubtitleModuleViewModel subtitleModuleViewModel,
            DanmakuModuleViewModel danmakuModuleViewModel,
            InteractionModuleViewModel interactionModuleViewModel,
            AppViewModel appViewModel,
            CoreDispatcher dispatcher,
            DisplayRequest displayRequest)
        {
            _playerProvider = playerProvider;
            _liveProvider = liveProvider;
            _resourceToolkit = resourceToolkit;
            _fileToolkit = fileToolkit;
            _settingsToolkit = settingsToolkit;
            _numberToolkit = numberToolkit;
            _appToolkit = appToolkit;
            _accountViewModel = accountViewModel;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;
            _dispatcher = dispatcher;
            _displayRequest = displayRequest;
            SubtitleViewModel = subtitleModuleViewModel;
            DanmakuViewModel = danmakuModuleViewModel;
            InteractionViewModel = interactionModuleViewModel;
            ApplicationView.GetForCurrentView().VisibleBoundsChanged += OnViewVisibleBoundsChanged;
            InteractionViewModel.NoMoreChoices += OnInteractionModuleNoMoreChoices;

            Volume = _settingsToolkit.ReadLocalSetting(SettingNames.Volume, 100d);
            PlaybackRate = _settingsToolkit.ReadLocalSetting(SettingNames.PlaybackRate, 1d);

            Formats = new ObservableCollection<FormatInformation>();
            PlaybackRates = new ObservableCollection<PlaybackRateItemViewModel>();

            ReloadCommand = ReactiveCommand.CreateFromTask(LoadAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangePartCommand = ReactiveCommand.CreateFromTask<VideoIdentifier>(ChangePartAsync, outputScheduler: RxApp.MainThreadScheduler);
            ResetProgressHistoryCommand = ReactiveCommand.Create(ResetProgressHistory, outputScheduler: RxApp.MainThreadScheduler);
            ClearCommand = ReactiveCommand.Create(Reset, outputScheduler: RxApp.MainThreadScheduler);
            ChangeLiveAudioOnlyCommand = ReactiveCommand.CreateFromTask<bool>(ChangeLiveAudioOnlyAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangeFormatCommand = ReactiveCommand.CreateFromTask<FormatInformation>(ChangeFormatAsync, outputScheduler: RxApp.MainThreadScheduler);
            ShowNextVideoTipCommand = ReactiveCommand.Create(ShowNextVideoTip, outputScheduler: RxApp.MainThreadScheduler);
            PlayNextCommand = ReactiveCommand.Create(PlayNextVideo, outputScheduler: RxApp.MainThreadScheduler);
            SelectInteractionChoiceCommand = ReactiveCommand.Create<InteractionInformation>(SelectInteractionChoice, outputScheduler: RxApp.MainThreadScheduler);
            BackToInteractionVideoStartCommand = ReactiveCommand.Create(BackToInteractionVideoStart, outputScheduler: RxApp.MainThreadScheduler);

            PlayPauseCommand = ReactiveCommand.CreateFromTask(PlayPauseAsync, outputScheduler: RxApp.MainThreadScheduler);
            ForwardSkipCommand = ReactiveCommand.CreateFromTask(ForwardSkipAsync, outputScheduler: RxApp.MainThreadScheduler);
            BackwardSkipCommand = ReactiveCommand.CreateFromTask(BackwardSkipAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangePlayRateCommand = ReactiveCommand.CreateFromTask<double>(ChangePlayRateAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangeVolumeCommand = ReactiveCommand.Create<double>(ChangeVolume, outputScheduler: RxApp.MainThreadScheduler);
            ToggleFullScreenCommand = ReactiveCommand.Create(ToggleFullScreenMode, outputScheduler: RxApp.MainThreadScheduler);
            ToggleFullWindowCommand = ReactiveCommand.Create(ToggleFullWindowMode, outputScheduler: RxApp.MainThreadScheduler);
            ToggleCompactOverlayCommand = ReactiveCommand.Create(ToggleCompactOverlayMode, outputScheduler: RxApp.MainThreadScheduler);
            ExitFullPlayerCommand = ReactiveCommand.Create(ExitFullPlayer, outputScheduler: RxApp.MainThreadScheduler);
            ScreenShotCommand = ReactiveCommand.CreateFromTask(ScreenShotAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangeProgressCommand = ReactiveCommand.Create<double>(ChangeProgress, outputScheduler: RxApp.MainThreadScheduler);
            StartTempQuickPlayCommand = ReactiveCommand.CreateFromTask(StartTempQuickPlayAsync, outputScheduler: RxApp.MainThreadScheduler);
            StopTempQuickPlayCommand = ReactiveCommand.CreateFromTask(StopTempQuickPlayAsync, outputScheduler: RxApp.MainThreadScheduler);
            JumpToLastProgressCommand = ReactiveCommand.Create(JumpToLastProgress, outputScheduler: RxApp.MainThreadScheduler);
            ReportViewProgressCommand = ReactiveCommand.CreateFromTask(ReportViewProgressAsync, outputScheduler: RxApp.MainThreadScheduler);
            IncreasePlayRateCommand = ReactiveCommand.Create(IncreasePlayRate, outputScheduler: RxApp.MainThreadScheduler);
            DecreasePlayRateCommand = ReactiveCommand.Create(DecreasePlayRate, outputScheduler: RxApp.MainThreadScheduler);
            IncreaseVolumeCommand = ReactiveCommand.Create(IncreaseVolume, outputScheduler: RxApp.MainThreadScheduler);
            DecreaseVolumeCommand = ReactiveCommand.Create(DecreaseVolume, outputScheduler: RxApp.MainThreadScheduler);
            BackToDefaultModeCommand = ReactiveCommand.Create(BackToDefaultMode, outputScheduler: RxApp.MainThreadScheduler);
            ClearSourceProgressCommand = ReactiveCommand.Create(ClearSourceProgress, outputScheduler: RxApp.MainThreadScheduler);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading, scheduler: RxApp.MainThreadScheduler);

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

            this.WhenAnyValue(p => p.ProgressSeconds)
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(ChangeProgressCommand);

            this.WhenAnyValue(p => p.IsShowMediaTransport)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
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

        /// <summary>
        /// 设置视频播放数据.
        /// </summary>
        /// <param name="data">视频视图数据.</param>
        public void SetVideoData(VideoPlayerView data)
        {
            _viewData = data;
            _videoType = VideoType.Video;
            ReloadCommand.Execute().Subscribe();
        }

        /// <summary>
        /// 设置 PGC 播放数据.
        /// </summary>
        /// <param name="view">PGC 内容视图.</param>
        /// <param name="episode">单集信息.</param>
        public void SetPgcData(PgcPlayerView view, EpisodeInformation episode)
        {
            _viewData = view;
            _currentEpisode = episode;
            _videoType = VideoType.Pgc;
            ReloadCommand.Execute().Subscribe();
        }

        /// <summary>
        /// 设置直播播放数据.
        /// </summary>
        /// <param name="data">直播视图数据.</param>
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
            _initializeProgress = TimeSpan.Zero;
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
            if(_videoType == VideoType.Live)
            {
                _player = Locator.Current.GetService<IFFmpegPlayerViewModel>();
            }
            else
            {
                _player = playerType switch
                {
                    PlayerType.FFmpeg => Locator.Current.GetService<IFFmpegPlayerViewModel>(),
                    _ => Locator.Current.GetService<INativePlayerViewModel>(),
                };
            }

            _player.MediaOpened += OnMediaOpened;
            _player.MediaPlayerChanged += OnMediaPlayerChanged;
            _player.PositionChanged += OnMediaPositionChanged;
            _player.StateChanged += OnMediaStateChangedAsync;
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
