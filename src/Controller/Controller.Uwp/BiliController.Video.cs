// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
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
        public async Task<ViewReply> GetVideoDetailAsync(int videoId)
        {
            var result = await _videoProvider.GetVideoDetailAsync(videoId);
            return result;
        }
    }
}
