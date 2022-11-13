// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 评论页面/模块视图模型.
    /// </summary>
    public sealed partial class CommentPageViewModel
    {
        [ObservableProperty]
        private bool _isMainShown;

        [ObservableProperty]
        private bool _isDetailShown;

        /// <inheritdoc/>
        public ICommentMainModuleViewModel MainViewModel { get; }

        /// <inheritdoc/>
        public ICommentDetailModuleViewModel DetailViewModel { get; }
    }
}
