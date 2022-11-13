// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls
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
