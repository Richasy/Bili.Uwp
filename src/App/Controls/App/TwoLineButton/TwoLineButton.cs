// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 包含双行文本的按钮.
    /// </summary>
    public sealed class TwoLineButton : Button
    {
        /// <summary>
        /// <see cref="FirstLineText"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty FirstLineTextProperty =
            DependencyProperty.Register(nameof(FirstLineText), typeof(string), typeof(TwoLineButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="SecondLineText"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SecondLineTextProperty =
            DependencyProperty.Register(nameof(SecondLineText), typeof(string), typeof(TwoLineButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoLineButton"/> class.
        /// </summary>
        public TwoLineButton()
        {
            DefaultStyleKey = typeof(TwoLineButton);
        }

        /// <summary>
        /// 首行文本.
        /// </summary>
        public string FirstLineText
        {
            get { return (string)GetValue(FirstLineTextProperty); }
            set { SetValue(FirstLineTextProperty, value); }
        }

        /// <summary>
        /// 次行文本.
        /// </summary>
        public string SecondLineText
        {
            get { return (string)GetValue(SecondLineTextProperty); }
            set { SetValue(SecondLineTextProperty, value); }
        }
    }
}
