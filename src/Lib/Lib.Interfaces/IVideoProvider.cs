// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.App.Playurl.V1;
using Bilibili.App.View.V1;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供视频数据操作.
    /// </summary>
    public interface IVideoProvider
    {
        /// <summary>
        /// 获取视频详细信息，包括分P内容.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns><see cref="ViewReply"/>.</returns>
        Task<ViewReply> GetVideoDetailAsync(int videoId);

        /// <summary>
        /// 获取同时在线观看人数.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns>同时在线观看人数.</returns>
        Task<string> GetOnlineViewerCountAsync(int videoId, int partId);

        /// <summary>
        /// 获取播放地址.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns><see cref="PlayURLReply"/>.</returns>
        Task<PlayURLReply> GetPlayUrlAsync(int videoId, int partId);
    }
}
