// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 显示额外省略号的文本控件.
    /// </summary>
    public sealed partial class TrimTextBlock : UserControl
    {
        /// <summary>
        /// <see cref="Text"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TrimTextBlock), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="MaxLines"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty =
            DependencyProperty.Register(nameof(MaxLines), typeof(int), typeof(TrimTextBlock), new PropertyMetadata(6));

        /// <summary>
        /// Initializes a new instance of the <see cref="TrimTextBlock"/> class.
        /// </summary>
        public TrimTextBlock()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 文本.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 最大行数.
        /// </summary>
        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        private void OnIsTextTrimmedChanged(TextBlock sender, IsTextTrimmedChangedEventArgs args)
        {
            if (sender.IsTextTrimmed)
            {
                OverflowButton.Visibility = Visibility.Visible;
            }
            else
            {
                OverflowButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
