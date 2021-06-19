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
            CoverUrl = video.Cover;
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.LikeCount);
            VideoId = video.Parameter;
            PublisherAvatar = video.PublisherAvatar;
            PartitionName = video.PartitionName;
            Source = video;
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
            PublisherAvatar = video.PublisherInfo.PublisherAvatar;
            CoverUrl = video.Cover;
            Duration = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(video.Duration));
            PlayCount = _numberToolkit.GetCountText(video.Status.PlayCount);
            ReplyCount = _numberToolkit.GetCountText(video.Status.ReplyCount);
            DanmakuCount = _numberToolkit.GetCountText(video.Status.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(video.Status.LikeCount);
            VideoId = video.Aid.ToString();
            PartitionName = video.PartitionName;
            Source = video;
        }

        internal VideoViewModel()
        {
            ServiceLocator.Instance.LoadService(out _numberToolkit);
        }
    }
}
