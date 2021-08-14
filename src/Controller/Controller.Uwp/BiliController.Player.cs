// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.Bili;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的视频处理部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 获取视频详情信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns><see cref="ViewReply"/>.</returns>
        public async Task<ViewReply> GetVideoDetailAsync(long videoId)
        {
            var result = await _playerProvider.GetVideoDetailAsync(videoId);
            return result;
        }

        /// <summary>
        /// 获取视频播放信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <returns>播放信息.</returns>
        public async Task<PlayerDashInformation> GetVideoPlayInformationAsync(long videoId, long partId)
        {
            var result = await _playerProvider.GetDashAsync(videoId, partId);
            return result;
        }

        /// <summary>
        /// 获取PGC播放信息.
        /// </summary>
        /// <param name="partId">分集Id.</param>
        /// <param name="seasonType">剧集类型.</param>
        /// <returns>播放信息.</returns>
        public async Task<PlayerDashInformation> GetPgcPlayInformationAsync(int partId, int seasonType)
        {
            var result = await _playerProvider.GetDashAsync(partId, seasonType);
            return result;
        }

        /// <summary>
        /// 获取弹幕元数据.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <returns>弹幕元数据.</returns>
        public async Task<DmViewReply> GetDanmakuMetaDataAsync(long videoId, long partId)
        {
            var result = await _playerProvider.GetDanmakuMetaDataAsync(videoId, partId);
            return result;
        }

        /// <summary>
        /// 请求新的分片弹幕.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <param name="segmentIndex">分片索引.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestNewSegmentDanmakuAsync(long videoId, long partId, int segmentIndex)
        {
            var result = await _playerProvider.GetSegmentDanmakuAsync(videoId, partId, segmentIndex);
            if (result.Elems.Count > 0)
            {
                var args = new SegmentDanmakuIterationEventArgs(result, videoId, partId);
                SegmentDanmakuIteration?.Invoke(this, args);
            }
        }

        /// <summary>
        /// 上报历史记录.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <param name="episodeId">分集Id.</param>
        /// <param name="seasonId">剧集Id.</param>
        /// <param name="progress">播放进度.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ReportHistoryAsync(long videoId, long partId, int episodeId, int seasonId, TimeSpan progress)
        {
            if (await _authorizeProvider.IsTokenValidAsync())
            {
                try
                {
                    var isSuccess = false;
                    if (videoId > 0)
                    {
                        isSuccess = await _playerProvider.ReportProgressAsync(videoId, partId, Convert.ToInt64(progress.TotalSeconds));
                    }
                    else if (episodeId > 0)
                    {
                        isSuccess = await _playerProvider.ReportProgressAsync(episodeId, seasonId, Convert.ToInt64(progress.TotalSeconds));
                    }

                    // Record.
                }
                catch (Exception)
                {
                    // Record.
                }
            }
        }

        /// <summary>
        /// 点赞视频.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="isLike">是否点赞.</param>
        /// <returns>结果.</returns>
        public async Task<bool> LikeVideoAsync(long videoId, bool isLike = true)
        {
            if (await _authorizeProvider.IsTokenValidAsync())
            {
                return await _playerProvider.LikeAsync(videoId, isLike);
            }

            return false;
        }

        /// <summary>
        /// 给视频投币.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="coinNumber">硬币数.</param>
        /// <param name="isAlsoLike">同时点赞视频.</param>
        /// <returns>投币结果.</returns>
        public async Task<CoinResult> CoinVideoAsync(long videoId, int coinNumber, bool isAlsoLike)
        {
            if (await _authorizeProvider.IsTokenValidAsync())
            {
                return await _playerProvider.CoinAsync(videoId, coinNumber, isAlsoLike);
            }

            return null;
        }

        /// <summary>
        /// 收藏视频.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="addFavoriteListIds">要添加到的收藏夹Id列表.</param>
        /// <param name="deleteFavoriteListIds">要移除的收藏夹Id列表.</param>
        /// <returns>结果.</returns>
        public async Task<FavoriteResult> FavoriteVideoAsync(long videoId, List<string> addFavoriteListIds, List<string> deleteFavoriteListIds)
        {
            if (await _authorizeProvider.IsTokenValidAsync())
            {
                return await _playerProvider.FavoriteAsync(videoId, addFavoriteListIds, deleteFavoriteListIds);
            }

            return FavoriteResult.NeedLogin;
        }

        /// <summary>
        /// 一键三连.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns>三连结果.</returns>
        public async Task<TripleResult> TripleAsync(long videoId)
        {
            if (await _authorizeProvider.IsTokenValidAsync())
            {
                return await _playerProvider.TripleAsync(videoId);
            }

            return null;
        }
    }
}
