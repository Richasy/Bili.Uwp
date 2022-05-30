// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Data.Player;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Models.Enums.Community;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Card.V1;
using Bilibili.App.Dynamic.V2;
using Bilibili.App.Interfaces.V1;
using Bilibili.App.Show.V1;
using Bilibili.App.View.V1;
using Humanizer;

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
            var publisher = _userAdapter.ConvertToRoleProfile(videoCard.Mask.Avatar);
            var title = videoCard.Title;
            var id = videoCard.Parameter;
            var duration = (videoCard.PlayerArgs?.Duration).HasValue
                    ? videoCard.PlayerArgs.Duration
                    : 0;
            var subtitle = videoCard.Description;
            var cover = _imageAdapter.ConvertToVideoCardCover(videoCard.Cover);
            var highlight = videoCard.RecommendReason;

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                subtitle: subtitle,
                highlight: highlight,
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
            var publisher = _userAdapter.ConvertToRoleProfile(video.Publisher, Models.Enums.App.AvatarSize.Size48);
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
            var publisher = new RoleProfile(user);
            var cover = _imageAdapter.ConvertToVideoCardCover(rankVideo.Cover);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(rankVideo);
            var subtitle = $"{user.Name} · {publishTime.Humanize()}";

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                subtitle: subtitle,
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
            var highlight = hotVideo.SmallCoverV5.RcmdReasonStyle?.Text ?? string.Empty;

            var identifier = new VideoIdentifier(id, title, duration, cover);

            return new VideoInformation(
                identifier,
                null,
                bvId,
                description: description,
                subtitle: subtitle,
                highlight: highlight,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(Relate relatedVideo)
        {
            var title = relatedVideo.Title;
            var id = relatedVideo.Aid.ToString();
            var duration = Convert.ToInt32(relatedVideo.Duration);
            var description = relatedVideo.Desc;
            var publisher = _userAdapter.ConvertToRoleProfile(relatedVideo.Author);
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
            var publisher = new RoleProfile(user);
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
            var publisher = _userAdapter.ConvertToRoleProfile(video.Publisher, Models.Enums.App.AvatarSize.Size48);
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
            var publisher = _userAdapter.ConvertToRoleProfile(archive.Author);
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

        /// <inheritdoc/>
        public VideoView ConvertToVideoInformation(ViewReply videoDetail)
        {
            var videoInfo = GetVideoInformationFromViewReply(videoDetail);
            var subVideos = GetSubVideosFromViewReply(videoDetail);
            var interaction = GetInteractionRecordFromViewReply(videoDetail);
            var publisherCommunity = GetPublisherCommunityFromViewReply(videoDetail);
            var operation = GetOperationInformationFromViewReply(videoDetail);
            var relatedVideos = GetRelatedVideosFromViewReply(videoDetail);
            var sections = GetVideoSectionsFromViewReply(videoDetail);
            var history = GetHistoryFromViewReply(videoDetail);

            if (subVideos.Count() > 0)
            {
                var historyVideo = subVideos.FirstOrDefault(p => p.Id.Equals(history.Identifier.Id));
                history.Identifier = historyVideo;
            }
            else if (sections.Count() > 0)
            {
                history.Identifier = sections
                    .SelectMany(p => p.Videos)
                    .FirstOrDefault(p => p.AlternateId == history.Identifier.Id)
                    .Identifier;
            }

            var tags = videoDetail.Tag.Select(p => new Models.Data.Community.Tag(p.Id.ToString(), p.Name.TrimStart('#'), p.Uri));

            return new VideoView(
                videoInfo,
                publisherCommunity,
                subVideos,
                sections,
                relatedVideos,
                history,
                operation,
                interaction,
                tags);
        }

        /// <inheritdoc/>
        public ViewLaterView ConvertToViewLaterView(ViewLaterResponse response)
        {
            var count = response.Count;
            var items = response.List == null
                ? new List<VideoInformation>()
                : response.List.Select(p => ConvertToVideoInformation(p)).ToList();
            return new ViewLaterView(items, count);
        }

        private VideoInformation GetVideoInformationFromEpisode(Episode episode)
        {
            var id = episode.Aid.ToString();
            var cid = episode.Cid.ToString();
            var title = Regex.Replace(episode.Title, "<[^>]+>", string.Empty);
            var duration = Convert.ToInt32(episode.Page.Duration);
            var publisher = _userAdapter.ConvertToRoleProfile(episode.Author);
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(episode.Stat);
            var cover = _imageAdapter.ConvertToVideoCardCover(episode.Cover);
            var subtitle = episode.CoverRightText;

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(identifier, publisher, cid, subtitle: subtitle, communityInformation: communityInfo);
        }

        private VideoInformation GetVideoInformationFromViewReply(ViewReply videoDetail)
        {
            var arc = videoDetail.Arc;
            var title = arc.Title;
            var id = arc.Aid.ToString();
            var bvid = videoDetail.Bvid;
            var duration = Convert.ToInt32(arc.Duration);
            var cover = _imageAdapter.ConvertToImage(arc.Pic);
            var collaborators = videoDetail.Staff.Count > 0
                ? videoDetail.Staff.Select(p => _userAdapter.ConvertToRoleProfile(p, Models.Enums.App.AvatarSize.Size32))
                : null;
            var publisher = videoDetail.Staff.Count > 0
                ? null
                : _userAdapter.ConvertToRoleProfile(arc.Author, Models.Enums.App.AvatarSize.Size32);
            var description = arc.Desc;
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(arc.Pubdate).ToLocalTime().DateTime;
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(arc.Stat);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                bvid,
                description,
                publishTime: publishTime,
                collaborators: collaborators,
                communityInformation: communityInfo);
        }

        private IEnumerable<VideoIdentifier> GetSubVideosFromViewReply(ViewReply videoDetail)
        {
            var subVideos = new List<VideoIdentifier>();
            foreach (var page in videoDetail.Pages.OrderBy(p => p.Page.Page_))
            {
                var cid = page.Page.Cid.ToString();
                var title = page.Page.Part;
                var duration = Convert.ToInt32(page.Page.Duration);
                var identifier = new VideoIdentifier(cid, title, duration, null);
                subVideos.Add(identifier);
            }

            return subVideos;
        }

        private InteractionVideoRecord GetInteractionRecordFromViewReply(ViewReply videoDetail)
        {
            var interaction = videoDetail.Interaction;
            var partId = interaction.HistoryNode != null
                    ? interaction.HistoryNode.Cid.ToString()
                    : videoDetail.Pages.First().Page.Cid.ToString();
            var nodeId = interaction.HistoryNode != null
                    ? interaction.HistoryNode.NodeId.ToString()
                    : string.Empty;
            var graphVersion = interaction.GraphVersion.ToString();

            return new InteractionVideoRecord(graphVersion, partId, nodeId);
        }

        private UserCommunityInformation GetPublisherCommunityFromViewReply(ViewReply videoDetail)
        {
            var relation = UserRelationStatus.Unfollow;
            if (videoDetail.ReqUser.Attention == 1)
            {
                relation = videoDetail.ReqUser.GuestAttention == 1
                    ? UserRelationStatus.Friends
                    : UserRelationStatus.Following;
            }
            else if (videoDetail.ReqUser.GuestAttention == 1)
            {
                relation = UserRelationStatus.BeFollowed;
            }

            return new UserCommunityInformation()
            {
                Id = videoDetail.Arc.Author.Mid.ToString(),
                Relation = relation,
            };
        }

        private VideoOpeartionInformation GetOperationInformationFromViewReply(ViewReply videoDetail)
        {
            var reqUser = videoDetail.ReqUser;
            var isLiked = reqUser.Like == 1;
            var isCoined = reqUser.Coin == 1;
            var isFavorited = reqUser.Favorite == 1;
            return new VideoOpeartionInformation(
                videoDetail.Arc.Aid.ToString(),
                isLiked,
                isCoined,
                isFavorited,
                false);
        }

        private IEnumerable<VideoInformation> GetRelatedVideosFromViewReply(ViewReply videoDetail)
        {
            // 将非视频内容过滤掉.
            var relates = videoDetail.Relates.Where(p => p.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase));
            var relatedVideos = relates.Select(p => ConvertToVideoInformation(p));
            return relatedVideos;
        }

        private IEnumerable<VideoSection> GetVideoSectionsFromViewReply(ViewReply videoDetail)
        {
            if (videoDetail.UgcSeason == null || videoDetail.UgcSeason.Sections?.Count == 0)
            {
                return null;
            }

            var sections = new List<VideoSection>();
            foreach (var item in videoDetail.UgcSeason.Sections)
            {
                var id = item.Id.ToString();
                var title = item.Title;
                var videos = item.Episodes.Select(p => GetVideoInformationFromEpisode(p));
                var section = new VideoSection(id, title, videos);
                sections.Add(section);
            }

            return sections;
        }

        private PlayedProgress GetHistoryFromViewReply(ViewReply videoDetail)
        {
            // 当历史记录为空，或者当前视频为交互视频时（交互视频按照节点记录历史），返回 null.
            if (videoDetail.History == null || videoDetail.Interaction != null)
            {
                return null;
            }

            var history = videoDetail.History;
            var historyStatus = history.Progress switch
            {
                0 => PlayedProgressStatus.NotStarted,
                -1 => PlayedProgressStatus.Finish,
                _ => PlayedProgressStatus.Playing
            };

            var progress = historyStatus == PlayedProgressStatus.Playing
                ? history.Progress
                : 0;

            var id = history.Cid.ToString();
            var identifier = new VideoIdentifier(id, default, default, default);
            return new PlayedProgress(progress, historyStatus, identifier);
        }
    }
}
