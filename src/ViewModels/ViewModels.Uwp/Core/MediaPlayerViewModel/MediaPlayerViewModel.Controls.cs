// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.Enums;

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

        private void EnsureMediaPlayerExist()
        {
            if (_mediaPlayer == null || _mediaPlayer.PlaybackSession == null)
            {
                throw new InvalidOperationException("此时媒体播放器尚未就绪");
            }
        }
    }
}
