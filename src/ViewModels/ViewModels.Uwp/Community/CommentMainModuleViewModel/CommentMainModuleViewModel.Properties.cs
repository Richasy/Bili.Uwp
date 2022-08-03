// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
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

        /// <inheritdoc/>
        public event EventHandler<ICommentItemViewModel> RequestShowDetail;

        /// <inheritdoc/>
        public ObservableCollection<CommentSortHeader> SortCollection { get; }

        /// <inheritdoc/>
        public ReactiveCommand<CommentSortHeader, Unit> ChangeSortCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ResetSelectedCommentCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> SendCommentCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public ICommentItemViewModel TopComment { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsEmpty { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public CommentSortHeader CurrentSort { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ReplyTip { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ReplyText { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsSending { get; set; }
    }
}
