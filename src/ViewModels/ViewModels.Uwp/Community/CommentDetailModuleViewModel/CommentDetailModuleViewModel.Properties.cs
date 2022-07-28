// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 二级评论模块视图模型.
    /// </summary>
    public sealed partial class CommentDetailModuleViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ICallerViewModel _callerViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isSending;
        private bool _isEnd;
        private CommentItemViewModel _selectedComment;

        /// <summary>
        /// 请求返回到主评论视图.
        /// </summary>
        public event EventHandler RequestBackToMain;

        /// <summary>
        /// 重置选中的评论.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ResetSelectedCommentCommand { get; }

        /// <summary>
        /// 发送评论命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SendCommentCommand { get; }

        /// <summary>
        /// 返回到上一层（主视图）的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <summary>
        /// 根评论.
        /// </summary>
        [Reactive]
        public CommentItemViewModel RootComment { get; set; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }

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
