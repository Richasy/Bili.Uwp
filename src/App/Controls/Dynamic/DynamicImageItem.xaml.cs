// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Dynamic
{
    /// <summary>
    /// 图文动态条目.
    /// </summary>
    public sealed partial class DynamicImageItem : UserControl
    {
        /// <summary>
        /// <see cref="ItemsSource"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(DynamicImageItem), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicImageItem"/> class.
        /// </summary>
        public DynamicImageItem() => InitializeComponent();

        /// <summary>
        /// 数据.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var images = ItemsSource as List<Models.Data.Appearance.Image>;
            var imageCount = images.Count();
            var columnCount = e.NewSize.Width / 100;
            var lineCount = Math.Ceiling(imageCount * 1.0 / columnCount);
            var height = (lineCount * 100) + ((lineCount - 1) * 4);
            ImageRepeater.Height = height;
        }

        private void OnImageTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var image = sender as CommonImageEx;
            var sources = ItemsSource as List<Models.Data.Appearance.Image>;
            var index = sources.ToList().IndexOf(image.DataContext as Models.Data.Appearance.Image);
            Splat.Locator.Current.GetService<AppViewModel>().ShowImages(sources, index);
        }
    }
}
