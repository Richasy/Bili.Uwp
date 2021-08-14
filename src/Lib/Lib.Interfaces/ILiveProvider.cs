// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供直播相关的操作.
    /// </summary>
    public interface ILiveProvider
    {
        /// <summary>
        /// 获取直播源列表.
        /// </summary>
        /// <param name="page">页码.</param>
        /// <returns><see cref="LiveFeedResponse"/>.</returns>
        Task<LiveFeedResponse> GetLiveFeedsAsync(int page);

        /// <summary>
        /// 获取直播间详情.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <returns><see cref="LiveRoomDetail"/>.</returns>
        Task<LiveRoomDetail> GetLiveRoomDetailAsync(int roomId);

        /// <summary>
        /// 获取直播间播放数据.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <returns>播放信息.</returns>
        Task<LivePlayInformation> GetLivePlayInformationAsync(int roomId);
    }
}
