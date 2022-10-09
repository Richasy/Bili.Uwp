// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.App.Args;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.Player;
using Bili.ViewModels.Interfaces.Common;
using Windows.Media;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private void ResetMediaData()
        {
            TryClear(Formats);
            TryClear(PlaybackRates);
            IsShowProgressTip = false;
            IsShowMediaTransport = false;
            ProgressTip = default;
            _video = null;
            _audio = null;
            CurrentFormat = null;
            IsLoop = false;
            DurationSeconds = 0;
            DurationText = "--";
            ProgressSeconds = 0;
            ProgressText = "--";
            Volume = _settingsToolkit.ReadLocalSetting(SettingNames.Volume, 100d);
            _lastReportProgress = TimeSpan.Zero;
            _initializeProgress = TimeSpan.Zero;
            _originalPlayRate = 0;
            IsInteractionEnd = false;
            IsInteractionVideo = false;
            IsShowInteractionChoices = false;
            DanmakuViewModel.ResetCommand.Execute(null);

            if (_systemMediaTransportControls != null)
            {
                _systemMediaTransportControls.DisplayUpdater.ClearAll();
                _systemMediaTransportControls.IsEnabled = false;
                _systemMediaTransportControls = null;
            }
        }

        private void InitializePlaybackRates()
        {
            var isEnhancement = _settingsToolkit.ReadLocalSetting(SettingNames.PlaybackRateEnhancement, false);
            MaxPlaybackRate = isEnhancement ? 6d : 3d;
            PlaybackRateStep = isEnhancement ? 0.2 : 0.1;

            TryClear(PlaybackRates);
            var defaultList = !isEnhancement
                ? new List<double> { 0.5, 0.75, 1.0, 1.25, 1.5, 2.0 }
                : new List<double> { 0.5, 1.0, 1.5, 2.0, 3.0, 4.0 };

            defaultList.ForEach(p =>
            {
                var vm = Locator.Instance.GetService<IPlaybackRateItemViewModel>();
                vm.InjectData(p);
                vm.InjectAction(rate => ChangePlayRateCommand.ExecuteAsync(rate));
                vm.IsSelected = p == PlaybackRate;
                PlaybackRates.Add(vm);
            });

            var isGlobal = _settingsToolkit.ReadLocalSetting(SettingNames.GlobalPlaybackRate, false);
            if (!isGlobal)
            {
                PlaybackRate = 1d;
            }
        }

        /// <summary>
        /// 清理播放数据.
        /// </summary>
        private void ResetPlayer()
        {
            ReleaseDisplay();

            if (_player != null)
            {
                _player.ClearCommand.Execute(null);
                _player = null;
            }

            _lastReportProgress = TimeSpan.Zero;
            _progressTimer?.Stop();
            _unitTimer?.Stop();

            Status = PlayerStatus.NotLoad;
        }

        private void InitializeTimers()
        {
            if (_unitTimer == null)
            {
                _unitTimer = new DispatcherTimer();
                _unitTimer.Interval = TimeSpan.FromMilliseconds(100);
                _unitTimer.Tick += OnUnitTimerTickAsync;
            }

            if (_progressTimer == null)
            {
                _progressTimer = new DispatcherTimer();
                _progressTimer.Interval = TimeSpan.FromSeconds(5);
                _progressTimer.Tick += OnProgressTimerTick;
            }
        }

        private void StartTimers()
        {
            _progressTimer?.Start();
            _unitTimer?.Start();
        }

        private string GetVideoPreferCodecId()
        {
            var preferCodec = _settingsToolkit.ReadLocalSetting(SettingNames.PreferCodec, PreferCodec.H264);
            var id = preferCodec switch
            {
                PreferCodec.H265 => "hev",
                PreferCodec.Av1 => "av01",
                _ => "avc",
            };

            return id;
        }

        private string GetLivePreferCodecId()
        {
            var preferCodec = _settingsToolkit.ReadLocalSetting(SettingNames.PreferCodec, PreferCodec.H264);
            var id = preferCodec switch
            {
                PreferCodec.H265 => "hevc",
                PreferCodec.Av1 => "av1",
                _ => "avc",
            };

            return id;
        }

        /// <summary>
        /// 在切换片源时记录当前已播放的进度，以便在切换后重新定位.
        /// </summary>
        private void MarkProgressBreakpoint()
        {
            var progress = TimeSpan.FromSeconds(ProgressSeconds);

            if (progress > TimeSpan.FromSeconds(1))
            {
                _initializeProgress = progress;
            }
        }

        private void InitializeDisplayModeText()
        {
            FullScreenText = DisplayMode == PlayerDisplayMode.FullScreen
                ? _resourceToolkit.GetLocaleString(LanguageNames.ExitFullScreen)
                : _resourceToolkit.GetLocaleString(LanguageNames.EnterFullScreen);

            FullWindowText = DisplayMode == PlayerDisplayMode.FullWindow
                ? _resourceToolkit.GetLocaleString(LanguageNames.ExitFullWindow)
                : _resourceToolkit.GetLocaleString(LanguageNames.EnterFullWindow);

            CompactOverlayText = DisplayMode == PlayerDisplayMode.CompactOverlay
                ? _resourceToolkit.GetLocaleString(LanguageNames.ExitCompactOverlay)
                : _resourceToolkit.GetLocaleString(LanguageNames.EnterCompactOverlay);
        }

        private async Task ReportViewProgressAsync()
        {
            if (_accountViewModel.State != AuthorizeState.SignedIn)
            {
                return;
            }

            var progress = _player.Position;
            if (progress != _lastReportProgress && progress > TimeSpan.Zero)
            {
                if (_videoType == VideoType.Video)
                {
                    var view = _viewData as VideoPlayerView;
                    var aid = view.Information.Identifier.Id;
                    var cid = _currentPart.Id;
                    await _playerProvider.ReportProgressAsync(aid, cid, progress.TotalSeconds);
                }
                else if (_videoType == VideoType.Pgc && _currentEpisode != null)
                {
                    var view = _viewData as PgcPlayerView;
                    var aid = _currentEpisode.VideoId;
                    var cid = _currentEpisode.PartId;
                    var epid = _currentEpisode.Identifier.Id;
                    var sid = view.Information.Identifier.Id;
                    await _playerProvider.ReportProgressAsync(aid, cid, epid, sid, progress.TotalSeconds);
                }

                _lastReportProgress = progress;
            }
        }

        private void ShowNextVideoTip()
            => IsShowNextVideoTip = true;

        private void PlayNextVideo()
            => _playNextAction?.Invoke();

        private void ClearSourceProgress()
        {
            if (_viewData is VideoPlayerView videoPlayer)
            {
                videoPlayer.Progress = default;
            }
            else if (_viewData is PgcPlayerView pgcPlayer)
            {
                pgcPlayer.Progress = default;
            }
        }

        private void CheckExitFullPlayerButtonVisibility()
        {
            var isFullPlayer = DisplayMode != PlayerDisplayMode.Default;
            IsShowExitFullPlayerButton = isFullPlayer && (IsError || IsShowMediaTransport);
        }

        private void InitializeDisplayInformation()
        {
            switch (_videoType)
            {
                case VideoType.Video:
                    FillVideoPlaybackProperties();
                    break;
                case VideoType.Pgc:
                    FillEpisodePlaybackProperties();
                    break;
                case VideoType.Live:
                    FillLivePlaybackProperties();
                    break;
                default:
                    break;
            }
        }

        private void SetDisplayProperties(string cover, string title, string subtitle, string videoType)
        {
            if (_systemMediaTransportControls == null)
            {
                return;
            }

            var updater = _systemMediaTransportControls.DisplayUpdater;
            updater.ClearAll();
            updater.Type = MediaPlaybackType.Video;
            updater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(cover));
            updater.VideoProperties.Title = title;
            updater.VideoProperties.Subtitle = subtitle;
            updater.VideoProperties.Genres.Add(videoType);
            updater.Update();
        }

        private void OnMediaStateChanged(object sender, MediaStateChangedEventArgs e)
        {
            IsError = e.Status == PlayerStatus.Failed;
            Status = e.Status;
            IsMediaPause = e.Status != PlayerStatus.Playing;
            IsBuffering = e.Status == PlayerStatus.Buffering;

            if (_systemMediaTransportControls == null)
            {
                return;
            }

            if (e.Status == PlayerStatus.Failed)
            {
                ErrorText = e.Message;
                _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
            }
            else if (e.Status == PlayerStatus.Playing)
            {
                _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                if (_player.Position < _initializeProgress)
                {
                    _player.SeekTo(_initializeProgress);
                    _initializeProgress = TimeSpan.Zero;
                }
            }
            else
            {
                _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Paused;
            }

            if (e.Status == PlayerStatus.Playing)
            {
                ActiveDisplay();
            }
            else
            {
                ReleaseDisplay();
            }
        }

        private void OnMediaPositionChanged(object sender, MediaPositionChangedEventArgs e)
        {
            DurationSeconds = e.Duration.TotalSeconds;
            ProgressSeconds = e.Position.TotalSeconds;

            DurationText = _numberToolkit.FormatDurationText(e.Duration, e.Duration.Hours > 0);
            ProgressText = _numberToolkit.FormatDurationText(e.Position, e.Duration.Hours > 0);

            if (SubtitleViewModel.HasSubtitles)
            {
                SubtitleViewModel.SeekCommand.Execute(ProgressSeconds);
            }

            var segmentIndex = Convert.ToInt32(Math.Ceiling(ProgressSeconds / 360d));
            if (segmentIndex < 1)
            {
                segmentIndex = 1;
            }

            DanmakuViewModel.LoadSegmentDanmakuCommand.ExecuteAsync(segmentIndex);
            DanmakuViewModel.SeekCommand.Execute(ProgressSeconds);
        }

        private void OnMediaPlayerChanged(object sender, object e)
            => MediaPlayerChanged?.Invoke(this, e);

        private void OnMediaOpened(object sender, EventArgs e)
        {
            ChangePlayRateCommand.ExecuteAsync(PlaybackRate);
            ChangeVolumeCommand.Execute(Volume);
            InitializeDisplayInformation();

            var autoPlay = _settingsToolkit.ReadLocalSetting(SettingNames.IsAutoPlayWhenLoaded, true);
            if (autoPlay)
            {
                _player.Play();
            }
        }

        private void OnMediaEnded(object sender, EventArgs e)
        {
            if (IsInteractionVideo)
            {
                if (InteractionViewModel.Choices.Count == 1 && string.IsNullOrEmpty(InteractionViewModel.Choices.First().Text))
                {
                    // 这是默认选项，直接切换.
                    SelectInteractionChoiceCommand.Execute(InteractionViewModel.Choices.First());
                }
                else
                {
                    IsShowInteractionChoices = true;
                }
            }

            if (!IsLoop)
            {
                MediaEnded?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnProgressTimerTick(object sender, object e)
            => ReportViewProgressCommand.ExecuteAsync(null);

        private async void OnUnitTimerTickAsync(object sender, object e)
        {
            _presetVolumeHoldTime += 100;
            if (_presetVolumeHoldTime > 300)
            {
                _presetVolumeHoldTime = 0;
                if (_player != null && Volume != _player.Volume)
                {
                    await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        var msg = Volume > 0
                            ? $"{_resourceToolkit.GetLocaleString(LanguageNames.CurrentVolume)}: {Math.Round(Volume)}"
                            : _resourceToolkit.GetLocaleString(LanguageNames.Muted);
                        RequestShowTempMessage?.Invoke(this, msg);
                        _player.SetVolume(Volume);
                    });
                }
            }
        }

        private void OnViewVisibleBoundsChanged(ApplicationView sender, object args)
        {
            // 如果用户通过窗口按钮手动退出全屏状态，则播放器调整为默认模式.
            if (!sender.IsFullScreenMode && DisplayMode == PlayerDisplayMode.FullScreen)
            {
                DisplayMode = PlayerDisplayMode.Default;
            }
        }

        private void OnInteractionModuleNoMoreChoices(object sender, EventArgs e)
            => IsInteractionEnd = true;

        private async void OnSystemControlsButtonPressedAsync(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        _player?.Play();
                    });
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        _player?.Pause();
                    });
                    break;
                default:
                    break;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProgressSeconds))
            {
                ChangeProgressCommand.Execute(ProgressSeconds);
            }
        }

        private int GetFormatId(bool isLive = false)
        {
            var defaultPreferQuality = _appViewModel.IsXbox ? PreferQuality.HDFirst : PreferQuality.Auto;
            var preferQuality = _settingsToolkit.ReadLocalSetting(SettingNames.PreferQuality, defaultPreferQuality);
            var formatId = 0;
            if (preferQuality == PreferQuality.HDFirst)
            {
                var hdQuality = isLive ? 10000 : 116;
                formatId = Formats.Where(p => !p.IsLimited && p.Quality <= hdQuality).Max(p => p.Quality);
            }
            else if (preferQuality == PreferQuality.HighQuality)
            {
                formatId = Formats.Where(p => !p.IsLimited).Max(p => p.Quality);
            }

            if (formatId == 0)
            {
                var formatSetting = isLive ? SettingNames.DefaultLiveFormat : SettingNames.DefaultVideoFormat;
                var defaultFormat = isLive ? 400 : 64;
                formatId = _settingsToolkit.ReadLocalSetting(formatSetting, defaultFormat);
            }

            if (!Formats.Any(p => p.Quality == formatId))
            {
                formatId = Formats.Where(p => !p.IsLimited).Max(p => p.Quality);
            }

            return formatId;
        }
    }
}
