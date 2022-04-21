// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bilibili.App.Dynamic.V2;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 图文动态条目.
    /// </summary>
    public sealed partial class DynamicImageItem : UserControl
    {
        /// <summary>
        /// <see cref="Data"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(MdlDynDraw), typeof(DynamicImageItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicImageItem"/> class.
        /// </summary>
        public DynamicImageItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据.
        /// </summary>
        public MdlDynDraw Data
        {
            get { return (MdlDynDraw)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MdlDynDraw data)
            {
                var instance = d as DynamicImageItem;
                instance.ImageRepeater.ItemsSource = data.Items.ToList();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var imageCount = Data.Items.Count;
            var columnCount = e.NewSize.Width / 100;
            var lineCount = Math.Ceiling(imageCount * 1.0 / columnCount);
            var height = (lineCount * 100) + ((lineCount - 1) * 4);
            ImageRepeater.Height = height;
        }

        private void OnImageTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var image = sender as CommonImageEx;
            var sources = Data.Items.Select(p => p.Src).ToList();
            var index = sources.IndexOf(image.DataContext as string);
            AppViewModel.Instance.ShowImages(sources, index);
        }
    }
}
