// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 横幅条目.
    /// </summary>
    public sealed class BannerItem : Control
    {
        /// <summary>
        /// <see cref="Source"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(BannerItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Uri"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register(nameof(Uri), typeof(string), typeof(BannerItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Title"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BannerItem), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerItem"/> class.
        /// </summary>
        public BannerItem()
        {
            this.DefaultStyleKey = typeof(BannerItem);
        }

        /// <summary>
        /// 图片源.
        /// </summary>
        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// 导航地址.
        /// </summary>
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <inheritdoc/>
        protected async override void OnTapped(TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Uri))
            {
                await Launcher.LaunchUriAsync(new Uri(Uri));
            }
        }
    }
}
