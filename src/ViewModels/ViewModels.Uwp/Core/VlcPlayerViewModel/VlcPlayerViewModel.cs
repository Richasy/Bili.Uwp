// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;
using Bili.Models.Data.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using ReactiveUI;
using Windows.Storage;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// VLC 播放器视图模型.
    /// </summary>
    public sealed partial class VlcPlayerViewModel : ViewModelBase, IVlcPlayerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VlcPlayerViewModel"/> class.
        /// </summary>
        public VlcPlayerViewModel(
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            AppViewModel appViewModel,
            CoreDispatcher dispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _settingsToolkit = settingsToolkit;
            _appViewModel = appViewModel;
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
            if (_videoPlayer != null && _videoPlayer.CanPause)
            {
                _videoPlayer.Pause();
            }

            if (_audioPlayer != null && _audioPlayer.CanPause)
            {
                _audioPlayer.Pause();
            }
        }

        /// <inheritdoc/>
        public void Play()
        {
            if (_videoPlayer != null
                && _videoPlayer.WillPlay)
            {
                _videoPlayer.Play();
            }

            if (_audioPlayer != null
                && _audioPlayer.WillPlay)
            {
                _audioPlayer.Play();
            }
        }

        /// <inheritdoc/>
        public void SeekTo(TimeSpan time)
        {
            if (_videoPlayer != null
                && _videoPlayer.IsSeekable)
            {
                _videoPlayer.SeekTo(time);
            }

            if (_audioPlayer != null
                && _audioPlayer.IsSeekable)
            {
                _audioPlayer.SeekTo(time);
            }
        }

        /// <inheritdoc/>
        public void SetPlayRate(double rate)
        {
            if (_videoPlayer != null)
            {
                _videoPlayer.SetRate((float)rate);
            }

            if (_audioPlayer != null)
            {
                _audioPlayer.SetRate((float)rate);
            }
        }

        /// <inheritdoc/>
        public void SetVolume(double volume)
        {
            var v = volume > 100 ? 100 : Convert.ToInt32(volume);
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
            // VLC 暂不设置循环播放.
        }

        /// <inheritdoc/>
        public async Task ScreenshotAsync(Stream targetFileStream)
        {
            var tempFolder = ApplicationData.Current.TemporaryFolder;
            var tempFile = await tempFolder.CreateFileAsync("temp.png", CreationCollisionOption.ReplaceExisting);
            uint width = 0;
            uint height = 0;
            _videoPlayer.Size(0, ref width, ref height);
            await Task.Run(() =>
            {
                _videoPlayer.TakeSnapshot(0, tempFile.Path, width, height);
            });
            using (targetFileStream)
            using (var tempFileStream = await tempFile.OpenStreamForReadAsync())
            {
                await tempFileStream.CopyToAsync(targetFileStream);
            }
        }

        /// <inheritdoc/>
        public void SetDisplayProperties(string cover, string title, string subtitle, string videoType)
        {
            // 暂不处理.
        }
    }
}
