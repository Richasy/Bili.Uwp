// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.BiliBili;
using Bili.Models.Data.Live;
using Bili.Models.Enums.App;

namespace Bili.Lib.Interfaces
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
        Task<LiveFeedView> GetLiveFeedsAsync();

        /// <summary>
        /// 获取直播间分区.
        /// </summary>
        /// <returns><see cref="LiveAreaResponse"/>.</returns>
        Task<IEnumerable<Models.Data.Community.Partition>> GetLiveAreaIndexAsync();

        /// <summary>
        /// 获取直播分区详情.
        /// </summary>
        /// <param name="areaId">分区Id.</param>
        /// <param name="parentId">父分区Id.</param>
        /// <param name="sortType">排序方式.</param>
        /// <returns><see cref="LivePartitionView"/>.</returns>
        Task<LivePartitionView> GetLiveAreaDetailAsync(string areaId, string parentId, string sortType);

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
        /// 获取直播间播放数据.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <param name="quality">清晰度.</param>
        /// <param name="audioOnly">是否仅音频.</param>
        /// <returns>播放信息.</returns>
        Task<LiveAppPlayUrlInfo> GetAppLivePlayInformation(int roomId, int quality, bool audioOnly);

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

        /// <summary>
        /// 重置推荐信息流.
        /// </summary>
        void ResetFeedState();

        /// <summary>
        /// 重置分区详情的状态信息.
        /// </summary>
        void ResetPartitionDetailState();
    }
}
