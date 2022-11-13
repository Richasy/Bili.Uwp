// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;
using Windows.Foundation;

namespace Bili.Uwp.App.Controls.Community
{
    /// <summary>
    /// 评论条目.
    /// </summary>
    public sealed class CommentItem : ReactiveControl<ICommentItemViewModel>, IRepeaterItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentItem"/> class.
        /// </summary>
        public CommentItem() => DefaultStyleKey = typeof(CommentItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(double.PositiveInfinity, 120);
    }
}
