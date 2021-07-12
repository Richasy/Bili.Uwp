// Copyright (c) Richasy. All rights reserved.

using System;
using Bilibili.App.Card.V1;
using Bilibili.App.Show.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

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
            PublisherName = video.Publisher ?? "--";
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.LikeCount);
            VideoId = video.Parameter;
            PartitionName = video.PartitionName;
            PartitionId = video.PartitionId;
            Source = video;
            LimitCoverAndAvatar(video.Cover, video.PublisherAvatar);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="video">排行榜视频.</param>
        public VideoViewModel(RankItem video)
            : this()
        {
            Title = video.Title ?? string.Empty;
            PublisherName = video.Name ?? "--";
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
            LimitCoverAndAvatar(video.Cover, video.Face);
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
                PublisherName = card.CardArgs.PublisherName ?? "--";
                if ((card.PlayerArgs?.Duration).HasValue)
                {
                    Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds((double)card.PlayerArgs?.Duration));
                }
                else
                {
                    Duration = _numberToolkit.FormatDurationText(card.DurationText);
                }

                PartitionId = card.CardArgs.PartitionId;
                PartitionName = card.CardArgs.PartitionName;
            }
            else
            {
                // 动漫处理.
                LikeCount = card.SubStatusText;
                DanmakuCount = string.Empty;
                PublisherName = card.Description?.Text ?? "--";
            }

            AdditionalText = card.RecommendReason ?? string.Empty;
            Source = card;
            LimitCoverAndAvatar(card.Cover);
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
            PublisherName = v5.RightDesc1;
            AdditionalText = v5.RcmdReasonStyle?.Text ?? string.Empty;
            Duration = _numberToolkit.FormatDurationText(v5.CoverRightText1);
            LimitCoverAndAvatar(cardBase.Cover);
            Source = card;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="followRoom">关注的直播间.</param>
        public VideoViewModel(LiveFeedFollowRoom followRoom)
            : this()
        {
            Title = followRoom.Title;
            VideoId = followRoom.RoomId.ToString();
            ViewerCount = _numberToolkit.GetCountText(followRoom.ViewerCount);
            PublisherName = followRoom.UserName;
            PartitionName = followRoom.DisplayAreaName;
            PartitionId = Convert.ToInt32(followRoom.DisplayAreaId);
            AdditionalText = $"开始于:{DateTimeOffset.FromUnixTimeSeconds(followRoom.LiveStartTime):HH:mm}";
            LimitCoverAndAvatar(followRoom.Cover, followRoom.UserAvatar);
            Source = followRoom;
            VideoType = VideoType.Live;
        }

        internal VideoViewModel()
        {
            ServiceLocator.Instance.LoadService(out _numberToolkit);
            VideoType = VideoType.Video;
        }

        /// <summary>
        /// 限制图片分辨率以减轻UI和内存压力.
        /// </summary>
        private void LimitCoverAndAvatar(string coverUrl, string avatarUrl = null)
        {
            SourceCoverUrl = coverUrl;
            CoverUrl = coverUrl + "@400w_250h_1c_100q.jpg";
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                PublisherAvatar = avatarUrl + "@60w_60h_1c_100q.jpg";
            }
        }
    }
}
