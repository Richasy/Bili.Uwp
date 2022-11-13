// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 主评论模块视图模型.
    /// </summary>
    public sealed partial class CommentMainModuleViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ICallerViewModel _callerViewModel;
        private bool _isEnd;
        private CommentType _commentType;
        private string _targetId;
        private ICommentItemViewModel _selectedComment;

        [ObservableProperty]
        private ICommentItemViewModel _topComment;

        [ObservableProperty]
        private bool _isEmpty;

        [ObservableProperty]
        private CommentSortHeader _currentSort;

        [ObservableProperty]
        private string _replyTip;

        [ObservableProperty]
        private string _replyText;

        [ObservableProperty]
        private bool _isSending;

        /// <inheritdoc/>
        public event EventHandler<ICommentItemViewModel> RequestShowDetail;

        /// <inheritdoc/>
        public ObservableCollection<CommentSortHeader> SortCollection { get; }

        /// <inheritdoc/>
        public IRelayCommand<CommentSortHeader> ChangeSortCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ResetSelectedCommentCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand SendCommentCommand { get; }
    }
}
