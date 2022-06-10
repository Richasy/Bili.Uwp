// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.BiliBili;
using Bili.Models.Enums.App;
using Bilibili.Community.Service.Dm.V1;

namespace Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的视频处理部分.
    /// </summary>
    public partial class BiliController
    {
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
                        isSuccess = await _playerProvider.ReportProgressAsync(videoId, partId, episodeId, seasonId, Convert.ToInt64(progress.TotalSeconds));
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
    }
}
