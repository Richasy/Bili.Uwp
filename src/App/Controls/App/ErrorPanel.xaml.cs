// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 错误面板，用于显示指定的错误内容.
    /// </summary>
    public sealed partial class ErrorPanel : UserControl
    {
        /// <summary>
        /// <see cref="Text"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ErrorPanel), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="ActionContent"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ActionContentProperty =
            DependencyProperty.Register(nameof(ActionContent), typeof(string), typeof(ErrorPanel), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorPanel"/> class.
        /// </summary>
        public ErrorPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当用户点击指定的动作按钮时发生.
        /// </summary>
        public event RoutedEventHandler ActionButtonClick;

        /// <summary>
        /// 错误文本.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 按钮文本.
        /// </summary>
        public string ActionContent
        {
            get { return (string)GetValue(ActionContentProperty); }
            set { SetValue(ActionContentProperty, value); }
        }

        private void OnActionButtonClick(object sender, RoutedEventArgs e)
        {
            ActionButtonClick?.Invoke(sender, e);
        }
    }
}
