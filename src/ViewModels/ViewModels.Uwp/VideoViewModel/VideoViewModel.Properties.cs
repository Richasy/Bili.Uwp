// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
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
        /// 发布者.
        /// </summary>
        [Reactive]
        public string PublisherName { get; set; }

        /// <summary>
        /// 发布者头像.
        /// </summary>
        [Reactive]
        public string PublisherAvatar { get; set; }

        /// <summary>
        /// 分区名称.
        /// </summary>
        [Reactive]
        public string PartitionName { get; set; }

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
