// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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
    }
}
