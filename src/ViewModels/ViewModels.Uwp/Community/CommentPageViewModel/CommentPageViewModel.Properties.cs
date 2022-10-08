// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 评论页面/模块视图模型.
    /// </summary>
    public sealed partial class CommentPageViewModel
    {
        /// <inheritdoc/>
        public ICommentMainModuleViewModel MainViewModel { get; }

        /// <inheritdoc/>
        public ICommentDetailModuleViewModel DetailViewModel { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsMainShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsDetailShown { get; set; }
    }
}
