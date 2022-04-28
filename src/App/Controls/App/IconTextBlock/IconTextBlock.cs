// Copyright (c) Richasy. All rights reserved.

using Richasy.FluentIcon.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 带图标的文本.
    /// </summary>
    public sealed class IconTextBlock : Control
    {
        /// <summary>
        /// <see cref="IconFontSize"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IconFontSizeProperty =
            DependencyProperty.Register(nameof(IconFontSize), typeof(double), typeof(IconTextBlock), new PropertyMetadata(14d));

        /// <summary>
        /// <see cref="Spacing"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(nameof(Spacing), typeof(double), typeof(IconTextBlock), new PropertyMetadata(8d));

        /// <summary>
        /// <see cref="Symbol"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(Symbol), typeof(RegularFluentSymbol), typeof(IconTextBlock), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="Text"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconTextBlock), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes a new instance of the <see cref="IconTextBlock"/> class.
        /// </summary>
        public IconTextBlock() => DefaultStyleKey = typeof(IconTextBlock);

        /// <summary>
        /// 图标字体大小.
        /// </summary>
        public double IconFontSize
        {
            get { return (double)GetValue(IconFontSizeProperty); }
            set { SetValue(IconFontSizeProperty, value); }
        }

        /// <summary>
        /// 图标与文本的间距.
        /// </summary>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// 图标.
        /// </summary>
        public RegularFluentSymbol Symbol
        {
            get { return (RegularFluentSymbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        /// <summary>
        /// 文本.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
