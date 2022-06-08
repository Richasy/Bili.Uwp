// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp.Community;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
