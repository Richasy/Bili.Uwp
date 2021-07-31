// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.App.Playurl.V1;
using Bilibili.App.View.V1;

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
            var result = await _videoProvider.GetVideoDetailAsync(videoId);
            return result;
        }

        /// <summary>
        /// 获取视频播放信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <returns>播放信息.</returns>
        public async Task<PlayViewReply> GetVideoPlayInformationAsync(long videoId, long partId)
        {
            var result = await _videoProvider.GetPlayViewAsync(videoId, partId);
            return result;
        }
    }
}
