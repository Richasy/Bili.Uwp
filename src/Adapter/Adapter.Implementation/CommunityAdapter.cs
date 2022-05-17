// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Enums.Community;
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
        private readonly IImageAdapter _imageAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityAdapter"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字处理工具.</param>
        /// <param name="imageAdapter">图片数据适配器.</param>
        public CommunityAdapter(
            INumberToolkit numberToolkit,
            IImageAdapter imageAdapter)
        {
            _numberToolkit = numberToolkit;
            _imageAdapter = imageAdapter;
        }

        /// <inheritdoc/>
        public BannerIdentifier ConvertToBannerIdentifier(PartitionBanner banner)
        {
            var id = banner.Id.ToString();
            var title = banner.Title;
            var image = _imageAdapter.ConvertToImage(banner.Image, 600, 180);
            var uri = banner.NavigateUri;
            return new BannerIdentifier(id, title, image, uri);
        }

        /// <inheritdoc/>
        public Models.Data.Community.Partition ConvertToPartition(Models.BiliBili.Partition partition)
        {
            var id = partition.Tid.ToString();
            var name = partition.Name;
            var logo = string.IsNullOrEmpty(partition.Logo)
                ? null
                : _imageAdapter.ConvertToImage(partition.Logo);
            var children = partition.Children?.Select(p => ConvertToPartition(p)).ToList();
            if (children?.Count > 0)
            {
                children.Insert(0, new Models.Data.Community.Partition(partition.Tid.ToString(), "推荐"));
            }

            return new Models.Data.Community.Partition(id, name, logo, children);
        }

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
                -1,
                (UserRelationStatus)spaceInfo.Relation.Status);

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(RecommendCard videoCard)
        {
            var playCount = _numberToolkit.GetCountNumber(videoCard.PlayCountText, "观看");
            var danmakuCount = -1d;
            var trackCount = -1d;

            if (videoCard.SubStatusText.Contains("弹幕"))
            {
                danmakuCount = _numberToolkit.GetCountNumber(videoCard.SubStatusText, "弹幕");
            }
            else
            {
                var tempText = videoCard.SubStatusText
                    .Replace("追剧", string.Empty)
                    .Replace("追番", string.Empty);
                trackCount = _numberToolkit.GetCountNumber(tempText);
            }

            var recommendReason = videoCard.RecommendReason;
            return new VideoCommunityInformation(
                videoCard.Parameter,
                playCount: playCount,
                danmakuCount: danmakuCount,
                trackCount: trackCount);
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
                playCount);
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
                commentCount: videoStat.Reply,
                shareCount: videoStat.Share);
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

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(PgcEpisodeStat stat)
        {
            return new VideoCommunityInformation(
                default,
                stat.PlayCount,
                stat.DanmakuCount,
                stat.LikeCount,
                coinCount: stat.CoinCount,
                commentCount: stat.ReplyCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(PgcInformationStat stat)
        {
            var tracingCount = _numberToolkit.GetCountNumber(stat.FollowerDisplayText);
            return new VideoCommunityInformation(
                default,
                stat.PlayCount,
                stat.DanmakuCount,
                stat.LikeCount,
                -1,
                stat.FavoriteCount,
                stat.CoinCount,
                stat.ReplyCount,
                stat.ShareCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(PgcItemStat stat)
        {
            return new VideoCommunityInformation(
                default,
                stat.ViewCount,
                stat.DanmakuCount,
                trackCount: stat.FollowCount);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(PgcSearchItem item)
        {
            return new VideoCommunityInformation(
                item.SeasonId.ToString(),
                score: item.Rating);
        }

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(PgcPlayListItemStat stat)
        {
            return new VideoCommunityInformation(
                default,
                stat.PlayCount,
                stat.DanmakuCount,
                favoriteCount: stat.FavoriteCount);
        }
    }
}
