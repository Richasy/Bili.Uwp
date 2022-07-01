// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using FFmpegInteropX;
using Windows.Media.Playback;

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

                _videoFFSource = await FFmpegMediaSource.CreateFromUriAsync(_video.BaseUrl, _videoConfig);
                _audioFFSource = await FFmpegMediaSource.CreateFromUriAsync(_audio.BaseUrl, _videoConfig);

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

                _mediaTimelineController = GetTimelineController();

                _videoPlayer = GetVideoPlayer();
                _videoPlayer.CommandManager.IsEnabled = false;
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
