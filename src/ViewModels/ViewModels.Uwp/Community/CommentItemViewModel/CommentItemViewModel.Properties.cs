// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        private Action<ICommentItemViewModel> _showCommentDetailAction;
        private Action<ICommentItemViewModel> _clickAction;

        [ObservableProperty]
        private CommentInformation _data;

        [ObservableProperty]
        private bool _isLiked;

        [ObservableProperty]
        private string _likeCountText;

        [ObservableProperty]
        private string _replyCountText;

        [ObservableProperty]
        private string _publishDateText;

        [ObservableProperty]
        private bool _isUserHighlight;

        /// <inheritdoc/>
        public IAsyncRelayCommand ToggleLikeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowCommentDetailCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowUserDetailCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClickCommand { get; }
    }
}
