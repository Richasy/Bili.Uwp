// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bilibili.App.Dynamic.V2;
using Bilibili.App.Interfaces.V1;

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
        /// 将个人信息 <see cref="MyInfo"/> 转换为用户社区交互信息.
        /// </summary>
        /// <param name="mine">个人信息.</param>
        /// <returns><see cref="UserCommunityInformation"/>.</returns>
        UserCommunityInformation ConvertToUserCommunityInformation(MyInfo mine);

        /// <summary>
        /// 将关系用户 <see cref="RelatedUser"/> 转换为用户社区交互信息.
        /// </summary>
        /// <param name="user">关系用户.</param>
        /// <returns><see cref="UserCommunityInformation"/>.</returns>
        UserCommunityInformation ConvertToUserCommunityInformation(RelatedUser user);

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

        /// <summary>
        /// 将 PGC 条目社区信息 <see cref="PgcItemStat"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="stat">PGC 条目社区信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(PgcItemStat stat);

        /// <summary>
        /// 将 PGC 搜索条目 <see cref="PgcSearchItem"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="item">PGC 搜索条目.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(PgcSearchItem item);

        /// <summary>
        /// 将 PGC 播放列表条目社区信息 <see cref="PgcPlayListItemStat"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="stat">PGC 搜索条目.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(PgcPlayListItemStat stat);

        /// <summary>
        /// 将 UGC 视频卡片 <see cref="PgcPlayListItemStat"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="ugc">UGC 条目信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        VideoCommunityInformation ConvertToVideoCommunityInformation(CardUGC ugc);

        /// <summary>
        /// 将文章状态 <see cref="ArticleStats"/> 转换为文章社区交互信息.
        /// </summary>
        /// <param name="stats">文章状态.</param>
        /// <param name="articleId">文章Id.</param>
        /// <returns><see cref="ArticleCommunityInformation"/>.</returns>
        ArticleCommunityInformation ConvertToArticleCommunityInformation(ArticleStats stats, string articleId);

        /// <summary>
        /// 将分区实例 <see cref="Models.BiliBili.Partition"/> 转换为自定义的分区信息.
        /// </summary>
        /// <param name="partition">分区实例.</param>
        /// <returns><see cref="Models.Data.Community.Partition"/>.</returns>
        Models.Data.Community.Partition ConvertToPartition(Models.BiliBili.Partition partition);

        /// <summary>
        /// 将直播数据流中的热门区域 <see cref="LiveFeedHotArea"/> 转换为自定义的分区信息.
        /// </summary>
        /// <param name="area">热门区域.</param>
        /// <returns><see cref="Models.Data.Community.Partition"/>.</returns>
        Models.Data.Community.Partition ConvertToPartition(LiveFeedHotArea area);

        /// <summary>
        /// 将直播分区 <see cref="LiveArea"/> 转换为自定义的分区信息.
        /// </summary>
        /// <param name="area">直播分区.</param>
        /// <returns><see cref="Models.Data.Community.Partition"/>.</returns>
        Models.Data.Community.Partition ConvertToPartition(LiveArea area);

        /// <summary>
        /// 将直播分区组 <see cref="LiveAreaGroup"/> 转换为自定义的分区信息.
        /// </summary>
        /// <param name="group">直播分区组.</param>
        /// <returns><see cref="Models.Data.Community.Partition"/>.</returns>
        Models.Data.Community.Partition ConvertToPartition(LiveAreaGroup group);

        /// <summary>
        /// 将PGC标签 <see cref="PgcTab"/> 转换为自定义的分区信息.
        /// </summary>
        /// <param name="tab">PGC标签.</param>
        /// <returns><see cref="Models.Data.Community.Partition"/>.</returns>
        Models.Data.Community.Partition ConvertToPartition(PgcTab tab);

        /// <summary>
        /// 将文章分类 <see cref="LiveAreaGroup"/> 转换为自定义的分区信息.
        /// </summary>
        /// <param name="category">文章分类.</param>
        /// <returns><see cref="Models.Data.Community.Partition"/>.</returns>
        Models.Data.Community.Partition ConvertToPartition(ArticleCategory category);

        /// <summary>
        /// 将分区横幅 <see cref="PartitionBanner"/> 转换为横幅信息.
        /// </summary>
        /// <param name="banner">分区横幅条目.</param>
        /// <returns><see cref="BannerIdentifier"/>.</returns>
        BannerIdentifier ConvertToBannerIdentifier(PartitionBanner banner);

        /// <summary>
        /// 将直播数据流横幅 <see cref="LiveFeedBanner"/> 转换为横幅信息.
        /// </summary>
        /// <param name="banner">直播数据流横幅条目.</param>
        /// <returns><see cref="BannerIdentifier"/>.</returns>
        BannerIdentifier ConvertToBannerIdentifier(LiveFeedBanner banner);

        /// <summary>
        /// 将PGC模块条目 <see cref="PgcModuleItem"/> 转换为横幅信息.
        /// </summary>
        /// <param name="item">PGC模块条目.</param>
        /// <returns><see cref="BannerIdentifier"/>.</returns>
        BannerIdentifier ConvertToBannerIdentifier(PgcModuleItem item);

        /// <summary>
        /// 将未读消息 <see cref="UnreadMessage"/> 转换为未读信息.
        /// </summary>
        /// <param name="message">未读消息.</param>
        /// <returns><see cref="UnreadInformation"/>.</returns>
        UnreadInformation ConvertToUnreadInformation(UnreadMessage message);

        /// <summary>
        /// 将点赞消息条目 <see cref="LikeMessageItem"/> 转换为消息信息.
        /// </summary>
        /// <param name="messageItem">消息条目.</param>
        /// <returns><see cref="MessageInformation"/>.</returns>
        MessageInformation ConvertToMessageInformation(LikeMessageItem messageItem);

        /// <summary>
        /// 将提及消息条目 <see cref="AtMessageItem"/> 转换为消息信息.
        /// </summary>
        /// <param name="messageItem">消息条目.</param>
        /// <returns><see cref="MessageInformation"/>.</returns>
        MessageInformation ConvertToMessageInformation(AtMessageItem messageItem);

        /// <summary>
        /// 将回复消息条目 <see cref="ReplyMessageItem"/> 转换为消息信息.
        /// </summary>
        /// <param name="messageItem">消息条目.</param>
        /// <returns><see cref="MessageInformation"/>.</returns>
        MessageInformation ConvertToMessageInformation(ReplyMessageItem messageItem);

        /// <summary>
        /// 将点赞消息响应 <see cref="LikeMessageResponse"/> 转换为消息信息.
        /// </summary>
        /// <param name="messageResponse">消息响应.</param>
        /// <returns><see cref="MessageView"/>.</returns>
        MessageView ConvertToMessageView(LikeMessageResponse messageResponse);

        /// <summary>
        /// 将提及消息响应 <see cref="AtMessageResponse"/> 转换为消息信息.
        /// </summary>
        /// <param name="messageResponse">消息响应.</param>
        /// <returns><see cref="MessageView"/>.</returns>
        MessageView ConvertToMessageView(AtMessageResponse messageResponse);

        /// <summary>
        /// 将回复消息响应 <see cref="ReplyMessageResponse"/> 转换为消息信息.
        /// </summary>
        /// <param name="messageResponse">消息响应.</param>
        /// <returns><see cref="MessageView"/>.</returns>
        MessageView ConvertToMessageView(ReplyMessageResponse messageResponse);
    }
}
