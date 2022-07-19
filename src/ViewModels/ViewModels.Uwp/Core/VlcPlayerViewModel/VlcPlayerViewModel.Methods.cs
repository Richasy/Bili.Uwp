// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using LibVLCSharp.Shared;
using Windows.Web.Http;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// VLC 播放器视图模型.
    /// </summary>
    public sealed partial class VlcPlayerViewModel
    {
        private TimeSpan GetPosition()
        {
            if (_videoPlayer == null)
            {
                return TimeSpan.Zero;
            }

            var length = _videoPlayer.Length;
            if (length <= 0)
            {
                return TimeSpan.Zero;
            }

            var playedMs = length * _videoPlayer.Position;
            var ts = TimeSpan.FromMilliseconds(playedMs);
            return ts;
        }

        private TimeSpan GetDuration()
        {
            if (_videoPlayer == null)
            {
                return TimeSpan.Zero;
            }

            var length = _videoPlayer.Length;
            if (length <= 0)
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromMilliseconds(length);
        }

        private HttpClient GetVideoClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Referer = new Uri("https://www.bilibili.com");
            httpClient.DefaultRequestHeaders.Add("User-Agent", ServiceConstants.DefaultUserAgentString);
            return httpClient;
        }

        private async Task LoadDashVideoSourceAsync()
        {
            var hasAudio = _audio != null;
            var config = new LibVLC(_appViewModel.VlcOptions);
            var tasks = new List<Task>
                {
                    Task.Run(async () =>
                    {
                        var client = GetVideoClient();
                        _videoStream = await HttpRandomAccessStream.CreateAsync(client, new Uri(_video.BaseUrl));
                        _videoInput = new StreamMediaInput(_videoStream.AsStream());
                        _videoMedia = new Media(config, _videoInput);
                        _videoPlayer = GetVideoPlayer(_videoMedia);
                    }),
                    Task.Run(async () =>
                    {
                        if (hasAudio)
                        {
                            var client = GetVideoClient();
                            _audioStream = await HttpRandomAccessStream.CreateAsync(client, new Uri(_audio.BaseUrl));
                            _audioInput = new StreamMediaInput(_audioStream.AsStream());
                            _audioMedia = new Media(config, _audioInput);
                            _audioPlayer = GetVideoPlayer(_audioMedia);
                        }
                    }),
                };

            await Task.WhenAll(tasks);
            MediaPlayerChanged?.Invoke(this, _videoPlayer);
        }

        private async Task LoadDashLiveSourceAsync(string url)
        {
            try
            {
                var config = new LibVLC(_appViewModel.VlcOptions);
                var client = GetVideoClient();
                _videoStream = await HttpRandomAccessStream.CreateAsync(client, new Uri(url));
                _videoInput = new StreamMediaInput(_videoStream.AsStream());
                _videoMedia = new Media(config, _videoInput);
                _videoPlayer = GetVideoPlayer(_videoMedia);
                MediaPlayerChanged?.Invoke(this, _videoPlayer);
            }
            catch (Exception ex)
            {
                Status = PlayerStatus.Failed;
                StateChanged?.Invoke(this, new Models.App.Args.MediaStateChangedEventArgs(Status, ex.Message));
                LogException(ex);
            }
        }

        private MediaPlayer GetVideoPlayer(Media media)
        {
            var player = new MediaPlayer(media);
            player.Opening += OnMediaPlayerOpening;
            player.Buffering += OnBuffering;
            player.Paused += OnPaused;
            player.Playing += OnPlaying;
            player.EndReached += OnEndReached;
            player.PositionChanged += OnPositionChangedAsync;
            return player;
        }

        private async void OnPositionChangedAsync(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var duration = Duration;
                var progress = Position;
                if (progress > duration)
                {
                    Pause();
                    return;
                }

                PositionChanged?.Invoke(this, new Models.App.Args.MediaPositionChangedEventArgs(Position, Duration));
            });
        }

        private void OnEndReached(object sender, EventArgs e)
        {
            Status = PlayerStatus.End;
            StateChanged?.Invoke(this, new Models.App.Args.MediaStateChangedEventArgs(Status, string.Empty));
        }

        private void OnPlaying(object sender, EventArgs e)
        {
            Status = PlayerStatus.Playing;
            StateChanged?.Invoke(this, new Models.App.Args.MediaStateChangedEventArgs(Status, string.Empty));
        }

        private void OnPaused(object sender, EventArgs e)
        {
            Status = PlayerStatus.Pause;
            StateChanged?.Invoke(this, new Models.App.Args.MediaStateChangedEventArgs(Status, string.Empty));
        }

        private void OnBuffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            Status = PlayerStatus.Buffering;
            MediaOpened?.Invoke(this, EventArgs.Empty);
            StateChanged?.Invoke(this, new Models.App.Args.MediaStateChangedEventArgs(Status, string.Empty));
        }

        private void OnMediaPlayerOpening(object sender, EventArgs e)
        {
            Status = PlayerStatus.Buffering;
            StateChanged?.Invoke(this, new Models.App.Args.MediaStateChangedEventArgs(Status, string.Empty));
        }

        private void ClearMediaPlayerData(MediaPlayer mediaPlayer, MediaInput mediaInput, Media media, HttpRandomAccessStream stream)
        {
            if (mediaPlayer == null)
            {
                return;
            }

            mediaPlayer.Opening -= OnMediaPlayerOpening;
            mediaPlayer.Buffering -= OnBuffering;
            mediaPlayer.Paused -= OnPaused;
            mediaPlayer.Playing -= OnPlaying;
            mediaPlayer.EndReached -= OnEndReached;
            mediaPlayer.PositionChanged -= OnPositionChangedAsync;

            if (mediaInput != null)
            {
                mediaInput?.Dispose();
                mediaInput = null;
            }

            if (media != null)
            {
                media?.Dispose();
                media = null;
            }

            mediaPlayer?.Dispose();
            mediaPlayer = null;

            if (stream != null)
            {
                stream?.Dispose();
                stream = null;
            }
        }

        private void Clear()
        {
            ClearMediaPlayerData(_videoPlayer, _videoInput, _videoMedia, _videoStream);
            ClearMediaPlayerData(_audioPlayer, _audioInput, _audioMedia, _audioStream);

            Status = PlayerStatus.NotLoad;
        }
    }
}
