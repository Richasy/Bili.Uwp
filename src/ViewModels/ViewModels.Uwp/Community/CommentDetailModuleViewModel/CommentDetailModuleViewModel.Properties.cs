// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        private bool _isEnd;
        private ICommentItemViewModel _selectedComment;

        [ObservableProperty]
        private ICommentItemViewModel _rootComment;

        [ObservableProperty]
        private bool _isEmpty;

        [ObservableProperty]
        private string _replyTip;

        [ObservableProperty]
        private string _replyText;

        [ObservableProperty]
        private bool _isSending;

        /// <summary>
        /// 请求返回到主评论视图.
        /// </summary>
        public event EventHandler RequestBackToMain;

        /// <summary>
        /// 重置选中的评论.
        /// </summary>
        public IRelayCommand ResetSelectedCommentCommand { get; }

        /// <summary>
        /// 发送评论命令.
        /// </summary>
        public IAsyncRelayCommand SendCommentCommand { get; }

        /// <summary>
        /// 返回到上一层（主视图）的命令.
        /// </summary>
        public IRelayCommand BackCommand { get; }
    }
}
