// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Archive.V1;
using Bilibili.App.Card.V1;
using Bilibili.App.Dynamic.V2;
using Bilibili.App.Show.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 社区数据适配器.
    /// </summary>
    public sealed class CommunityAdapter : ICommunityAdapter
    {
        private readonly INumberToolkit _numberToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityAdapter"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字处理工具.</param>
        public CommunityAdapter(INumberToolkit numberToolkit)
            => _numberToolkit = numberToolkit;

        /// <inheritdoc/>
        public UserCommunityInformation ConvertToUserCommunityInformation(Mine mine)
            => new UserCommunityInformation(
                mine.Mid.ToString(),
                mine.FollowCount,
                mine.FollowerCount,
                mine.CoinNumber,
                -1,
                mine.DynamicCount);

        /// <inheritdoc/>
        public UserCommunityInformation ConvertToUserCommunityInformation(UserSpaceInformation spaceInfo)
            => new UserCommunityInformation(
                spaceInfo.UserId,
                spaceInfo.FollowCount,
                spaceInfo.FollowerCount,
                -1,
                spaceInfo.LikeInformation.LikeCount,
                -1);

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(RecommendCard videoCard)
        {
            var playCount = _numberToolkit.GetCountNumber(videoCard.PlayCountText, "观看");
            var danmakuCount = _numberToolkit.GetCountNumber(videoCard.SubStatusText, "弹幕");
            var recommendReason = videoCard.RecommendReason;
            return new VideoCommunityInformation(
                videoCard.Parameter,
                playCount: playCount,
                danmakuCount: danmakuCount,
                recommendReason: recommendReason);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(PartitionVideo video)
        {
            return new VideoCommunityInformation(
                video.Parameter,
                video.PlayCount,
                video.DanmakuCount,
                video.LikeCount,
                commentCount: video.ReplyCount,
                favoriteCount: video.FavouriteCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(MdlDynArchive video)
        {
            var danmakuCount = _numberToolkit.GetCountNumber(video.CoverLeftText3, "弹幕");
            return new VideoCommunityInformation(video.Avid.ToString(), video.View, danmakuCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(VideoStatusInfo statusInfo)
        {
            return new VideoCommunityInformation(
                statusInfo.Aid.ToString(),
                statusInfo.PlayCount,
                statusInfo.DanmakuCount,
                statusInfo.LikeCount,
                favoriteCount: statusInfo.FavoriteCount,
                coinCount: statusInfo.CoinCount,
                commentCount: statusInfo.ReplyCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(Item rankItem)
        {
            return new VideoCommunityInformation(
                rankItem.Param,
                rankItem.Play,
                rankItem.Danmaku,
                rankItem.Like,
                rankItem.Pts,
                rankItem.Favourite,
                commentCount: rankItem.Reply);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(Card hotVideo)
        {
            var share = hotVideo.SmallCoverV5.Base.ThreePointV4.SharePlane;
            var playCount = _numberToolkit.GetCountNumber(share.PlayNumber, "次");
            return new VideoCommunityInformation(
                share.Aid.ToString(),
                playCount,
                recommendReason: hotVideo.SmallCoverV5.RcmdReasonStyle?.Text ?? string.Empty);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(Stat videoStat)
        {
            return new VideoCommunityInformation(
                videoStat.Aid.ToString(),
                videoStat.View,
                videoStat.Danmaku,
                videoStat.Like,
                favoriteCount: videoStat.Fav,
                coinCount: videoStat.Coin,
                commentCount: videoStat.Reply);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(VideoSearchItem searchVideo)
        {
            return new VideoCommunityInformation(
                searchVideo.Parameter,
                searchVideo.PlayCount,
                searchVideo.DanmakuCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(UserSpaceVideoItem video)
        {
            return new VideoCommunityInformation(
                video.Id,
                video.PlayCount,
                video.DanmakuCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(FavoriteMedia video)
        {
            return new VideoCommunityInformation(
                video.Id.ToString(),
                video.Stat.PlayCount,
                video.Stat.DanmakuCount,
                favoriteCount: video.Stat.FavoriteCount);
        }
    }
}
