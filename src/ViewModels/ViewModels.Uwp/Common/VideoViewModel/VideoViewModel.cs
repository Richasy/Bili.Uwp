// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bilibili.App.Card.V1;
using Bilibili.App.Dynamic.V2;
using Bilibili.App.Interfaces.V1;
using Bilibili.App.Show.V1;
using Bilibili.App.View.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频视图模型.
    /// </summary>
    public partial class VideoViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="video">分区视频.</param>
        public VideoViewModel(PartitionVideo video)
            : this()
        {
            Title = video.Title ?? string.Empty;
            Publisher = new UserViewModel(video.Publisher, video.PublisherAvatar);
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.LikeCount);
            VideoId = video.Parameter;
            PartitionName = video.PartitionName;
            PartitionId = video.PartitionId;
            Source = video;
            VideoType = Models.Enums.VideoType.Video;
            LimitCover(video.Cover);
            CanShowAvatar = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="archive">稿件.</param>
        public VideoViewModel(MdlDynArchive archive)
            : this()
        {
            Title = archive.Title ?? string.Empty;
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(archive.Duration));
            PlayCount = _numberToolkit.GetCountText(archive.View);
            DanmakuCount = archive.CoverLeftText3.Replace("弹幕", string.Empty).Trim();
            Source = archive;
            LimitCover(archive.Cover);

            if (archive.IsPGC)
            {
                VideoType = Models.Enums.VideoType.Pgc;
                var episodeId = new Uri(archive.Uri).Segments.Last().Replace("ep", string.Empty);
                VideoId = episodeId;
            }
            else
            {
                VideoType = Models.Enums.VideoType.Video;
                VideoId = archive.Avid.ToString();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="video">稍后再看视频信息.</param>
        public VideoViewModel(ViewLaterVideo video)
            : this()
        {
            Title = video.Title ?? string.Empty;
            Publisher = new UserViewModel(video.Publisher);
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.StatusInfo.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.StatusInfo.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.StatusInfo.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.StatusInfo.LikeCount);
            VideoId = video.VideoId.ToString();
            PartitionName = video.PartitionName;
            PartitionId = video.PartitionId;
            Source = video;
            CanShowAvatar = !string.IsNullOrEmpty(Publisher.Avatar);
            LimitCover(video.Cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="video">排行榜视频.</param>
        public VideoViewModel(Item video)
            : this()
        {
            Title = video.Title ?? string.Empty;
            Publisher = new UserViewModel(video.Name, video.Face, Convert.ToInt32(video.Mid));
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.Play);
            ReplyCount = _numberToolkit.GetCountText(video.Reply);
            DanmakuCount = _numberToolkit.GetCountText(video.Danmaku);
            LikeCount = _numberToolkit.GetCountText(video.Like);
            VideoId = video.Param;
            PartitionName = video.Rname;
            Source = video;
            PartitionId = video.Rid;
            AdditionalText = video.Pts.ToString();
            CanShowAvatar = true;
            LimitCover(video.Cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="card">推荐视频卡片.</param>
        public VideoViewModel(RecommendCard card)
            : this()
        {
            Title = card.Title ?? string.Empty;
            VideoId = card.Parameter;
            PlayCount = card.PlayCountText;
            if (card.CardGoto == ServiceConstants.Av)
            {
                // 视频处理.
                DanmakuCount = card.SubStatusText;
                LikeCount = string.Empty;
                Publisher = new UserViewModel(card.Mask.Avatar);
                Duration = (card.PlayerArgs?.Duration).HasValue
                    ? _numberToolkit.GetDurationText(TimeSpan.FromSeconds((double)card.PlayerArgs?.Duration))
                    : _numberToolkit.FormatDurationText(card.DurationText);

                PartitionId = card.CardArgs.PartitionId;
                PartitionName = card.CardArgs.PartitionName;
                CanShowAvatar = true;
            }
            else
            {
                // 动漫处理.
                LikeCount = card.SubStatusText;
                DanmakuCount = string.Empty;
                Duration = "--";
                VideoType = Models.Enums.VideoType.Pgc;
            }

            Description = card.Description;
            IsShowDescription = !string.IsNullOrEmpty(Description);
            AdditionalText = card.RecommendReason ?? string.Empty;
            Source = card;
            LimitCover(card.Cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="card">视频卡片.</param>
        public VideoViewModel(Card card)
            : this()
        {
            var v5 = card.SmallCoverV5;
            var cardBase = v5.Base;
            Title = cardBase.Title;
            VideoId = cardBase.Param;
            PlayCount = v5.RightDesc2;
            Publisher = new UserViewModel(v5.RightDesc1);
            AdditionalText = v5.RcmdReasonStyle?.Text ?? string.Empty;
            Duration = _numberToolkit.FormatDurationText(v5.CoverRightText1);
            LimitCover(cardBase.Cover);
            Source = card;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="followRoom">关注的直播间.</param>
        public VideoViewModel(LiveFeedRoom followRoom)
            : this()
        {
            Title = followRoom.Title;
            VideoId = followRoom.RoomId.ToString();
            ViewerCount = _numberToolkit.GetCountText(followRoom.ViewerCount);
            Publisher = new UserViewModel(followRoom.UserName, followRoom.UserAvatar, followRoom.UserId);
            PartitionName = followRoom.DisplayAreaName;
            PartitionId = Convert.ToInt32(followRoom.DisplayAreaId);
            LimitCover(followRoom.Cover);
            Source = followRoom;
            VideoType = Models.Enums.VideoType.Live;
            CanShowAvatar = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="card">直播间卡片.</param>
        public VideoViewModel(LiveRoomCard card)
            : this()
        {
            Title = card.Title;
            VideoId = card.RoomId.ToString();
            ViewerCount = card.CoverRightContent.Text;
            Publisher = new UserViewModel(card.CoverLeftContent.Text);
            PartitionName = card.AreaName;
            PartitionId = Convert.ToInt32(card.AreaId);
            LimitCover(card.Cover);
            Source = card;
            VideoType = Models.Enums.VideoType.Live;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="relate">相关视频推荐.</param>
        public VideoViewModel(Relate relate)
            : this()
        {
            Title = relate.Title;
            VideoId = relate.Aid.ToString();
            PlayCount = _numberToolkit.GetCountText(relate.Stat.View);
            DanmakuCount = _numberToolkit.GetCountText(relate.Stat.Danmaku);
            LikeCount = _numberToolkit.GetCountText(relate.Stat.Like);
            ReplyCount = _numberToolkit.GetCountText(relate.Stat.Reply);
            var author = relate.Author;
            Publisher = new UserViewModel(author.Name, author.Face, Convert.ToInt32(author.Mid));
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(relate.Duration));
            AdditionalText = relate.Rating.ToString();
            LimitCover(relate.Pic);
            Source = relate;
            IsRelated = true;
            VideoType = relate.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase) ?
                Models.Enums.VideoType.Video : Models.Enums.VideoType.Pgc;
            CanShowAvatar = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="item">视频搜索结果.</param>
        public VideoViewModel(VideoSearchItem item)
            : this()
        {
            VideoType = Models.Enums.VideoType.Video;
            Title = item.Title;
            VideoId = item.Parameter;
            PlayCount = _numberToolkit.GetCountText(item.PlayCount);
            DanmakuCount = _numberToolkit.GetCountText(item.DanmakuCount);
            Publisher = new UserViewModel(item.Author, item.Avatar, item.UserId);
            Duration = _numberToolkit.FormatDurationText(item.Duration);
            LimitCover(item.Cover);
            Source = item;
            CanShowAvatar = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="item">直播搜索结果.</param>
        public VideoViewModel(LiveSearchItem item)
            : this()
        {
            VideoType = Models.Enums.VideoType.Live;
            Title = item.Title;
            VideoId = item.RoomId.ToString();
            Publisher = new UserViewModel(item.Name, userId: item.UserId);
            ViewerCount = _numberToolkit.GetCountText(item.ViewerCount);
            PartitionName = item.AreaName;
            LimitCover(item.Cover);
            Source = item;
            VideoType = Models.Enums.VideoType.Live;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="item">用户空间视频条目.</param>
        public VideoViewModel(UserSpaceVideoItem item)
            : this()
        {
            VideoType = item.IsPgc ? Models.Enums.VideoType.Pgc : Models.Enums.VideoType.Video;
            Title = item.Title;
            VideoId = item.Id;
            PartitionName = item.PartitionName;
            PlayCount = _numberToolkit.GetCountText(item.PlayCount);
            DanmakuCount = _numberToolkit.GetCountText(item.DanmakuCount);
            Publisher = new UserViewModel(item.PublisherName);
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(item.Duration));
            LimitCover(item.Cover);
            Source = item;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="item">历史记录条目.</param>
        public VideoViewModel(CursorItem item)
            : this()
        {
            VideoType = item.CardItemCase == CursorItem.CardItemOneofCase.CardUgc ? Models.Enums.VideoType.Video : Models.Enums.VideoType.Pgc;
            Title = item.Title;
            VideoId = item.Kid.ToString();
            if (VideoType == Models.Enums.VideoType.Video)
            {
                var video = item.CardUgc;
                PlayCount = _numberToolkit.GetCountText(video.View);
                Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
                Publisher = new UserViewModel(video.Name, userId: Convert.ToInt32(video.Mid));
                LimitCover(video.Cover);
            }
            else if (VideoType == Models.Enums.VideoType.Pgc)
            {
                var pgc = item.CardOgv;
                var uri = new Uri(item.Uri);
                var episodeId = uri.Segments.Where(p => p.Contains("ep")).FirstOrDefault()?.Replace("ep", string.Empty);
                if (!string.IsNullOrEmpty(episodeId))
                {
                    VideoId = episodeId;
                }

                PlayCount = "--";
                Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(pgc.Duration));
                Publisher = new UserViewModel("--");
                LimitCover(pgc.Cover);
            }

            Source = item;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="media">收藏夹媒体.</param>
        public VideoViewModel(FavoriteMedia media)
            : this()
        {
            Title = media.Title;
            VideoId = media.Id.ToString();
            PlayCount = _numberToolkit.GetCountText(media.Stat.PlayCount);
            DanmakuCount = _numberToolkit.GetCountText(media.Stat.DanmakuCount);
            Publisher = new UserViewModel(media.Publisher);
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(media.Duration));
            LimitCover(media.Cover);
            Source = media;
            CanShowAvatar = !string.IsNullOrEmpty(Publisher.Avatar);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="arc">视频条目.</param>
        public VideoViewModel(Arc arc)
            : this()
        {
            var item = arc.Archive;
            VideoType = Models.Enums.VideoType.Video;
            Title = Regex.Replace(item.Title, "<[^>]+>", string.Empty);
            VideoId = item.Aid.ToString();
            PartitionName = item.TypeName;
            PlayCount = _numberToolkit.GetCountText(item.Stat.View);
            DanmakuCount = _numberToolkit.GetCountText(item.Stat.Danmaku);
            Publisher = new UserViewModel(item.Author.Name, item.Author.Face, Convert.ToInt32(item.Author.Mid));
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(item.Duration));
            LimitCover(item.Pic);
            Source = item;
            CanShowAvatar = true;
        }

        internal VideoViewModel()
        {
            ServiceLocator.Instance.LoadService(out _numberToolkit);
            VideoType = Models.Enums.VideoType.Video;
        }

        /// <summary>
        /// 限制图片分辨率以减轻UI和内存压力.
        /// </summary>
        private void LimitCover(string coverUrl)
        {
            SourceCoverUrl = coverUrl;
            CoverUrl = coverUrl + "@400w_250h_1c_100q.jpg";
        }
    }
}
