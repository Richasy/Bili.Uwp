// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums.Workspace;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Workspace.Controls
{
    /// <summary>
    /// 应用使用的线性图标.
    /// </summary>
    public sealed class FluentIcon : FontIcon
    {
        /// <summary>
        /// <see cref="Symbol"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(Symbol), typeof(FluentSymbol), typeof(FluentIcon), new PropertyMetadata(default, OnSymbolChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentIcon"/> class.
        /// </summary>
        public FluentIcon()
            => FontFamily = new Microsoft.UI.Xaml.Media.FontFamily("ms-appx:///Assets/FluentIcon.ttf#FluentSystemIcons-Resizable");

        /// <summary>
        /// 图标.
        /// </summary>
        public FluentSymbol Symbol
        {
            get => (FluentSymbol)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is FluentSymbol symbol)
            {
                var icon = d as FluentIcon;
                icon.Glyph = ((char)symbol).ToString();
            }
        }
    }
}
