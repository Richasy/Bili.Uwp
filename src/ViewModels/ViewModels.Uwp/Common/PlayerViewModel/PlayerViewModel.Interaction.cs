// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        /// <summary>
        /// 回退跳转.
        /// </summary>
        /// <param name="seconds">跳转秒数.</param>
        public void BackSkip(double seconds)
        {
            if (seconds <= 0 || PlayerStatus == PlayerStatus.NotLoad || PlayerStatus == PlayerStatus.Buffering)
            {
                return;
            }

            try
            {
                var currentPos = _currentVideoPlayer.PlaybackSession.Position;
                if (currentPos.TotalSeconds > seconds)
                {
                    currentPos -= TimeSpan.FromSeconds(seconds);
                }
                else
                {
                    currentPos = TimeSpan.Zero;
                }

                _currentVideoPlayer.PlaybackSession.Position = currentPos;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 前进跳转.
        /// </summary>
        /// <param name="seconds">跳转秒数.</param>
        public void ForwardSkip(double seconds)
        {
            if (seconds <= 0 || PlayerStatus == PlayerStatus.NotLoad || PlayerStatus == PlayerStatus.Buffering)
            {
                return;
            }

            try
            {
                var duration = _currentVideoPlayer.PlaybackSession.NaturalDuration;
                var currentPos = _currentVideoPlayer.PlaybackSession.Position;
                if ((duration - currentPos).TotalSeconds > seconds)
                {
                    currentPos += TimeSpan.FromSeconds(seconds);
                }
                else
                {
                    currentPos = duration;
                }

                _currentVideoPlayer.PlaybackSession.Position = currentPos;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 一键三连.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task TripleAsync()
        {
            TripleResult result = null;
            if (_videoId > 0)
            {
                result = await Controller.TripleAsync(_videoId);
            }
            else if (IsPgc)
            {
                result = await Controller.TripleAsync(CurrentPgcEpisode.Aid);
            }

            if (result != null)
            {
                IsLikeChecked = result.IsLike;
                IsCoinChecked = result.IsCoin;
                IsFavoriteChecked = result.IsFavorite;
            }
        }
    }
}
