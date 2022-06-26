﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
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

        private async Task BackwardSkipAsync()
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
                if (currentPos.TotalSeconds > seconds)
                {
                    currentPos -= TimeSpan.FromSeconds(seconds);
                }
                else
                {
                    currentPos = TimeSpan.Zero;
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

        private async Task ChangeVolumeAsync(double volume)
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

            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (Volume != volume)
                {
                    Volume = volume;
                }

                var msg = volume > 0
                    ? $"{_resourceToolkit.GetLocaleString(LanguageNames.CurrentVolume)}: {Math.Round(volume)}"
                    : _resourceToolkit.GetLocaleString(LanguageNames.Muted);

                RequestShowTempMessage?.Invoke(this, msg);

                _mediaPlayer.Volume = volume / 100.0;
                _settingsToolkit.WriteLocalSetting(SettingNames.Volume, Volume);
            });
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

        private void ChangeProgress(double seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            if (ts.TotalSeconds == _interactionProgress.TotalSeconds)
            {
                return;
            }

            InteractionProgressSeconds = seconds;
            _interactionProgress = ts;
            _isInteractionProgressChanged = true;

            var msg = $"{_resourceToolkit.GetLocaleString(LanguageNames.CurrentProgress)}: {TimeSpan.FromSeconds(seconds):g}";
            RequestShowTempMessage?.Invoke(this, msg);
        }

        private async Task StartTempQuickPlayAsync()
        {
            EnsureMediaPlayerExist();
            if (Status != PlayerStatus.Playing || PlaybackRate >= 3)
            {
                return;
            }

            _originalPlayRate = PlaybackRate;
            _originalDanmakuSpeed = DanmakuViewModel.DanmakuSpeed;
            await ChangePlayRateAsync(3);
            DanmakuViewModel.DanmakuSpeed = 2;
            var msg = _resourceToolkit.GetLocaleString(LanguageNames.StartQuickPlay);
            RequestShowTempMessage?.Invoke(this, msg);
        }

        private async Task StopTempQuickPlayAsync()
        {
            if (_originalPlayRate <= 0)
            {
                return;
            }

            DanmakuViewModel.DanmakuSpeed = _originalDanmakuSpeed;
            await ChangePlayRateAsync(_originalPlayRate);
            _originalPlayRate = 0;
            _originalDanmakuSpeed = 0;
            RequestShowTempMessage?.Invoke(this, default);
        }

        private void JumpToLastProgress()
        {
            if (_videoType == VideoType.Video)
            {
                var view = _viewData as VideoPlayerView;
                if (view.Progress != null)
                {
                    if (view.Progress.Identifier.Id != _currentPart.Id)
                    {
                        // 切换分P.
                        InternalPartChanged?.Invoke(this, view.Progress.Identifier);
                    }
                    else
                    {
                        ChangeProgress(view.Progress.Progress);
                        ResetProgressHistory();
                    }
                }

                IsShowProgressTip = false;
            }
            else if (_videoType == VideoType.Pgc)
            {
                var view = _viewData as PgcPlayerView;
                if (view.Progress != null)
                {
                    var episode = view.Progress.Identifier;
                    if (_currentEpisode.Identifier.Id != episode.Id)
                    {
                        // 切换分集.
                        InternalPartChanged?.Invoke(this, view.Progress.Identifier);
                    }
                    else
                    {
                        ChangeProgress(view.Progress.Progress);
                        ResetProgressHistory();
                    }
                }
            }
        }

        private void IncreasePlayRate()
        {
            if (_videoType == VideoType.Live)
            {
                return;
            }

            var rate = PlaybackRate + PlaybackRateStep;
            if (rate > MaxPlaybackRate)
            {
                rate = MaxPlaybackRate;
            }

            ChangePlayRateCommand.Execute(rate).Subscribe();
            RequestShowTempMessage?.Invoke(this, $"{_resourceToolkit.GetLocaleString(LanguageNames.CurrentPlaybackRate)}: {rate}x");
        }

        private void DecreasePlayRate()
        {
            if (_videoType == VideoType.Live)
            {
                return;
            }

            var rate = PlaybackRate - PlaybackRateStep;
            if (rate < 0.5)
            {
                rate = 0.5;
            }

            ChangePlayRateCommand.Execute(rate).Subscribe();
            RequestShowTempMessage?.Invoke(this, $"{_resourceToolkit.GetLocaleString(LanguageNames.CurrentPlaybackRate)}: {rate}x");
        }

        private void IncreaseVolume()
        {
            var volume = Volume + 5;
            if (volume > 100)
            {
                volume = 100;
            }

            ChangeVolumeCommand.Execute(volume).Subscribe();
        }

        private void DecreaseVolume()
        {
            var volume = Volume - 5;
            if (volume < 0)
            {
                volume = 0;
            }

            ChangeVolumeCommand.Execute(volume).Subscribe();
        }

        private void BackToDefaultMode()
        {
            if (DisplayMode == PlayerDisplayMode.FullWindow)
            {
                ToggleFullWindowCommand.Execute().Subscribe();
            }
            else if (DisplayMode == PlayerDisplayMode.FullScreen)
            {
                ToggleFullScreenCommand.Execute().Subscribe();
            }
            else if (DisplayMode == PlayerDisplayMode.CompactOverlay)
            {
                ToggleCompactOverlayCommand.Execute().Subscribe();
            }
        }
    }
}
