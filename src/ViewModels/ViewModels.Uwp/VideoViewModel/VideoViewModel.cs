// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
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
            PublisherName = video.Publisher ?? "--";
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.LikeCount);
            VideoId = video.Parameter;
            PartitionName = video.PartitionName;
            Source = video;
            LimitCoverAndAvatar(video.Cover, video.PublisherAvatar);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoViewModel"/> class.
        /// </summary>
        /// <param name="video">排行榜视频.</param>
        public VideoViewModel(RankVideo video)
            : this()
        {
            Title = video.Title ?? string.Empty;
            PublisherName = video.PublisherInfo.Publisher;
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.Status.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.Status.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.Status.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.Status.LikeCount);
            VideoId = video.Aid.ToString();
            PartitionName = video.PartitionName;
            Source = video;
            LimitCoverAndAvatar(video.Cover, video.PublisherInfo.PublisherAvatar);
        }

        internal VideoViewModel()
        {
            ServiceLocator.Instance.LoadService(out _numberToolkit);
        }

        /// <summary>
        /// 限制图片分辨率以减轻UI和内存压力.
        /// </summary>
        private void LimitCoverAndAvatar(string coverUrl, string avatarUrl)
        {
            SourceCoverUrl = coverUrl;
            CoverUrl = coverUrl + "@400w_250h_1c_100q.jpg";
            PublisherAvatar = avatarUrl + "@60w_60h_1c_100q.jpg";
        }
    }
}
