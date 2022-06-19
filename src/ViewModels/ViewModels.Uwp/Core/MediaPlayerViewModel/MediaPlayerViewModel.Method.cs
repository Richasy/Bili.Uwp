// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        }

        private void InitializeMediaPlayer()
        {
            var player = new MediaPlayer();
            player.MediaOpened += OnMediaPlayerOpened;
            player.CurrentStateChanged += OnMediaPlayerCurrentStateChangedAsync;
            player.MediaEnded += OnMediaPlayerEndedAsync;
            player.MediaFailed += OnMediaPlayerFailedAsync;
            player.AutoPlay = _settingsToolkit.ReadLocalSetting(SettingNames.IsAutoPlayWhenLoaded, true);
            player.Volume = Volume;
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
            _subtitleTimer?.Stop();

            if (_interopMSS != null)
            {
                _interopMSS.Dispose();
                _interopMSS = null;
            }

            Status = PlayerStatus.NotLoad;
        }

        private void InitializeTimer()
        {
            if (_progressTimer == null)
            {
                _progressTimer = new DispatcherTimer();
                _progressTimer.Interval = TimeSpan.FromSeconds(5);
                _progressTimer.Tick += OnProgressTimerTickAsync;
            }

            if (_subtitleTimer == null)
            {
                _subtitleTimer = new DispatcherTimer();
                _subtitleTimer.Interval = TimeSpan.FromSeconds(0.5);
                _subtitleTimer.Tick += OnSubtitleTimerTickAsync;
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

        private Tuple<string, string> GetProxyAndArea(string title, bool isVideo)
        {
            var proxy = string.Empty;
            var area = string.Empty;

            var isOpenRoaming = _settingsToolkit.ReadLocalSetting(SettingNames.IsOpenRoaming, false);
            var localProxy = isVideo
                ? _settingsToolkit.ReadLocalSetting(SettingNames.RoamingVideoAddress, string.Empty)
                : _settingsToolkit.ReadLocalSetting(SettingNames.RoamingViewAddress, string.Empty);
            if (isOpenRoaming && !string.IsNullOrEmpty(localProxy))
            {
                if (!string.IsNullOrEmpty(title))
                {
                    if (Regex.IsMatch(title, @"僅.*港.*地區"))
                    {
                        area = "hk";
                    }
                    else if (Regex.IsMatch(title, @"僅.*台.*地區"))
                    {
                        area = "tw";
                    }
                }

                var isForceProxy = _settingsToolkit.ReadLocalSetting(SettingNames.IsGlobeProxy, false);
                if ((isForceProxy && string.IsNullOrEmpty(area))
                    || !string.IsNullOrEmpty(area))
                {
                    proxy = localProxy;
                }
            }

            return new Tuple<string, string>(proxy, area);
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
            });
        }

        private async void OnMediaPlayerCurrentStateChangedAsync(MediaPlayer sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
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
                            break;
                        case MediaPlaybackState.Paused:
                            Status = PlayerStatus.Pause;
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

                session.PlaybackRate = PlaybackRate;
            }
        }

        private async void OnPlayerPositionChangedAsync(MediaPlaybackSession sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                DurationSeconds = sender.NaturalDuration.TotalSeconds;
                ProgressSeconds = sender.Position.TotalSeconds;
                DurationText = _numberToolkit.FormatDurationText(sender.NaturalDuration, sender.NaturalDuration.Hours > 0);
                ProgressText = _numberToolkit.FormatDurationText(sender.Position, sender.NaturalDuration.Hours > 0);
            });
        }

        private void OnSubtitleTimerTickAsync(object sender, object e) => throw new NotImplementedException();

        private void OnProgressTimerTickAsync(object sender, object e) => throw new NotImplementedException();

        private void OnViewVisibleBoundsChanged(ApplicationView sender, object args)
        {
            // 如果用户通过窗口按钮手动退出全屏状态，则播放器调整为默认模式.
            if (!sender.IsFullScreenMode && DisplayMode == PlayerDisplayMode.FullScreen)
            {
                DisplayMode = PlayerDisplayMode.Default;
            }
        }
    }
}
