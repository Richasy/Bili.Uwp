// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 评论条目视图模型.
    /// </summary>
    public sealed partial class CommentItemViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly INumberToolkit _numberToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ICallerViewModel _callerViewModel;
        private readonly INavigationViewModel _navigationViewModel;
        private Action<CommentItemViewModel> _showCommentDetailAction;
        private Action<CommentItemViewModel> _clickAction;

        /// <summary>
        /// 点赞或取消点赞的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleLikeCommand { get; }

        /// <summary>
        /// 显示评论详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowCommentDetailCommand { get; }

        /// <summary>
        /// 显示用户详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowUserDetailCommand { get; }

        /// <summary>
        /// 点击命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClickCommand { get; }

        /// <summary>
        /// 评论信息.
        /// </summary>
        [Reactive]
        public CommentInformation Information { get; set; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        [Reactive]
        public bool IsLiked { get; set; }

        /// <summary>
        /// 点赞数的可读文本.
        /// </summary>
        [Reactive]
        public string LikeCountText { get; set; }

        /// <summary>
        /// 回复数的可读文本.
        /// </summary>
        [Reactive]
        public string ReplyCountText { get; set; }

        /// <summary>
        /// 发布日期的可读文本.
        /// </summary>
        [Reactive]
        public string PublishDateText { get; set; }

        /// <summary>
        /// 是否用户高亮.
        /// </summary>
        [Reactive]
        public bool IsUserHighlight { get; set; }
    }
}
