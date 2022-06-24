// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Community;
using Splat;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 评论区页面视图.
    /// </summary>
    public sealed class CommentPageView : ReactiveControl<CommentPageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentPageView"/> class.
        /// </summary>
        public CommentPageView()
        {
            DefaultStyleKey = typeof(CommentPageView);
            ViewModel = Splat.Locator.Current.GetService<CommentPageViewModel>();
        }
    }
}
