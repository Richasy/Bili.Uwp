// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供视频数据操作.
    /// </summary>
    public interface IPlayerProvider
    {
        /// <summary>
        /// 获取视频详细信息，包括分P内容.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns><see cref="ViewReply"/>.</returns>
        Task<ViewReply> GetVideoDetailAsync(long videoId);

        /// <summary>
        /// 获取同时在线观看人数.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns>同时在线观看人数.</returns>
        Task<string> GetOnlineViewerCountAsync(long videoId, long partId);

        /// <summary>
        /// 获取Dash播放信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns><see cref="PlayerDashInformation"/>.</returns>
        Task<PlayerDashInformation> GetDashAsync(long videoId, long partId);
    }
}
