// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Community;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 评论区详情视图.
    /// </summary>
    public sealed class CommentDetailView : ReactiveControl<CommentDetailModuleViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDetailView"/> class.
        /// </summary>
        public CommentDetailView() => DefaultStyleKey = typeof(CommentDetailView);
    }
}
