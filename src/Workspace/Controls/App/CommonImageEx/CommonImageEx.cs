﻿// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Bili.Workspace.Controls.App
{
    /// <summary>
    /// 通用图片扩展.
    /// </summary>
    public sealed class CommonImageEx : Control
    {
        /// <summary>
        /// <see cref="ImageUrl"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(nameof(ImageUrl), typeof(string), typeof(CommonImageEx), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Stretch"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(CommonImageEx), new PropertyMetadata(Stretch.UniformToFill));

        /// <summary>
        /// <see cref="DecodePixelWidth"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DecodePixelWidthProperty =
            DependencyProperty.Register(nameof(DecodePixelWidth), typeof(double), typeof(CommonImageEx), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="PlaceholderSource"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty PlaceholderSourceProperty =
            DependencyProperty.Register(nameof(PlaceholderSource), typeof(ImageSource), typeof(CommonImageEx), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonImageEx"/> class.
        /// </summary>
        public CommonImageEx()
        {
            DefaultStyleKey = typeof(CommonImageEx);
        }

        /// <summary>
        /// 图片地址.
        /// </summary>
        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        /// <summary>
        /// 图片拉伸.
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// 横向解码宽度.
        /// </summary>
        public int DecodePixelWidth
        {
            get { return (int)GetValue(DecodePixelWidthProperty); }
            set { SetValue(DecodePixelWidthProperty, value); }
        }

        /// <summary>
        /// 占位符图片.
        /// </summary>
        public ImageSource PlaceholderSource
        {
            get { return (ImageSource)GetValue(PlaceholderSourceProperty); }
            set { SetValue(PlaceholderSourceProperty, value); }
        }
    }
}
