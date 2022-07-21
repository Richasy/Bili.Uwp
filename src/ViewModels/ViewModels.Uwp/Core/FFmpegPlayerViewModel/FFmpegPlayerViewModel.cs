// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;
using Bili.Models.Data.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using FFmpegInteropX;
using Microsoft.Graphics.Canvas;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 使用 FFmpeg 的播放器视图模型.
    /// </summary>
    public sealed partial class FFmpegPlayerViewModel : ViewModelBase, IFFmpegPlayerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FFmpegPlayerViewModel"/> class.
        /// </summary>
        public FFmpegPlayerViewModel(
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            CoreDispatcher dispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _settingsToolkit = settingsToolkit;
            _dispatcher = dispatcher;

            _liveConfig = new MediaSourceConfig();
            _liveConfig.FFmpegOptions.Add("referer", "https://live.bilibili.com/");
            _liveConfig.FFmpegOptions.Add("user-agent", "Mozilla/5.0 BiliDroid/1.12.0 (bbcallen@gmail.com)");

            ClearCommand = ReactiveCommand.Create(Clear);

            _videoConfig = new MediaSourceConfig();
        }

        /// <inheritdoc/>
        public async Task SetSourceAsync(SegmentInformation video, SegmentInformation audio)
        {
            _video = video;
            _audio = audio;
            await LoadDashVideoSourceAsync();
        }

        /// <inheritdoc/>
        public async Task SetSourceAsync(string url)
        {
            _video = null;
            _audio = null;
            await LoadDashLiveSourceAsync(url);
        }

        /// <inheritdoc/>
        public void Pause()
        {
            if (_mediaTimelineController != null)
            {
                _mediaTimelineController.Pause();
            }
            else
            {
                if (_audioPlayer != null
                    && _audioPlayer.PlaybackSession != null
                    && _audioPlayer.PlaybackSession.CanPause)
                {
                    _audioPlayer.Pause();
                }
            }
        }

        /// <inheritdoc/>
        public void Play()
        {
            if (_mediaTimelineController != null)
            {
                _mediaTimelineController.Resume();
            }
            else
            {
                if (_videoPlayer != null
                    && _videoPlayer.PlaybackSession != null)
                {
                    _videoPlayer.Play();
                }

                if (_audioPlayer != null
                    && _audioPlayer.PlaybackSession != null)
                {
                    _audioPlayer.Play();
                }
            }
        }

        /// <inheritdoc/>
        public void SeekTo(TimeSpan time)
        {
            if (_mediaTimelineController != null)
            {
                _mediaTimelineController.Position = time;
            }
            else
            {
                if (_videoPlayer != null
                    && _videoPlayer.PlaybackSession != null
                    && _videoPlayer.PlaybackSession.CanSeek)
                {
                    _videoPlayer.PlaybackSession.Position = time;
                }

                if (_audioPlayer != null
                    && _audioPlayer.PlaybackSession != null
                    && _audioPlayer.PlaybackSession.CanSeek)
                {
                    _audioPlayer.PlaybackSession.Position = time;
                }
            }
        }

        /// <inheritdoc/>
        public void SetPlayRate(double rate)
        {
            if (_mediaTimelineController != null)
            {
                _mediaTimelineController.ClockRate = rate;
            }
            else
            {
                if (_videoPlayer != null
                && _videoPlayer.PlaybackSession != null)
                {
                    _videoPlayer.PlaybackSession.PlaybackRate = rate;
                }

                if (_audioPlayer != null && _audioPlayer.PlaybackSession != null)
                {
                    _audioPlayer.PlaybackSession.PlaybackRate = rate;
                }
            }
        }

        /// <inheritdoc/>
        public void SetVolume(double volume)
        {
            var v = volume > 100 ? 100d : volume;
            v = v / 100d;
            if (_videoPlayer != null)
            {
                _videoPlayer.Volume = v;
            }

            if (_audioPlayer != null)
            {
                _audioPlayer.Volume = v;
            }
        }

        /// <inheritdoc/>
        public void SetLoop(bool isLoop)
        {
            if (_mediaTimelineController != null)
            {
                _mediaTimelineController.IsLoopingEnabled = isLoop;
            }
            else if (_videoPlayer != null)
            {
                _videoPlayer.IsLoopingEnabled = isLoop;
            }
        }

        /// <inheritdoc/>
        public async Task ScreenshotAsync(Stream targetFileStream)
        {
            var rendertarget = new CanvasRenderTarget(
                    CanvasDevice.GetSharedDevice(),
                    _videoPlayer.PlaybackSession.NaturalVideoWidth,
                    _videoPlayer.PlaybackSession.NaturalVideoHeight,
                    96);
            _videoPlayer.CopyFrameToVideoSurface(rendertarget);
            using (targetFileStream)
            {
                var stream = targetFileStream.AsRandomAccessStream();
                await rendertarget.SaveAsync(stream, CanvasBitmapFileFormat.Png);
            }
        }

        /// <inheritdoc/>
        public void SetDisplayProperties(string cover, string title, string subtitle, string videoType)
        {
            if (_videoPlaybackItem == null)
            {
                return;
            }

            var props = _videoPlaybackItem.GetDisplayProperties();
            props.Type = Windows.Media.MediaPlaybackType.Video;
            props.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(cover));
            props.VideoProperties.Title = title;
            props.VideoProperties.Subtitle = subtitle;
            props.VideoProperties.Genres.Add(videoType);
            _videoPlaybackItem.ApplyDisplayProperties(props);
        }
    }
}
