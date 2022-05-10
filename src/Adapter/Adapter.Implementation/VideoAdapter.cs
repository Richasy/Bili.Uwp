// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Player;
using Bili.Models.Data.User;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Card.V1;
using Bilibili.App.Dynamic.V2;
using Bilibili.App.Interfaces.V1;
using Bilibili.App.Show.V1;
using Bilibili.App.View.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 视频信息适配器.
    /// </summary>
    public sealed class VideoAdapter : IVideoAdapter
    {
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IUserAdapter _userAdapter;
        private readonly IImageAdapter _imageAdapter;
        private readonly INumberToolkit _numberToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoAdapter"/> class.
        /// </summary>
        /// <param name="communityAdapter">社区数据适配器.</param>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="imageAdapter">图片适配器.</param>
        /// <param name="numberToolkit">数字工具.</param>
        public VideoAdapter(
            ICommunityAdapter communityAdapter,
            IUserAdapter userAdapter,
            IImageAdapter imageAdapter,
            INumberToolkit numberToolkit)
        {
            _communityAdapter = communityAdapter;
            _userAdapter = userAdapter;
            _imageAdapter = imageAdapter;
            _numberToolkit = numberToolkit;
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(RecommendCard videoCard)
        {
            if (videoCard.CardGoto != ServiceConstants.Av)
            {
                throw new ArgumentException($"推荐卡片的 CardGoTo 属性应该是 {ServiceConstants.Av}，这里是 {videoCard.Goto}，不符合要求，请检查分配条件", nameof(videoCard));
            }

            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(videoCard);
            var publisher = _userAdapter.ConvertToPublisherProfile(videoCard.Mask.Avatar);
            var title = videoCard.Title;
            var id = videoCard.Parameter;
            var duration = (videoCard.PlayerArgs?.Duration).HasValue
                    ? videoCard.PlayerArgs.Duration
                    : 0;
            var subtitle = videoCard.Description;
            var cover = _imageAdapter.ConvertToVideoCardCover(videoCard.Cover);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                subtitle: subtitle,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(PartitionVideo video)
        {
            var title = video.Title;
            var id = video.Parameter;
            var duration = video.Duration;
            var subtitle = video.Publisher;
            var cover = _imageAdapter.ConvertToVideoCardCover(video.Cover);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(video);
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(video.PublishDateTime).ToLocalTime();
            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                null,
                subtitle: subtitle,
                publishTime: publishTime.DateTime,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(MdlDynArchive dynamicVideo)
        {
            if (dynamicVideo.IsPGC)
            {
                throw new ArgumentException($"该动态视频是 PGC 内容，不符合要求，请检查分配条件", nameof(dynamicVideo));
            }

            var title = dynamicVideo.Title;
            var id = dynamicVideo.Avid.ToString();
            var bvid = dynamicVideo.Bvid;
            var duration = Convert.ToInt32(dynamicVideo.Duration);
            var cover = _imageAdapter.ConvertToImage(dynamicVideo.Cover, AppConstants.DynamicCoverWidth, AppConstants.DynamicCoverHeight);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(dynamicVideo);
            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                null,
                bvid,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(ViewLaterVideo video)
        {
            var title = video.Title;
            var duration = video.Duration;
            var id = video.VideoId.ToString();
            var bvid = video.BvId;
            var description = video.Description;
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(video.PublishDateTime).ToLocalTime();
            var cover = _imageAdapter.ConvertToVideoCardCover(video.Cover);
            var publisher = _userAdapter.ConvertToPublisherProfile(video.Publisher, Models.Enums.App.AvatarSize.Size48);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(video.StatusInfo);
            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                bvid,
                description: description,
                publishTime: publishTime.DateTime,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(Item rankVideo)
        {
            var id = rankVideo.Param;
            var title = rankVideo.Title;
            var duration = Convert.ToInt32(rankVideo.Duration);
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(rankVideo.PubDate).ToLocalTime();

            var user = _userAdapter.ConvertToUserProfile(Convert.ToInt32(rankVideo.Mid), rankVideo.Name, rankVideo.Face, Models.Enums.App.AvatarSize.Size48);
            var publisher = new PublisherProfile(user);
            var cover = _imageAdapter.ConvertToVideoCardCover(rankVideo.Cover);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(rankVideo);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                publishTime: publishTime.DateTime,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(Card hotVideo)
        {
            var v5 = hotVideo.SmallCoverV5;
            var card = v5.Base;
            var shareInfo = card.ThreePointV4.SharePlane;
            var title = card.Title;
            var id = shareInfo.Aid.ToString();
            var bvId = shareInfo.Bvid;
            var subtitle = shareInfo.Author;
            var description = shareInfo.Desc;

            // 对于热门视频来说，它会把观看数和发布时间揉在一起，比如 "13.5万观看 · 21小时前"，
            // 考虑到我们的需求，这里需要把它拆开，让发布时间和作者名一起作为副标题存在，就像推荐视频一样.
            var descSplit = v5.RightDesc2.Split('·');
            if (descSplit.Length > 1)
            {
                var publishTimeText = descSplit[1].Trim();
                subtitle += $" · {publishTimeText}";
            }

            var duration = _numberToolkit.GetDurationSeconds(v5.CoverRightText1);
            var cover = _imageAdapter.ConvertToVideoCardCover(card.Cover);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(hotVideo);

            var identifier = new VideoIdentifier(id, title, duration, cover);

            return new VideoInformation(
                identifier,
                null,
                bvId,
                description: description,
                subtitle: subtitle,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(Relate relatedVideo)
        {
            var title = relatedVideo.Title;
            var id = relatedVideo.Aid.ToString();
            var duration = Convert.ToInt32(relatedVideo.Duration);
            var description = relatedVideo.Desc;
            var publisher = _userAdapter.ConvertToPublisherProfile(relatedVideo.Author);
            var cover = _imageAdapter.ConvertToVideoCardCover(relatedVideo.Pic);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(relatedVideo.Stat);
            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                description: description,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(VideoSearchItem searchVideo)
        {
            var title = searchVideo.Title;
            var id = searchVideo.Parameter;
            var duration = _numberToolkit.GetDurationSeconds(searchVideo.Duration);
            var cover = _imageAdapter.ConvertToVideoCardCover(searchVideo.Cover);
            var description = searchVideo.Description;
            var user = _userAdapter.ConvertToUserProfile(searchVideo.UserId, searchVideo.Author, searchVideo.Avatar, Models.Enums.App.AvatarSize.Size48);
            var publisher = new PublisherProfile(user);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(searchVideo);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                description: description,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(UserSpaceVideoItem spaceVideo)
        {
            var title = spaceVideo.Title;
            var id = spaceVideo.Id;
            var publishDate = DateTimeOffset.FromUnixTimeSeconds(spaceVideo.CreateTime).ToLocalTime();
            var duration = spaceVideo.Duration;
            var cover = _imageAdapter.ConvertToVideoCardCover(spaceVideo.Cover);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(spaceVideo);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                null,
                publishTime: publishDate.DateTime,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(CursorItem historyVideo)
        {
            if (historyVideo.CardItemCase != CursorItem.CardItemOneofCase.CardUgc)
            {
                throw new ArgumentException($"该历史记录不是视频内容，是 {historyVideo.CardItemCase}, 不符合要求，请检查分配条件", nameof(historyVideo));
            }

            var video = historyVideo.CardUgc;
            var title = historyVideo.Title;
            var id = historyVideo.Kid.ToString();
            var bvid = video.Bvid;
            var subtitle = $"{video.Name} · {TimeSpan.FromSeconds(video.Progress)}";
            var duration = Convert.ToInt32(video.Duration);
            var cover = _imageAdapter.ConvertToVideoCardCover(video.Cover);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(identifier, default, bvid, subtitle: subtitle);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(FavoriteMedia video)
        {
            var title = video.Title;
            var id = video.Id.ToString();
            var publisher = _userAdapter.ConvertToPublisherProfile(video.Publisher, Models.Enums.App.AvatarSize.Size48);
            var duration = video.Duration;
            var cover = _imageAdapter.ConvertToVideoCardCover(video.Cover);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(video);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(identifier, publisher, communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(Arc video)
        {
            var archive = video.Archive;
            var title = archive.Title;
            var id = archive.Aid.ToString();
            var publisher = _userAdapter.ConvertToPublisherProfile(archive.Author);
            var duration = Convert.ToInt32(archive.Duration);
            var cover = _imageAdapter.ConvertToVideoCardCover(archive.Pic);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(archive.Stat);
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(archive.Pubdate);
            var description = archive.Desc;

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                description: description,
                publishTime: publishTime.DateTime,
                communityInformation: communityInfo);
        }
    }
}
