// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 流畅图标.
    /// </summary>
    public sealed class FluentIcon : FontIcon
    {
        /// <summary>
        /// Dependency property of <see cref="Symbol"/>.
        /// </summary>
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(Symbol), typeof(FluentIcons.Common.Symbol), typeof(FluentIcon), new PropertyMetadata(default, new PropertyChangedCallback(OnSymbolChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentIcon"/> class.
        /// </summary>
        public FluentIcon()
            => FontFamily = new FontFamily("ms-appx:///Assets/FluentIcon.ttf#FluentSystemIcons-Resizable");

        /// <summary>
        /// Symbol enumeration value corresponding to the icon.
        /// </summary>
        public FluentIcons.Common.Symbol Symbol
        {
            get { return (FluentIcons.Common.Symbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is FluentIcons.Common.Symbol symbol)
            {
                var instance = d as FluentIcon;
                instance.Glyph = ((char)symbol).ToString();
            }
        }
    }
}
