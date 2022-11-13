// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;

namespace Bili.Uwp.App.Controls.Community
{
    /// <summary>
    /// 评论区详情视图.
    /// </summary>
    public sealed class CommentDetailView : ReactiveControl<ICommentDetailModuleViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDetailView"/> class.
        /// </summary>
        public CommentDetailView() => DefaultStyleKey = typeof(CommentDetailView);
    }
}
