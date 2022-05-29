﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
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
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IImageAdapter _imageAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityAdapter"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字处理工具.</param>
        /// <param name="resourceToolkit">资源管理工具.</param>
        /// <param name="imageAdapter">图片数据适配器.</param>
        public CommunityAdapter(
            INumberToolkit numberToolkit,
            IResourceToolkit resourceToolkit,
            IImageAdapter imageAdapter)
        {
            _numberToolkit = numberToolkit;
            _imageAdapter = imageAdapter;
            _resourceToolkit = resourceToolkit;
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
        public BannerIdentifier ConvertToBannerIdentifier(LiveFeedBanner banner)
        {
            var id = banner.Id.ToString();
            var title = banner.Title;
            var image = _imageAdapter.ConvertToImage(banner.Cover, 600, 180);
            var uri = banner.Link;
            return new BannerIdentifier(id, title, image, uri);
        }

        /// <inheritdoc/>
        public BannerIdentifier ConvertToBannerIdentifier(PgcModuleItem item)
        {
            var id = item.OriginId.ToString();
            var title = item.Title;
            var image = _imageAdapter.ConvertToImage(item.Cover, 600, 320);
            var uri = item.WebLink;
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
                children.ForEach(p => p.ParentId = id);
            }

            return new Models.Data.Community.Partition(id, name, logo, children);
        }

        /// <inheritdoc/>
        public Models.Data.Community.Partition ConvertToPartition(LiveFeedHotArea area)
        {
            var id = area.AreaId.ToString();
            var parentId = area.ParentAreaId.ToString();
            var name = area.Title;
            var logo = string.IsNullOrEmpty(area.Cover)
                ? null
                : _imageAdapter.ConvertToImage(area.Cover);

            return new Models.Data.Community.Partition(id, name, logo, parentId: parentId);
        }

        /// <inheritdoc/>
        public Models.Data.Community.Partition ConvertToPartition(LiveAreaGroup group)
        {
            var id = group.Id.ToString();
            var name = group.Name;
            var children = group.AreaList.Select(p => ConvertToPartition(p)).ToList();

            return new Models.Data.Community.Partition(id, name, children: children);
        }

        /// <inheritdoc/>
        public Models.Data.Community.Partition ConvertToPartition(LiveArea area)
        {
            var id = area.Id.ToString();
            var parentId = area.ParentId.ToString();
            var name = area.Name;
            var logo = string.IsNullOrEmpty(area.Cover)
                ? null
                : _imageAdapter.ConvertToImage(area.Cover);

            return new Models.Data.Community.Partition(id, name, logo, parentId: parentId);
        }

        /// <inheritdoc/>
        public Models.Data.Community.Partition ConvertToPartition(PgcTab tab)
            => new Models.Data.Community.Partition(tab.Id.ToString(), tab.Title);

        /// <inheritdoc/>
        public Models.Data.Community.Partition ConvertToPartition(ArticleCategory category)
        {
            var id = category.Id.ToString();
            var name = category.Name;
            var children = category.Children?.Any() ?? false
                ? category.Children.Select(p => ConvertToPartition(p)).ToList()
                : null;
            var parentId = category.ParentId.ToString();

            return new Models.Data.Community.Partition(id, name, children: children, parentId: parentId);
        }

        /// <inheritdoc/>
        public ArticleCommunityInformation ConvertToArticleCommunityInformation(ArticleStats stats, string articleId)
        {
            return new ArticleCommunityInformation(
                articleId,
                stats.ViewCount,
                stats.FavoriteCount,
                stats.LikeCount,
                stats.ReplyCount,
                stats.ShareCount,
                stats.CoinCount);
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

        /// <inheritdoc/>
        public UnreadInformation ConvertToUnreadInformation(UnreadMessage message)
            => new UnreadInformation(message.At, message.Reply, message.Like);

        /// <inheritdoc/>
        public MessageInformation ConvertToMessageInformation(LikeMessageItem messageItem)
        {
            var isMultiple = messageItem.Users.Count > 1;
            var firstUser = messageItem.Users[0];
            var userName = firstUser.UserName;
            var avatar = _imageAdapter.ConvertToImage(firstUser.Avatar, 48, 48);
            string message;
            if (isMultiple)
            {
                var secondUser = messageItem.Users[1];
                message = string.Format(
                        _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.LikeMessageMultipleDescription),
                        userName,
                        secondUser.UserName,
                        messageItem.Count,
                        messageItem.Item.Business);
            }
            else
            {
                message = string.Format(
                        _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.LikeMessageSingleDescription),
                        userName,
                        messageItem.Item.Business);
            }

            var publishTime = DateTimeOffset.FromUnixTimeSeconds(messageItem.LikeTime).ToLocalTime().DateTime;
            var id = messageItem.Id.ToString();
            var sourceContent = string.IsNullOrEmpty(messageItem.Item.Title)
                ? messageItem.Item.Description
                : messageItem.Item.Title;
            var sourceId = messageItem.Item.Uri;

            return new MessageInformation(
                id,
                Models.Enums.App.MessageType.Like,
                avatar,
                string.Empty,
                isMultiple,
                publishTime,
                string.Empty,
                message,
                sourceContent,
                sourceId);
        }

        /// <inheritdoc/>
        public MessageInformation ConvertToMessageInformation(AtMessageItem messageItem)
        {
            var user = messageItem.User;
            var userName = user.UserName;
            var avatar = _imageAdapter.ConvertToImage(user.Avatar, 48, 48);
            var subtitle = string.Format(
                _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AtMessageTypeDescription),
                messageItem.Item.Business);
            var message = messageItem.Item.SourceContent;
            var sourceContent = string.IsNullOrEmpty(messageItem.Item.Title)
                ? _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NoSpecificData)
                : messageItem.Item.Title;
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(messageItem.AtTime).ToLocalTime().DateTime;
            var id = messageItem.Id.ToString();
            var sourceId = messageItem.Item.Uri;

            return new MessageInformation(
                id,
                Models.Enums.App.MessageType.At,
                avatar,
                userName,
                false,
                publishTime,
                subtitle,
                message,
                sourceContent,
                sourceId);
        }

        /// <inheritdoc/>
        public MessageInformation ConvertToMessageInformation(ReplyMessageItem messageItem)
        {
            var user = messageItem.User;
            var userName = user.UserName;
            var avatar = _imageAdapter.ConvertToImage(user.Avatar, 48, 48);
            var isMultiple = messageItem.IsMultiple == 1;
            var subtitle = string.Format(
                _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.ReplyMessageTypeDescription),
                messageItem.Item.Business,
                messageItem.Counts);
            var message = messageItem.Item.SourceContent;
            var sourceContent = string.IsNullOrEmpty(messageItem.Item.Title)
                ? messageItem.Item.Description
                : messageItem.Item.Title;
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(messageItem.ReplyTime).ToLocalTime().DateTime;
            var id = messageItem.Id.ToString();
            var sourceId = messageItem.Item.SubjectId.ToString();
            var properties = new Dictionary<string, string>()
            {
                { "type", messageItem.Item.BusinessId },
            };

            return new MessageInformation(
                id,
                Models.Enums.App.MessageType.Reply,
                avatar,
                userName,
                isMultiple,
                publishTime,
                subtitle,
                message,
                sourceContent,
                sourceId,
                properties);
        }

        /// <inheritdoc/>
        public MessageView ConvertToMessageView(LikeMessageResponse messageResponse)
        {
            var cursor = messageResponse.Total.Cursor;
            var items = new List<MessageInformation>();
            if (messageResponse.Latest != null)
            {
                items = items
                    .Concat(messageResponse.Latest.Items.Select(p => ConvertToMessageInformation(p)))
                    .ToList();
            }

            if (messageResponse.Total != null)
            {
                items = items
                    .Concat(messageResponse.Total.Items.Select(p => ConvertToMessageInformation(p)))
                    .ToList();
            }

            return new MessageView(items, cursor.IsEnd);
        }

        /// <inheritdoc/>
        public MessageView ConvertToMessageView(AtMessageResponse messageResponse)
        {
            var cursor = messageResponse.Cursor;
            var items = messageResponse.Items.Select(p => ConvertToMessageInformation(p)).ToList();
            return new MessageView(items, cursor.IsEnd);
        }

        /// <inheritdoc/>
        public MessageView ConvertToMessageView(ReplyMessageResponse messageResponse)
        {
            var cursor = messageResponse.Cursor;
            var items = messageResponse.Items.Select(p => ConvertToMessageInformation(p)).ToList();
            return new MessageView(items, cursor.IsEnd);
        }
    }
}
