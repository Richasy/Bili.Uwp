// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Windows.Media.Playback;
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
            Formats.Clear();
            PlaybackRates.Clear();
            IsShowProgressTip = false;
            IsShowInteractionProgress = false;
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
            InteractionProgressSeconds = 0;
            InteractionProgressText = "--";
            Volume = _settingsToolkit.ReadLocalSetting(SettingNames.Volume, 100d);
            _lastReportProgress = TimeSpan.Zero;
            _initializeProgress = TimeSpan.Zero;
            _interactionProgress = TimeSpan.Zero;
            _isInteractionProgressChanged = false;
            _originalPlayRate = 0;
            IsInteractionEnd = false;
            IsInteractionVideo = false;
            IsShowInteractionChoices = false;
            DanmakuViewModel.ResetCommand.Execute().Subscribe();
        }

        private void InitializeMediaPlayer()
        {
            var player = new MediaPlayer();
            player.MediaOpened += OnMediaPlayerOpened;
            player.CurrentStateChanged += OnMediaPlayerCurrentStateChangedAsync;
            player.MediaEnded += OnMediaPlayerEndedAsync;
            player.MediaFailed += OnMediaPlayerFailedAsync;
            player.AutoPlay = _settingsToolkit.ReadLocalSetting(SettingNames.IsAutoPlayWhenLoaded, true);
            player.IsLoopingEnabled = IsLoop;

            _mediaPlayer = player;
            MediaPlayerChanged?.Invoke(this, _mediaPlayer);
        }

        private void InitializePlaybackRates()
        {
            var isEnhancement = _settingsToolkit.ReadLocalSetting(SettingNames.PlaybackRateEnhancement, false);
            MaxPlaybackRate = isEnhancement ? 6d : 3d;
            PlaybackRateStep = isEnhancement ? 0.2 : 0.1;

            PlaybackRates.Clear();
            var defaultList = !isEnhancement
                ? new List<double> { 0.5, 0.75, 1.0, 1.25, 1.5, 2.0 }
                : new List<double> { 0.5, 1.0, 1.5, 2.0, 3.0, 4.0 };

            defaultList.ForEach(p => PlaybackRates.Add(new Common.PlaybackRateItemViewModel(
                p,
                p == PlaybackRate,
                rate => ChangePlayRateCommand.Execute(rate).Subscribe())));

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
            if (_mediaPlayer != null)
            {
                if (_mediaPlayer.PlaybackSession != null)
                {
                    _mediaPlayer.PlaybackSession.PositionChanged -= OnPlayerPositionChangedAsync;
                    if (_mediaPlayer.PlaybackSession.CanPause)
                    {
                        _mediaPlayer.Pause();
                    }
                }

                if (_playbackItem != null)
                {
                    _playbackItem.Source.Dispose();
                    _playbackItem = null;
                }

                _mediaPlayer.Source = null;
                _mediaPlayer = null;
            }

            _lastReportProgress = TimeSpan.Zero;
            _progressTimer?.Stop();
            _unitTimer?.Stop();

            if (_interopMSS != null)
            {
                _interopMSS.Dispose();
                _interopMSS = null;
            }

            Status = PlayerStatus.NotLoad;

            try
            {
                _displayRequest.RequestRelease();
            }
            catch (Exception)
            {
            }
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

        private void StartTimersAndDisplayRequest()
        {
            _progressTimer?.Start();
            _unitTimer?.Start();
            try
            {
                _displayRequest.RequestActive();
            }
            catch (Exception)
            {
            }
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
            if (_mediaPlayer != null && _mediaPlayer.PlaybackSession != null)
            {
                var progress = _mediaPlayer.PlaybackSession.Position;
                if (progress.TotalSeconds > 1)
                {
                    _initializeProgress = progress;
                }
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
            if (_mediaPlayer == null
                || _mediaPlayer.PlaybackSession == null
                || _accountViewModel.State != AuthorizeState.SignedIn)
            {
                return;
            }

            var progress = _mediaPlayer.PlaybackSession.Position;
            if (progress != _lastReportProgress)
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

        private async void OnMediaPlayerFailedAsync(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            if (args.ExtendedErrorCode?.HResult == -1072873851 || args.Error == MediaPlayerError.Unknown)
            {
                // 不处理 Shutdown 造成的错误.
                return;
            }

            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // 在视频未加载时不对报错进行处理.
                if (Status == PlayerStatus.NotLoad)
                {
                    return;
                }

                Status = PlayerStatus.End;
                IsError = true;
                var message = string.Empty;
                switch (args.Error)
                {
                    case MediaPlayerError.Aborted:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.Aborted);
                        break;
                    case MediaPlayerError.NetworkError:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.NetworkError);
                        break;
                    case MediaPlayerError.DecodingError:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.DecodingError);
                        break;
                    case MediaPlayerError.SourceNotSupported:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.SourceNotSupported);
                        break;
                    default:
                        break;
                }

                ErrorText = message;
                LogException(new Exception($"播放失败: {args.Error} | {args.ErrorMessage} | {args.ExtendedErrorCode}"));
            });
        }

        private async void OnMediaPlayerEndedAsync(MediaPlayer sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Status = PlayerStatus.End;
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

                MediaEnded?.Invoke(this, EventArgs.Empty);
            });
        }

        private async void OnMediaPlayerCurrentStateChangedAsync(MediaPlayer sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    IsBuffering = false;
                    IsMediaPause = false;
                    switch (sender.PlaybackSession.PlaybackState)
                    {
                        case MediaPlaybackState.None:
                            Status = PlayerStatus.End;
                            break;
                        case MediaPlaybackState.Opening:
                            IsError = false;
                            Status = PlayerStatus.Playing;
                            break;
                        case MediaPlaybackState.Playing:
                            Status = PlayerStatus.Playing;
                            IsError = false;
                            if (sender.PlaybackSession.Position < _initializeProgress)
                            {
                                sender.PlaybackSession.Position = _initializeProgress;
                                _initializeProgress = TimeSpan.Zero;
                            }

                            break;
                        case MediaPlaybackState.Buffering:
                            Status = PlayerStatus.Buffering;
                            IsBuffering = true;
                            break;
                        case MediaPlaybackState.Paused:
                            Status = PlayerStatus.Pause;
                            IsMediaPause = true;
                            break;
                        default:
                            Status = PlayerStatus.NotLoad;
                            break;
                    }
                }
                catch (Exception)
                {
                    Status = PlayerStatus.NotLoad;
                }
            });
        }

        private void OnMediaPlayerOpened(MediaPlayer sender, object args)
        {
            var session = sender.PlaybackSession;
            if (session != null)
            {
                session.PositionChanged -= OnPlayerPositionChangedAsync;

                if (_videoType != VideoType.Live)
                {
                    session.PositionChanged += OnPlayerPositionChangedAsync;
                }

                if (_videoType == VideoType.Live && _interopMSS != null)
                {
                    _interopMSS.PlaybackSession = session;
                }
                else if (_initializeProgress != TimeSpan.Zero)
                {
                    session.Position = _initializeProgress;
                    _initializeProgress = TimeSpan.Zero;
                }

                ChangePlayRateCommand.Execute(PlaybackRate).Subscribe();
                ChangeVolumeCommand.Execute(Volume).Subscribe();
            }
        }

        private async void OnPlayerPositionChangedAsync(MediaPlaybackSession sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                DurationSeconds = sender.NaturalDuration.TotalSeconds;
                ProgressSeconds = sender.Position.TotalSeconds;
                if (ProgressSeconds > DurationSeconds)
                {
                    _mediaPlayer.Pause();
                    return;
                }

                if (!IsShowInteractionProgress)
                {
                    InteractionProgressSeconds = ProgressSeconds;
                }

                DurationText = _numberToolkit.FormatDurationText(sender.NaturalDuration, sender.NaturalDuration.Hours > 0);
                ProgressText = _numberToolkit.FormatDurationText(sender.Position, sender.NaturalDuration.Hours > 0);

                if (SubtitleViewModel.HasSubtitles)
                {
                    SubtitleViewModel.SeekCommand.Execute(ProgressSeconds).Subscribe();
                }

                var segmentIndex = Convert.ToInt32(Math.Ceiling(ProgressSeconds / 360d));
                if (segmentIndex < 1)
                {
                    segmentIndex = 1;
                }

                DanmakuViewModel.LoadSegmentDanmakuCommand.Execute(segmentIndex).Subscribe();
                DanmakuViewModel.SeekCommand.Execute(ProgressSeconds).Subscribe();
            });
        }

        private void OnProgressTimerTick(object sender, object e)
            => ReportViewProgressCommand.Execute().Subscribe();

        private void OnUnitTimerTickAsync(object sender, object e)
        {
            if (_isInteractionProgressChanged)
            {
                _isInteractionProgressChanged = false;
                _mediaPlayer.PlaybackSession.Position = _interactionProgress;
                _interactionProgress = TimeSpan.Zero;
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
    }
}
