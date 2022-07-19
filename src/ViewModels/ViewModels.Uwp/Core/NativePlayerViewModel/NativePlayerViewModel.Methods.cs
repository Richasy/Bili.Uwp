// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.Streaming.Adaptive;
using Windows.Web.Http;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 原生播放器视图模型.
    /// </summary>
    public sealed partial class NativePlayerViewModel
    {
        private HttpClient GetVideoClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Referer = new Uri("https://www.bilibili.com");
            httpClient.DefaultRequestHeaders.Add("User-Agent", ServiceConstants.DefaultUserAgentString);
            return httpClient;
        }

        private async Task LoadDashVideoSourceAsync()
        {
            var httpClient = GetVideoClient();
            var mpdFilePath = _audio == null
                    ? AppConstants.DashVideoWithoudAudioMPDFile
                    : AppConstants.DashVideoMPDFile;
            var mpdStr = await _fileToolkit.ReadPackageFile(mpdFilePath);

            var videoStr =
                    $@"<Representation bandwidth=""{_video.Bandwidth}"" codecs=""{_video.Codecs}"" height=""{_video.Height}"" mimeType=""{_video.MimeType}"" id=""{_video.Id}"" width=""{_video.Width}"">
                               <BaseURL></BaseURL>
                               <SegmentBase indexRange=""{_video.IndexRange}"">
                                   <Initialization range=""{_video.Initialization}"" />
                               </SegmentBase>
                           </Representation>";

            var audioStr = string.Empty;

            if (_audio != null)
            {
                audioStr =
                        $@"<Representation bandwidth=""{_audio.Bandwidth}"" codecs=""{_audio.Codecs}"" mimeType=""{_audio.MimeType}"" id=""{_audio.Id}"">
                               <BaseURL></BaseURL>
                               <SegmentBase indexRange=""{_audio.IndexRange}"">
                                   <Initialization range=""{_audio.Initialization}"" />
                               </SegmentBase>
                           </Representation>";
            }

            videoStr = videoStr.Trim();
            audioStr = audioStr.Trim();
            mpdStr = mpdStr.Replace("{video}", videoStr)
                             .Replace("{audio}", audioStr)
                             .Replace("{bufferTime}", $"PT4S");

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(mpdStr)).AsInputStream();
            var source = await AdaptiveMediaSource.CreateFromStreamAsync(stream, new Uri(_video.BaseUrl), "application/dash+xml", httpClient);
            Debug.Assert(source.Status == AdaptiveMediaSourceCreationStatus.Success, "解析MPD失败");
            source.MediaSource.DownloadRequested += (sender, args) =>
            {
                if (args.ResourceContentType == "audio/mp4" && _audio != null)
                {
                    args.Result.ResourceUri = new Uri(_audio.BaseUrl);
                }
            };

            var mediaSource = MediaSource.CreateFromAdaptiveMediaSource(source.MediaSource);
            _videoPlaybackItem = new MediaPlaybackItem(mediaSource);
            _videoPlayer = GetVideoPlayer();
            _videoPlayer.Source = _videoPlaybackItem;
            MediaPlayerChanged?.Invoke(this, _videoPlayer);
        }

        private async Task LoadDashLiveSourceAsync(string url)
        {
            try
            {
                var httpClient = GetVideoClient();
                var stream = await HttpRandomAccessStream.CreateAsync(httpClient, new Uri(url));
                var source = await AdaptiveMediaSource.CreateFromStreamAsync(stream.GetInputStreamAt(0), new Uri(url), stream.ContentType, httpClient);
                var mediaSource = MediaSource.CreateFromAdaptiveMediaSource(source.MediaSource);
                _videoPlaybackItem = new MediaPlaybackItem(mediaSource);
                _videoPlayer = GetVideoPlayer();
                _videoPlayer.Source = _videoPlaybackItem;
                MediaPlayerChanged?.Invoke(this, _videoPlayer);
            }
            catch (Exception ex)
            {
                Status = PlayerStatus.Failed;
                StateChanged?.Invoke(this, new MediaStateChangedEventArgs(Status, ex.Message));
                LogException(ex);
            }
        }

        private MediaPlayer GetVideoPlayer()
        {
            var player = new MediaPlayer();
            player.MediaOpened += OnMediaPlayerOpened;
            player.CurrentStateChanged += OnMediaPlayerCurrentStateChangedAsync;
            player.MediaEnded += OnMediaPlayerEndedAsync;
            player.MediaFailed += OnMediaPlayerFailedAsync;
            return player;
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

                Status = PlayerStatus.Failed;
                var arg = new MediaStateChangedEventArgs(Status, args.ErrorMessage);
                StateChanged?.Invoke(this, arg);
                LogException(new Exception($"播放失败: {args.Error} | {args.ErrorMessage} | {args.ExtendedErrorCode}"));
            });
        }

        private async void OnMediaPlayerEndedAsync(MediaPlayer sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Status = PlayerStatus.End;
                StateChanged?.Invoke(this, new MediaStateChangedEventArgs(Status, string.Empty));
            });
        }

        private async void OnMediaPlayerCurrentStateChangedAsync(MediaPlayer sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    Status = sender.PlaybackSession.PlaybackState switch
                    {
                        MediaPlaybackState.Opening => PlayerStatus.Buffering,
                        MediaPlaybackState.Playing => PlayerStatus.Playing,
                        MediaPlaybackState.Buffering => PlayerStatus.Buffering,
                        MediaPlaybackState.Paused => PlayerStatus.Pause,
                        _ => PlayerStatus.NotLoad,
                    };
                }
                catch (Exception)
                {
                    Status = PlayerStatus.NotLoad;
                }

                StateChanged?.Invoke(this, new MediaStateChangedEventArgs(Status, string.Empty));
            });
        }

        private void OnMediaPlayerOpened(MediaPlayer sender, object args)
        {
            var session = sender.PlaybackSession;
            if (session != null)
            {
                session.PositionChanged -= OnPlayerPositionChangedAsync;

                if (_video != null)
                {
                    session.PositionChanged += OnPlayerPositionChangedAsync;
                }

                var autoPlay = _settingsToolkit.ReadLocalSetting(SettingNames.IsAutoPlayWhenLoaded, true);
                if (autoPlay)
                {
                    _videoPlayer.AutoPlay = true;
                    _videoPlayer.Play();
                }
            }

            MediaOpened?.Invoke(this, EventArgs.Empty);
        }

        private async void OnPlayerPositionChangedAsync(MediaPlaybackSession sender, object args)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var duration = sender.NaturalDuration.TotalSeconds;
                var progress = sender.Position.TotalSeconds;
                if (progress > duration)
                {
                    _videoPlayer.Pause();
                    return;
                }

                PositionChanged?.Invoke(this, new MediaPositionChangedEventArgs(sender.Position, sender.NaturalDuration));
            });
        }

        private void Clear()
        {
            if (_videoPlayer == null)
            {
                return;
            }

            if (_videoPlayer.PlaybackSession != null)
            {
                _videoPlayer.PlaybackSession.PositionChanged -= OnPlayerPositionChangedAsync;
            }

            if (_videoPlaybackItem != null)
            {
                _videoPlaybackItem.Source?.Dispose();
                _videoPlaybackItem = null;
            }

            _videoPlayer.Source = null;
            _videoPlayer = null;

            if (_liveStream != null)
            {
                _liveStream?.Dispose();
                _liveStream = null;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Clear();
                }

                _disposedValue = true;
            }
        }
    }
}
