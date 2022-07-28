// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
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
        private readonly ObservableAsPropertyHelper<bool> _isSending;
        private bool _isEnd;
        private CommentType _commentType;
        private string _targetId;
        private CommentItemViewModel _selectedComment;

        /// <summary>
        /// 请求显示某评论的详情.
        /// </summary>
        public event EventHandler<CommentItemViewModel> RequestShowDetail;

        /// <summary>
        /// 排序方式集合.
        /// </summary>
        public ObservableCollection<CommentSortHeader> SortCollection { get; }

        /// <summary>
        /// 改变排序方式的命令.
        /// </summary>
        public ReactiveCommand<CommentSortHeader, Unit> ChangeSortCommand { get; }

        /// <summary>
        /// 重置选中的评论.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ResetSelectedCommentCommand { get; }

        /// <summary>
        /// 发送评论命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SendCommentCommand { get; }

        /// <summary>
        /// 置顶评论.
        /// </summary>
        [Reactive]
        public CommentItemViewModel TopComment { get; set; }

        /// <summary>
        /// 内容是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }

        /// <summary>
        /// 当前的排序方式.
        /// </summary>
        [Reactive]
        public CommentSortHeader CurrentSort { get; set; }

        /// <summary>
        /// 回复框提示文本.
        /// </summary>
        [Reactive]
        public string ReplyTip { get; set; }

        /// <summary>
        /// 回复框的输入文本.
        /// </summary>
        [Reactive]
        public string ReplyText { get; set; }

        /// <summary>
        /// 是否正在发送评论.
        /// </summary>
        public bool IsSending => _isSending.Value;
    }
}
