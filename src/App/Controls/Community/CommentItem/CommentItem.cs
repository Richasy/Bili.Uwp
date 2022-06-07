// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Community;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 评论条目.
    /// </summary>
    public sealed class CommentItem : ReactiveControl<CommentItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentItem"/> class.
        /// </summary>
        public CommentItem() => DefaultStyleKey = typeof(CommentItem);
    }
}
