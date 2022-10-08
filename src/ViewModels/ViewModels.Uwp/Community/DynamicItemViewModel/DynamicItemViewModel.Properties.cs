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
        [ObservableProperty]
        public IUserItemViewModel Publisher { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public DynamicInformation Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsLiked { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string CommentCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowCommunity { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool CanAddViewLater { get; set; }

        /// <inheritdoc/>
        public IRelayCommand ToggleLikeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ActiveCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowUserDetailCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand AddToViewLaterCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowCommentDetailCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShareCommand { get; }
    }
}
