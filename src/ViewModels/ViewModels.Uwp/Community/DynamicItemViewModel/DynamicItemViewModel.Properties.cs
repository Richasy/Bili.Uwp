// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Dynamic;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
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
        private readonly ICallerViewModel _callerViewModel;
        private readonly INavigationViewModel _navigationViewModel;

        /// <inheritdoc/>
        [Reactive]
        public IUserItemViewModel Publisher { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public DynamicInformation Data { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsLiked { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string CommentCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowCommunity { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool CanAddViewLater { get; set; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ToggleLikeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ActiveCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ShowUserDetailCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> AddToViewLaterCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ShowCommentDetailCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ShareCommand { get; }
    }
}
