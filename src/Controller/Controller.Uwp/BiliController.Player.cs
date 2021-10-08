// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;
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
        /// 获取在线观看人数.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="cid">分P Id.</param>
        /// <returns>观看人数.</returns>
        public async Task<string> GetOnlineViewerCountAsync(int videoId, int cid)
        {
            var result = await _playerProvider.GetOnlineViewerCountAsync(videoId, cid);
            return result;
        }

        /// <summary>
        /// 获取视频播放信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <returns>播放信息.</returns>
        public async Task<PlayerInformation> GetVideoPlayInformationAsync(long videoId, long partId)
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
        public async Task<PlayerInformation> GetPgcPlayInformationAsync(int partId, int seasonType)
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
                    if (episodeId > 0)
                    {
                        isSuccess = await _playerProvider.ReportProgressAsync(episodeId, seasonId, Convert.ToInt64(progress.TotalSeconds));
                    }
                    else if (videoId > 0)
                    {
                        isSuccess = await _playerProvider.ReportProgressAsync(videoId, partId, Convert.ToInt64(progress.TotalSeconds));
                    }
                }
                catch (Exception ex)
                {
                    _loggerModule.LogError(ex);
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
        public async Task<FavoriteResult> FavoriteVideoAsync(long videoId, List<int> addFavoriteListIds, List<int> deleteFavoriteListIds)
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

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="content">弹幕内容.</param>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <param name="progress">播放进度.</param>
        /// <param name="color">弹幕颜色.</param>
        /// <param name="isStandardSize">是否为标准字体大小.</param>
        /// <param name="location">弹幕位置.</param>
        /// <returns>是否发送成功.</returns>
        public Task<bool> SendDanmakuAsync(string content, int videoId, int partId, TimeSpan progress, string color, bool isStandardSize, DanmakuLocation location)
            => _playerProvider.SendDanmakuAsync(content, videoId, partId, Convert.ToInt32(progress.TotalMilliseconds), color, isStandardSize, location);

        /// <summary>
        /// 获取视频字幕索引.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <returns>字幕索引.</returns>
        public Task<SubtitleIndexResponse> GetSubtitleIndexAsync(int videoId, int partId)
            => _playerProvider.GetSubtitleIndexAsync(videoId, partId);

        /// <summary>
        /// 获取视频字幕详情.
        /// </summary>
        /// <param name="url">字幕地址.</param>
        /// <returns>字幕详情.</returns>
        public Task<SubtitleDetailResponse> GetSubtitleDetailAsync(string url)
            => _playerProvider.GetSubtitleDetailAsync(url);

        /// <summary>
        /// 获取互动视频选区.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="graphVersion">版本号.</param>
        /// <param name="edgeId">选区Id.</param>
        /// <returns>选区响应.</returns>
        public Task<InteractionEdgeResponse> GetInteractionEdgeAsync(long videoId, string graphVersion, long edgeId)
            => _playerProvider.GetInteractionEdgeAsync(videoId, graphVersion, edgeId);

        /// <summary>
        /// 获取视频的参数.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns>视频参数.</returns>
        public Task<VideoStatusInfo> GetVideoStatusAsync(long videoId)
            => _playerProvider.GetVideoStatusAsync(videoId);
    }
}
