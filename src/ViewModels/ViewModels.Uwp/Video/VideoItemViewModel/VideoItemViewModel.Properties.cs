// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频条目视图模型.
    /// </summary>
    public sealed partial class VideoItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IAccountProvider _accountProvider;
        private readonly IAuthorizeProvider _authorizeProvider;

        /// <summary>
        /// 添加到稍后再看的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddToViewLaterCommand { get; }

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> PlayCommand { get; }

        /// <summary>
        /// 视频信息.
        /// </summary>
        [Reactive]
        public VideoInformation Information { get; internal set; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        [Reactive]
        public string PlayCountText { get; internal set; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        [Reactive]
        public string DanmakuCountText { get; internal set; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        [Reactive]
        public string LikeCountText { get; internal set; }

        /// <summary>
        /// 视频时长.
        /// </summary>
        [Reactive]
        public string DurationText { get; internal set; }

        /// <summary>
        /// 是否显示评分.
        /// </summary>
        [Reactive]
        public bool IsShowScore { get; internal set; }

        /// <summary>
        /// 是否显示时长.
        /// </summary>
        [Reactive]
        public bool IsShowDuration { get; internal set; }

        /// <summary>
        /// 是否显示徽章内容.
        /// </summary>
        [Reactive]
        public bool IsShowBadge { get; internal set; }

        /// <summary>
        /// 是否显示头像.
        /// </summary>
        [Reactive]
        public bool IsShowAvatar { get; internal set; }

        /// <summary>
        /// 是否被选中.
        /// </summary>
        [Reactive]
        public bool IsSelected { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoItemViewModel model && EqualityComparer<VideoInformation>.Default.Equals(Information, model.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}
