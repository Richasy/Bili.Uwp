// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.Desktop.App.Controls.Community
{
    /// <summary>
    /// 评论区页面视图.
    /// </summary>
    public sealed class CommentPageView : ReactiveControl<ICommentPageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentPageView"/> class.
        /// </summary>
        public CommentPageView()
        {
            DefaultStyleKey = typeof(CommentPageView);
            ViewModel = Locator.Instance.GetService<ICommentPageViewModel>();
        }
    }
}
