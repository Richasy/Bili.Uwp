// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.Bili;

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

        /// <summary>
        /// 获取弹幕元数据信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns>弹幕元数据响应结果.</returns>
        Task<DmViewReply> GetDanmakuMetaDataAsync(long videoId, long partId);

        /// <summary>
        /// 获取分段弹幕信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <param name="segmentIndex">分段索引，6分钟为一段.</param>
        /// <returns><see cref="DmSegMobileReply"/>.</returns>
        Task<DmSegMobileReply> GetSegmentDanmakuAsync(long videoId, long partId, int segmentIndex);

        /// <summary>
        /// 报告播放进度记录.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <param name="progress">播放进度.</param>
        /// <returns>进度上报是否成功.</returns>
        Task<bool> ReportProgressAsync(long videoId, long partId, long progress);

        /// <summary>
        /// 点赞视频.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="isLike">是否点赞.</param>
        /// <returns>结果.</returns>
        Task<bool> LikeAsync(long videoId, bool isLike);

        /// <summary>
        /// 投币.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="number">投币数量，上限为2.</param>
        /// <param name="alsoLike">是否同时点赞.</param>
        /// <returns>投币结果.</returns>
        Task<CoinResult> CoinAsync(long videoId, int number, bool alsoLike);

        /// <summary>
        /// 添加收藏.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="needAddFavoriteList">需要添加的收藏夹列表.</param>
        /// <param name="needRemoveFavoriteList">需要移除的收藏夹列表.</param>
        /// <returns>收藏结果.</returns>
        Task<FavoriteResult> FavoriteAsync(long videoId, IList<string> needAddFavoriteList, IList<string> needRemoveFavoriteList);

        /// <summary>
        /// 一键三连.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns>三连结果.</returns>
        Task<TripleResult> TripleAsync(long videoId);
    }
}
