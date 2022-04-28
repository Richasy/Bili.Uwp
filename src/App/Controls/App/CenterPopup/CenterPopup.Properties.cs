// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 居中显示的弹出层.
    /// </summary>
    public partial class CenterPopup
    {
        /// <summary>
        /// <see cref="Title"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(CenterPopup), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="PresenterMaxWidth"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty PresenterMaxWidthProperty =
            DependencyProperty.Register(nameof(PresenterMaxWidth), typeof(double), typeof(CenterPopup), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="PresenterVerticalAlignment"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty PresenterVerticalAlignmentProperty =
            DependencyProperty.Register(nameof(PresenterVerticalAlignment), typeof(VerticalAlignment), typeof(CenterPopup), new PropertyMetadata(VerticalAlignment.Center));

        private const string CloseButtonName = "CloseButton";

        private Button _closeButton;

        /// <summary>
        /// 弹窗关闭时发生.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 显示区域的最大宽度.
        /// </summary>
        public double PresenterMaxWidth
        {
            get { return (double)GetValue(PresenterMaxWidthProperty); }
            set { SetValue(PresenterMaxWidthProperty, value); }
        }

        /// <summary>
        /// 显示区域的垂直布局方式.
        /// </summary>
        public VerticalAlignment PresenterVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(PresenterVerticalAlignmentProperty); }
            set { SetValue(PresenterVerticalAlignmentProperty, value); }
        }
    }
}
