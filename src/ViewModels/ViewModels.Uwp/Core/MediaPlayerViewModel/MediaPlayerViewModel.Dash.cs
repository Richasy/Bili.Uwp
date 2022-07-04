// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using FFmpegInteropX;
using Windows.Media.Playback;
using Windows.Web.Http;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private async Task GetDashVideoSourceAsync()
        {
            try
            {
                _videoFFSource?.Dispose();
                _audioFFSource?.Dispose();

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Referer = new Uri("https://www.bilibili.com");
                httpClient.DefaultRequestHeaders.Add("User-Agent", ServiceConstants.DefaultUserAgentString);

                var tasks = new List<Task>
                {
                    Task.Run(async () =>
                    {
                        var videoStream = await HttpRandomAccessStream.CreateAsync(httpClient, new Uri(_video.BaseUrl));
                        _videoFFSource = await FFmpegMediaSource.CreateFromStreamAsync(videoStream, _videoConfig);
                    }),
                    Task.Run(async () =>
                    {
                        var audioStream = await HttpRandomAccessStream.CreateAsync(httpClient, new Uri(_audio.BaseUrl));
                        _audioFFSource = await FFmpegMediaSource.CreateFromStreamAsync(audioStream, _videoConfig);
                    }),
                };

                await Task.WhenAll(tasks);
                _videoPlaybackItem = _videoFFSource.CreateMediaPlaybackItem();
                _audioPlaybackItem = _audioFFSource.CreateMediaPlaybackItem();

                _mediaTimelineController = GetTimelineController();

                _videoPlayer = GetVideoPlayer();
                _videoPlayer.CommandManager.IsEnabled = false;
                _videoPlayer.Source = _videoPlaybackItem;
                _videoPlayer.TimelineController = _mediaTimelineController;

                _audioPlayer = new MediaPlayer();
                _audioPlayer.CommandManager.IsEnabled = false;
                _audioPlayer.Source = _audioPlaybackItem;
                _audioPlayer.TimelineController = _mediaTimelineController;

                MediaPlayerChanged?.Invoke(this, _videoPlayer);
            }
            catch (Exception)
            {
                IsError = true;
                ErrorText = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestVideoFailed);
            }
        }

        private async Task GetDashLiveSourceAsync(string url)
        {
            try
            {
                _videoFFSource?.Dispose();
                _videoFFSource = await FFmpegMediaSource.CreateFromUriAsync(url, _liveConfig);
                _videoPlaybackItem = _videoFFSource.CreateMediaPlaybackItem();

                _videoPlayer = GetVideoPlayer();
                _videoPlayer.Source = _videoPlaybackItem;

                MediaPlayerChanged?.Invoke(this, _videoPlayer);
            }
            catch (Exception)
            {
                IsError = true;
                ErrorText = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestLivePlayInformationFailed);
            }
        }
    }
}
