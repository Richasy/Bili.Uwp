// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Live;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 直播数据适配器接口.
    /// </summary>
    public interface ILiveAdapter
    {
        /// <summary>
        /// 将关注的直播间 <see cref="LiveFeedRoom"/> 转换为直播间信息.
        /// </summary>
        /// <param name="room">关注的直播间.</param>
        /// <returns><see cref="LiveInformation"/>.</returns>
        LiveInformation ConvertToLiveInformation(LiveFeedRoom room);

        /// <summary>
        /// 从直播间卡片 <see cref="LiveRoomCard"/> 转换为直播间信息.
        /// </summary>
        /// <param name="card">直播间卡片.</param>
        /// <returns><see cref="LiveInformation"/>.</returns>
        LiveInformation ConvertToLiveInformation(LiveRoomCard card);

        /// <summary>
        /// 从直播搜索结果 <see cref="LiveSearchItem"/> 转换为直播间信息.
        /// </summary>
        /// <param name="item">直播搜索结果.</param>
        /// <returns><see cref="LiveInformation"/>.</returns>
        LiveInformation ConvertToLiveInformation(LiveSearchItem item);

        /// <summary>
        /// 将直播间详情 <see cref="LiveRoomDetail"/> 转换为直播间视图.
        /// </summary>
        /// <param name="detail">直播间详情.</param>
        /// <returns>直播间视图.</returns>
        LiveRoomView ConvertToLiveRoomView(LiveRoomDetail detail);

        /// <summary>
        /// 将直播首页数据流信息 <see cref="LiveFeedResponse"/> 转换为直播流视图.
        /// </summary>
        /// <param name="response">直播首页数据流信息.</param>
        /// <returns><see cref="LiveFeedView"/>.</returns>
        LiveFeedView ConvertToLiveFeedView(LiveFeedResponse response);
    }
}
