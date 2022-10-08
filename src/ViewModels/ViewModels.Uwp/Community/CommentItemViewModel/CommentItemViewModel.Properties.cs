// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
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
        private Action<ICommentItemViewModel> _showCommentDetailAction;
        private Action<ICommentItemViewModel> _clickAction;

        /// <inheritdoc/>
        public IRelayCommand ToggleLikeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowCommentDetailCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowUserDetailCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClickCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public CommentInformation Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsLiked { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ReplyCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string PublishDateText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsUserHighlight { get; set; }
    }
}
