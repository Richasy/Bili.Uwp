// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.Dynamic;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Community
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

        [ObservableProperty]
        private IUserItemViewModel _publisher;

        [ObservableProperty]
        private DynamicInformation _data;

        [ObservableProperty]
        private bool _isLiked;

        [ObservableProperty]
        private string _likeCountText;

        [ObservableProperty]
        private string _commentCountText;

        [ObservableProperty]
        private bool _isShowCommunity;

        [ObservableProperty]
        private bool _canAddViewLater;

        /// <inheritdoc/>
        public IAsyncRelayCommand ToggleLikeCommand { get; }

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
