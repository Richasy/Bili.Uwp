// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.Data.Community;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Bili;

namespace Bili.Lib.Interfaces
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
        /// <returns><see cref="VideoPlayerView"/>.</returns>
        Task<VideoPlayerView> GetVideoDetailAsync(string videoId);

        /// <summary>
        /// 获取PGC内容的详细信息.
        /// </summary>
        /// <param name="episodeId">(可选项) 单集Id.</param>
        /// <param name="seasonId">(可选项) 剧集/系列Id.</param>
        /// <param name="proxy">代理地址.</param>
        /// <param name="area">地区.</param>
        /// <returns>PGC内容详情.</returns>
        Task<PgcPlayerView> GetPgcDetailAsync(string episodeId, string seasonId, string proxy = default, string area = default);

        /// <summary>
        /// 获取分集的交互信息，包括用户的投币/点赞/收藏.
        /// </summary>
        /// <param name="episodeId">分集Id.</param>
        /// <returns>交互信息.</returns>
        Task<EpisodeInteractionInformation> GetEpisodeInteractionInformationAsync(string episodeId);

        /// <summary>
        /// 获取同时在线观看人数.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns>同时在线观看人数.</returns>
        Task<string> GetOnlineViewerCountAsync(string videoId, string partId);

        /// <summary>
        /// 获取Dash播放信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns><see cref="MediaInformation"/>.</returns>
        Task<MediaInformation> GetVideoMediaInformationAsync(string videoId, string partId);

        /// <summary>
        /// 获取PGC的剧集Dash播放信息.
        /// </summary>
        /// <param name="partId">对应剧集的Cid.</param>
        /// <param name="episodeId">对应分集Id.</param>
        /// <param name="seasonType">剧集类型.</param>
        /// <param name="proxy">代理地址.</param>
        /// <param name="area">地区.</param>
        /// <returns><see cref="MediaInformation"/>.</returns>
        Task<MediaInformation> GetPgcMediaInformationAsync(string partId, string episodeId, string seasonType, string proxy = default, string area = default);

        /// <summary>
        /// 获取弹幕元数据信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <returns>弹幕元数据响应结果.</returns>
         // Task<DmViewReply> GetDanmakuMetaDataAsync(long videoId, long partId);

        /// <summary>
        /// 获取分段弹幕信息.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <param name="segmentIndex">分段索引，6分钟为一段.</param>
        /// <returns>分段弹幕信息.</returns>
        Task<IEnumerable<DanmakuInformation>> GetSegmentDanmakuAsync(string videoId, string partId, int segmentIndex);

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="content">弹幕内容.</param>
        /// <param name="videoId">视频 Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <param name="progress">播放进度.</param>
        /// <param name="color">弹幕颜色.</param>
        /// <param name="isStandardSize">是否为标准字体大小.</param>
        /// <param name="location">弹幕位置.</param>
        /// <returns>是否发送成功.</returns>
        Task<bool> SendDanmakuAsync(string content, string videoId, string partId, int progress, string color, bool isStandardSize, DanmakuLocation location);

        /// <summary>
        /// 报告播放进度记录.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">视频分P的Id.</param>
        /// <param name="progress">播放进度.</param>
        /// <returns>进度上报是否成功.</returns>
        Task<bool> ReportProgressAsync(string videoId, string partId, double progress);

        /// <summary>
        /// 报告播放进度记录.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <param name="episodeId">分集Id.</param>
        /// <param name="seasonId">剧集Id.</param>
        /// <param name="progress">播放进度.</param>
        /// <returns>进度上报是否成功.</returns>
        Task<bool> ReportProgressAsync(string videoId, string partId, string episodeId, string seasonId, double progress);

        /// <summary>
        /// 点赞视频.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="isLike">是否点赞.</param>
        /// <returns>结果.</returns>
        Task<bool> LikeAsync(string videoId, bool isLike);

        /// <summary>
        /// 投币.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="number">投币数量，上限为2.</param>
        /// <param name="alsoLike">是否同时点赞.</param>
        /// <returns>投币结果.</returns>
        Task<bool> CoinAsync(string videoId, int number, bool alsoLike);

        /// <summary>
        /// 添加收藏.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="needAddFavoriteList">需要添加的收藏夹列表.</param>
        /// <param name="needRemoveFavoriteList">需要移除的收藏夹列表.</param>
        /// <param name="isVideo">是否为视频.</param>
        /// <returns>收藏结果.</returns>
        Task<FavoriteResult> FavoriteAsync(string videoId, IEnumerable<string> needAddFavoriteList, IEnumerable<string> needRemoveFavoriteList, bool isVideo);

        /// <summary>
        /// 一键三连.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns>三连结果.</returns>
        Task<TripleInformation> TripleAsync(string videoId);

        /// <summary>
        /// 获取视频字幕索引.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <returns>字幕索引.</returns>
        Task<IEnumerable<SubtitleMeta>> GetSubtitleIndexAsync(string videoId, string partId);

        /// <summary>
        /// 获取视频字幕详情.
        /// </summary>
        /// <param name="url">字幕地址.</param>
        /// <returns>字幕详情.</returns>
        Task<IEnumerable<SubtitleInformation>> GetSubtitleDetailAsync(string url);

        /// <summary>
        /// 获取互动视频选区.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="graphVersion">版本号.</param>
        /// <param name="edgeId">选区Id.</param>
        /// <returns>选区响应.</returns>
        Task<IEnumerable<InteractionInformation>> GetInteractionInformationsAsync(string videoId, string graphVersion, string edgeId);

        /// <summary>
        /// 获取视频的社区数据.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns>社区数据.</returns>
        Task<VideoCommunityInformation> GetVideoCommunityInformationAsync(string videoId);
    }
}
