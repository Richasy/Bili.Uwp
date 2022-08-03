// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Uwp.Community;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 评论列表.
    /// </summary>
    public sealed partial class CommentRepeater : UserControl
    {
        /// <summary>
        /// <see cref="ItemsSource"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(CommentRepeater), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="TopComment"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TopCommentProperty =
            DependencyProperty.Register(nameof(TopComment), typeof(ICommentItemViewModel), typeof(CommentRepeater), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRepeater"/> class.
        /// </summary>
        public CommentRepeater() => InitializeComponent();

        /// <summary>
        /// 数据源.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// 置顶评论.
        /// </summary>
        public ICommentItemViewModel TopComment
        {
            get { return (ICommentItemViewModel)GetValue(TopCommentProperty); }
            set { SetValue(TopCommentProperty, value); }
        }
    }
}
