// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bilibili.App.Dynamic.V2;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 社区数据适配器接口.
    /// </summary>
    public interface ICommunityAdapter
    {
        /// <summary>
        /// 将个人信息 <see cref="Mine"/> 转换为用户社区交互信息.
        /// </summary>
        /// <param name="mine">个人信息.</param>
        /// <returns><see cref="UserCommunityInformation"/>.</returns>
        UserCommunityInformation ConvertToUserCommunityInformation(Mine mine);

        /// <summary>
        /// 将用户空间信息 <see cref="UserSpaceInformation"/> 转换为用户社区交互信息.
        /// </summary>
        /// <param name="spaceInfo">用户空间信息.</param>
        /// <returns><see cref="UserCommunityInformation"/>.</returns>
        UserCommunityInformation ConvertToUserCommunityInformation(UserSpaceInformation spaceInfo);

        /// <summary>
        /// 将推荐卡片 <see cref="RecommendCard"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="videoCard">推荐卡片信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        /// <remarks>
        /// 这里的转换只是将 <see cref="RecommendCard"/> 中关于社区交互的信息提取出来，其它的视频信息交由 <see cref="IVideoAdapter"/> 来处理.
        /// </remarks>
        VideoCommunityInformation ConvertToVideoCommunityInformation(RecommendCard videoCard);

        /// <summary>
        /// 将分区视频 <see cref="PartitionVideo"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="video">分区视频信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(PartitionVideo video);

        /// <summary>
        /// 将动态视频 <see cref="MdlDynArchive"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="video">动态视频信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(MdlDynArchive video);

        /// <summary>
        /// 将视频状态信息 <see cref="VideoStatusInfo"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="statusInfo">状态信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(VideoStatusInfo statusInfo);

        /// <summary>
        /// 将排行榜视频 <see cref="Bilibili.App.Show.V1.Item"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="rankItem">排行榜视频.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(Bilibili.App.Show.V1.Item rankItem);

        /// <summary>
        /// 将热门视频 <see cref="Bilibili.App.Card.V1.Card"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="hotVideo">热门视频.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(Bilibili.App.Card.V1.Card hotVideo);

        /// <summary>
        /// 将视频状态数据 <see cref="Bilibili.App.Archive.V1.Stat"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="videoStat">视频状态数据.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(Bilibili.App.Archive.V1.Stat videoStat);

        /// <summary>
        /// 将视频搜索条目 <see cref="VideoSearchItem"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="searchVideo">搜索视频条目.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(VideoSearchItem searchVideo);

        /// <summary>
        /// 将用户空间视频条目 <see cref="UserSpaceVideoItem"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="video">用户空间视频条目.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(UserSpaceVideoItem video);

        /// <summary>
        /// 将收藏夹视频条目 <see cref="FavoriteMedia"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="video">收藏夹视频条目.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(FavoriteMedia video);

        /// <summary>
        /// 将剧集单集社区信息 <see cref="PgcEpisodeStat"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="stat">剧集单集社区信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(PgcEpisodeStat stat);

        /// <summary>
        /// 将剧集社区信息 <see cref="PgcInformationStat"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="stat">剧集社区信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(PgcInformationStat stat);
    }
}
