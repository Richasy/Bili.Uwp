// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;

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
        /// 获取直播间分区.
        /// </summary>
        /// <returns><see cref="LiveAreaResponse"/>.</returns>
        Task<LiveAreaResponse> GetLiveAreaIndexAsync();

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
        /// <param name="quality">清晰度.</param>
        /// <returns>播放信息.</returns>
        Task<LivePlayInformation> GetLivePlayInformationAsync(int roomId, int quality);

        /// <summary>
        /// 进入直播间.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <returns>是否成功.</returns>
        Task<bool> EnterLiveRoomAsync(int roomId);

        /// <summary>
        /// 发送消息.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <param name="message">消息内容.</param>
        /// <param name="color">弹幕颜色.</param>
        /// <param name="isStandardSize">是否为标准字体大小.</param>
        /// <param name="location">弹幕位置.</param>
        /// <returns>是否发送成功.</returns>
        Task<bool> SendMessageAsync(int roomId, string message, string color, bool isStandardSize, DanmakuLocation location);
    }
}
