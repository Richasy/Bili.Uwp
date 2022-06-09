// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;
using Windows.Media.Playback;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private void InitializeMediaPlayer()
        {
            var player = new MediaPlayer();
            player.MediaOpened += OnMediaPlayerOpened;
            player.CurrentStateChanged += OnMediaPlayerCurrentStateChangedAsync;
            player.MediaEnded += OnMediaPlayerEndedAsync;
            player.MediaFailed += OnMediaPlayerFailedAsync;
            player.AutoPlay = _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.IsAutoPlayWhenLoaded, true);
            player.Volume = _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.Volume, 100d);
            player.VolumeChanged += OnMediaPlayerVolumeChangedAsync;
            player.IsLoopingEnabled = IsLoop;

            _mediaPlayer = player;
        }

        private async void OnMediaPlayerVolumeChangedAsync(MediaPlayer sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Volume = sender.Volume;
            });
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

        private void OnMediaPlayerEndedAsync(MediaPlayer sender, object args) => throw new NotImplementedException();

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
    }
}
