// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.App.Controls
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
            DependencyProperty.Register(nameof(ImageUrl), typeof(string), typeof(CommonImageEx), new PropertyMetadata(null, new PropertyChangedCallback(OnImageUrlChanged)));

        /// <summary>
        /// <see cref="Stretch"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(CommonImageEx), new PropertyMetadata(Stretch.UniformToFill));

        /// <summary>
        /// <see cref="DecodePixelWidth"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DecodePixelWidthProperty =
            DependencyProperty.Register(nameof(DecodePixelWidth), typeof(double), typeof(CommonImageEx), new PropertyMetadata(-1));

        private Image _image;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonImageEx"/> class.
        /// </summary>
        public CommonImageEx()
        {
            DefaultStyleKey = typeof(CommonImageEx);
            Loaded += OnLoaded;
            Unloaded += OnUnload;
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

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
            => _image = GetTemplateChild("Image") as Image;

        private static void OnImageUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CommonImageEx;
            if (e.NewValue != null && e.OldValue != null && e.NewValue != e.OldValue)
            {
                instance.LoadImage();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }

        private void OnUnload(object sender, RoutedEventArgs e)
        {
            if (_image != null)
            {
                _image.Source = null;
            }
        }

        private void LoadImage()
        {
            if (_image == null || string.IsNullOrEmpty(ImageUrl))
            {
                return;
            }

            _image.Source = null;
            var bitmapImage = new BitmapImage();
            if (DecodePixelWidth != -1)
            {
                bitmapImage.DecodePixelWidth = DecodePixelWidth;
            }

            _image.Source = bitmapImage;
            var url = ImageUrl;
            bitmapImage.UriSource = new Uri(url);
        }
    }
}
