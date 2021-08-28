// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            var aid = GetAid();
            var result = await Controller.TripleAsync(aid);

            if (result != null)
            {
                IsLikeChecked = result.IsLike;
                IsCoinChecked = result.IsCoin;
                IsFavoriteChecked = result.IsFavorite;
            }
        }

        /// <summary>
        /// 点赞视频.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LikeAsync()
        {
            var isLike = !IsLikeChecked;
            var aid = GetAid();
            var isSuccess = await Controller.LikeVideoAsync(aid, isLike);

            if (isSuccess)
            {
                IsLikeChecked = !IsLikeChecked;
            }
        }

        /// <summary>
        /// 投币.
        /// </summary>
        /// <param name="number">投币个数.</param>
        /// <param name="isAlsoLike">是否同时点赞.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task CoinAsync(int number, bool isAlsoLike)
        {
            var aid = GetAid();
            var result = await Controller.CoinVideoAsync(aid, number, isAlsoLike);

            if (result != null)
            {
                IsCoinChecked = true;
                if (result.IsAlsoLike)
                {
                    IsLikeChecked = true;
                }
            }
        }

        /// <summary>
        /// 收藏视频.
        /// </summary>
        /// <param name="selectedIds">选中的收藏夹Id.</param>
        /// <param name="deselectedIds">取消选中的收藏夹Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task FavoriteAsync(List<string> selectedIds, List<string> deselectedIds)
        {
            var aid = GetAid();
            var result = await Controller.FavoriteVideoAsync(aid, selectedIds, deselectedIds);

            switch (result)
            {
                case Models.Enums.Bili.FavoriteResult.Success:
                case Models.Enums.Bili.FavoriteResult.InsufficientAccess:
                    IsFavoriteChecked = true;
                    break;
                default:
                    break;
            }
        }

        private long GetAid()
        {
            return IsPgc ? CurrentPgcEpisode.Aid : _videoId;
        }
    }
}
