// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Dynamic;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 动态条目视图模型.
    /// </summary>
    public sealed partial class DynamicItemViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly INumberToolkit _numberToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly AppViewModel _appViewModel;
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// 用户信息.
        /// </summary>
        [Reactive]
        public UserItemViewModel Publisher { get; set; }

        /// <summary>
        /// 信息.
        /// </summary>
        [Reactive]
        public DynamicInformation Information { get; set; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        [Reactive]
        public bool IsLiked { get; set; }

        /// <summary>
        /// 点赞次数的可读文本.
        /// </summary>
        [Reactive]
        public string LikeCountText { get; set; }

        /// <summary>
        /// 评论次数文本.
        /// </summary>
        [Reactive]
        public string CommentCountText { get; set; }

        /// <summary>
        /// 是否显示社区信息.
        /// </summary>
        [Reactive]
        public bool IsShowCommunity { get; set; }

        /// <summary>
        /// 是否可以加入稍后再看.
        /// </summary>
        [Reactive]
        public bool CanAddViewLater { get; set; }

        /// <summary>
        /// 点赞动态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleLikeCommand { get; }

        /// <summary>
        /// 点击动态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ActiveCommand { get; }

        /// <summary>
        /// 显示用户详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowUserDetailCommand { get; }

        /// <summary>
        /// 添加到稍后再看的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddToViewLaterCommand { get; }

        /// <summary>
        /// 显示评论详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowCommentDetailCommand { get; }

        /// <summary>
        /// 显示评论详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShareCommand { get; }
    }
}
