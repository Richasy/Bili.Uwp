// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Microsoft.Graphics.Canvas;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private async Task PlayPauseAsync()
        {
            EnsureMediaPlayerExist();

            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (Status == PlayerStatus.Playing)
                {
                    _mediaPlayer.Pause();
                }
                else if (Status == PlayerStatus.Pause)
                {
                    _mediaPlayer.Play();
                }
                else if (Status == PlayerStatus.End)
                {
                    _mediaPlayer.PlaybackSession.Position = TimeSpan.Zero;
                    _mediaPlayer.Play();
                }
            });
        }

        private async Task ForwardSkipAsync()
        {
            EnsureMediaPlayerExist();

            var seconds = _settingsToolkit.ReadLocalSetting(SettingNames.SingleFastForwardAndRewindSpan, 30d);
            if (seconds <= 0
                || Status == PlayerStatus.NotLoad
                || Status == PlayerStatus.Buffering)
            {
                return;
            }

            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var duration = _mediaPlayer.PlaybackSession.NaturalDuration;
                var currentPos = _mediaPlayer.PlaybackSession.Position;
                if ((duration - currentPos).TotalSeconds > seconds)
                {
                    currentPos += TimeSpan.FromSeconds(seconds);
                }
                else
                {
                    currentPos = duration;
                }

                _mediaPlayer.PlaybackSession.Position = currentPos;
            });
        }

        private async Task ChangePlayRateAsync(double rate)
        {
            try
            {
                EnsureMediaPlayerExist();
            }
            catch (Exception)
            {
                return;
            }

            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (rate > MaxPlaybackRate)
                {
                    return;
                }

                if (PlaybackRate != rate)
                {
                    PlaybackRate = rate;
                }

                _mediaPlayer.PlaybackSession.PlaybackRate = PlaybackRate;

                foreach (var r in PlaybackRates)
                {
                    r.IsSelected = r.Data == PlaybackRate;
                }
            });
        }

        private void ChangeVolume(double volume)
        {
            try
            {
                EnsureMediaPlayerExist();
            }
            catch (Exception)
            {
                return;
            }

            if (volume > 100)
            {
                volume = 100;
            }
            else if (volume < 0)
            {
                volume = 0;
            }

            if (Volume != volume)
            {
                Volume = volume;
            }

            _mediaPlayer.Volume = volume / 100.0;
            _settingsToolkit.WriteLocalSetting(SettingNames.Volume, Volume);
        }

        private void ToggleFullScreenMode()
        {
            DisplayMode = DisplayMode != PlayerDisplayMode.FullScreen
                ? PlayerDisplayMode.FullScreen
                : PlayerDisplayMode.Default;
        }

        private void ToggleFullWindowMode()
        {
            DisplayMode = DisplayMode != PlayerDisplayMode.FullWindow
                ? PlayerDisplayMode.FullWindow
                : PlayerDisplayMode.Default;
        }

        private void ToggleCompactOverlayMode()
        {
            DisplayMode = DisplayMode != PlayerDisplayMode.CompactOverlay
                ? PlayerDisplayMode.CompactOverlay
                : PlayerDisplayMode.Default;
        }

        private async Task ScreenShotAsync()
        {
            EnsureMediaPlayerExist();
            var rendertarget = new CanvasRenderTarget(
                    CanvasDevice.GetSharedDevice(),
                    _mediaPlayer.PlaybackSession.NaturalVideoWidth,
                    _mediaPlayer.PlaybackSession.NaturalVideoHeight,
                    96);
            _mediaPlayer.CopyFrameToVideoSurface(rendertarget);

            var folder = await KnownFolders.PicturesLibrary.CreateFolderAsync(AppConstants.ScreenshotFolderName, CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(Guid.NewGuid().ToString("N") + ".png", CreationCollisionOption.OpenIfExists);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await rendertarget.SaveAsync(stream, CanvasBitmapFileFormat.Png);
            }

            _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.ScreenshotSuccess), Models.Enums.App.InfoType.Success);

            var shouldCopy = _settingsToolkit.ReadLocalSetting(SettingNames.CopyScreenshotAfterSave, true);
            if (shouldCopy)
            {
                var dataPackage = new DataPackage();
                dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));
                Clipboard.SetContent(dataPackage);
            }

            var shouldOpenFile = _settingsToolkit.ReadLocalSetting(SettingNames.OpenScreenshotAfterSave, false);
            if (shouldOpenFile)
            {
                await Launcher.LaunchFileAsync(file).AsTask();
            }
        }

        private void EnsureMediaPlayerExist()
        {
            if (_mediaPlayer == null || _mediaPlayer.PlaybackSession == null)
            {
                throw new InvalidOperationException("此时媒体播放器尚未就绪");
            }
        }
    }
}
