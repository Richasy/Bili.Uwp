// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;
using Bili.Models.Data.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Microsoft.Graphics.Canvas;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 原生播放器视图模型.
    /// </summary>
    public sealed partial class NativePlayerViewModel : ViewModelBase, INativePlayerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NativePlayerViewModel"/> class.
        /// </summary>
        public NativePlayerViewModel(
            IFileToolkit fileToolkit,
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            CoreDispatcher dispatcher)
        {
            _fileToolkit = fileToolkit;
            _resourceToolkit = resourceToolkit;
            _settingsToolkit = settingsToolkit;
            _dispatcher = dispatcher;

            ClearCommand = ReactiveCommand.Create(Clear);
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
            if (_videoPlayer != null
                && _videoPlayer.PlaybackSession != null
                && _videoPlayer.PlaybackSession.CanPause)
            {
                _videoPlayer.Pause();
            }
        }

        /// <inheritdoc/>
        public void Play()
        {
            if (_videoPlayer != null
                && _videoPlayer.PlaybackSession != null)
            {
                _videoPlayer.Play();
            }
        }

        /// <inheritdoc/>
        public void SeekTo(TimeSpan time)
        {
            if (_videoPlayer != null
                && _videoPlayer.PlaybackSession != null
                && _videoPlayer.PlaybackSession.CanSeek)
            {
                _videoPlayer.PlaybackSession.Position = time;
            }
        }

        /// <inheritdoc/>
        public void SetPlayRate(double rate)
        {
            if (_videoPlayer != null
                && _videoPlayer.PlaybackSession != null)
            {
                _videoPlayer.PlaybackSession.PlaybackRate = rate;
            }
        }

        /// <inheritdoc/>
        public void SetVolume(double volume)
        {
            if (_videoPlayer != null)
            {
                if (volume > 100)
                {
                    volume = 100d;
                }

                _videoPlayer.Volume = volume / 100d;
            }
        }

        /// <inheritdoc/>
        public void SetLoop(bool isLoop)
        {
            if (_videoPlayer != null)
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
