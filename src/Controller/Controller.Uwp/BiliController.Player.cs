// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;

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
    }
}
