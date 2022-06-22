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
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Common;
using FFmpegInteropX;
using ReactiveUI;
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

            _liveConfig = new MediaSourceConfig();
            _liveConfig.FFmpegOptions.Add("referer", "https://live.bilibili.com/");
            _liveConfig.FFmpegOptions.Add("user-agent", "Mozilla/5.0 BiliDroid/1.12.0 (bbcallen@gmail.com)");

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
            ShowNextVideoTipCommand = ReactiveCommand.Create<Action>(ShowNextVideoTip, outputScheduler: RxApp.MainThreadScheduler);
            PlayNextVideoCommand = ReactiveCommand.Create(PlayNextVideo, outputScheduler: RxApp.MainThreadScheduler);
            SelectInteractionChoiceCommand = ReactiveCommand.Create<InteractionInformation>(SelectInteractionChoice, outputScheduler: RxApp.MainThreadScheduler);
            BackToInteractionVideoStartCommand = ReactiveCommand.Create(BackToInteractionVideoStart, outputScheduler: RxApp.MainThreadScheduler);

            PlayPauseCommand = ReactiveCommand.CreateFromTask(PlayPauseAsync, outputScheduler: RxApp.MainThreadScheduler);
            ForwardSkipCommand = ReactiveCommand.CreateFromTask(ForwardSkipAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangePlayRateCommand = ReactiveCommand.CreateFromTask<double>(ChangePlayRateAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangeVolumeCommand = ReactiveCommand.Create<double>(ChangeVolume, outputScheduler: RxApp.MainThreadScheduler);
            ToggleFullScreenCommand = ReactiveCommand.Create(ToggleFullScreenMode, outputScheduler: RxApp.MainThreadScheduler);
            ToggleFullWindowCommand = ReactiveCommand.Create(ToggleFullWindowMode, outputScheduler: RxApp.MainThreadScheduler);
            ToggleCompactOverlayCommand = ReactiveCommand.Create(ToggleCompactOverlayMode, outputScheduler: RxApp.MainThreadScheduler);
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

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading, scheduler: RxApp.MainThreadScheduler);

            ReloadCommand.ThrownExceptions.Subscribe(DisplayException);

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
                });

            this.WhenAnyValue(p => p.InteractionProgressSeconds)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    InteractionProgressText = _numberToolkit.FormatDurationText(TimeSpan.FromSeconds(x), DurationSeconds > 3600);
                });

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
                });
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

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = GetErrorMessage(exception);
            ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed)}\n{msg}";
            LogException(exception);
        }

        private void Reset()
        {
            ResetPlayer();
            ResetMediaData();
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

            StartTimersAndDisplayRequest();
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
            var needResume = Status == PlayerStatus.Playing && _mediaPlayer != null;
            _mediaPlayer?.Pause();
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
                _mediaPlayer?.Play();
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
    }
}
