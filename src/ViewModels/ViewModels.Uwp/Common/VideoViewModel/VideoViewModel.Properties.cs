// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频视图模型属性集.
    /// </summary>
    public partial class VideoViewModel
    {
        private readonly INumberToolkit _numberToolkit;

        /// <summary>
        /// 分区Id.
        /// </summary>
        public int PartitionId { get; }

        /// <summary>
        /// 视频标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        [Reactive]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 视频时长.
        /// </summary>
        [Reactive]
        public string Duration { get; set; }

        /// <summary>
        /// 播放数.
        /// </summary>
        [Reactive]
        public string PlayCount { get; set; }

        /// <summary>
        /// 评论数.
        /// </summary>
        [Reactive]
        public string ReplyCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [Reactive]
        public string DanmakuCount { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        [Reactive]
        public string LikeCount { get; set; }

        /// <summary>
        /// 观看人数.
        /// </summary>
        [Reactive]
        public string ViewerCount { get; set; }

        /// <summary>
        /// 发布者.
        /// </summary>
        [Reactive]
        public PublisherViewModel Publisher { get; set; }

        /// <summary>
        /// 分区名称.
        /// </summary>
        [Reactive]
        public string PartitionName { get; set; }

        /// <summary>
        /// 附加文本.
        /// </summary>
        [Reactive]
        public string AdditionalText { get; set; }

        /// <summary>
        /// 视频类型.
        /// </summary>
        public VideoType VideoType { get; set; }

        /// <summary>
        /// 生成视图模型的源数据.
        /// </summary>
        public object Source { get; private set; }

        /// <summary>
        /// 视频Id.
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// 原始封面地址.
        /// </summary>
        public string SourceCoverUrl { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoViewModel model && VideoId == model.VideoId;

        /// <inheritdoc/>
        public override int GetHashCode() => -593024695 + EqualityComparer<string>.Default.GetHashCode(VideoId);
    }
}
