// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.Uwp.App.Controls
{
    /// <summary>
    /// 用于ScrollViewer中的偏移按钮.
    /// </summary>
    public class OffsetButton : Button
    {
        /// <summary>
        /// 图标的依赖属性.
        /// </summary>
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(OffsetButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes a new instance of the <see cref="OffsetButton"/> class.
        /// </summary>
        public OffsetButton()
        {
            DefaultStyleKey = typeof(OffsetButton);
        }

        /// <summary>
        /// 按钮图标.
        /// </summary>
        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }
    }
}
