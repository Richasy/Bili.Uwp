// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.System;

namespace Bili.Desktop.App.Controls
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
        /// <see cref="IsTooltipEnabled"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsTooltipEnabledProperty =
            DependencyProperty.Register(nameof(IsTooltipEnabled), typeof(bool), typeof(BannerItem), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="MaxImageHeight"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty MaxImageHeightProperty =
            DependencyProperty.Register(nameof(MaxImageHeight), typeof(double), typeof(BannerItem), new PropertyMetadata(114d));

        /// <summary>
        /// <see cref="MinImageHeight"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty MinImageHeightProperty =
            DependencyProperty.Register(nameof(MinImageHeight), typeof(double), typeof(BannerItem), new PropertyMetadata(100d));

        /// <summary>
        /// <see cref="CardStyle"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CardStyleProperty =
            DependencyProperty.Register(nameof(CardStyle), typeof(Style), typeof(BannerItem), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerItem"/> class.
        /// </summary>
        public BannerItem()
        {
            DefaultStyleKey = typeof(BannerItem);
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

        /// <summary>
        /// 是否显示悬浮提示.
        /// </summary>
        public bool IsTooltipEnabled
        {
            get { return (bool)GetValue(IsTooltipEnabledProperty); }
            set { SetValue(IsTooltipEnabledProperty, value); }
        }

        /// <summary>
        /// 图片最大高度.
        /// </summary>
        public double MaxImageHeight
        {
            get { return (double)GetValue(MaxImageHeightProperty); }
            set { SetValue(MaxImageHeightProperty, value); }
        }

        /// <summary>
        /// 图片最小高度.
        /// </summary>
        public double MinImageHeight
        {
            get { return (double)GetValue(MinImageHeightProperty); }
            set { SetValue(MinImageHeightProperty, value); }
        }

        /// <summary>
        /// 内部容器<see cref="CardPanel"/>的样式.
        /// </summary>
        public Style CardStyle
        {
            get { return (Style)GetValue(CardStyleProperty); }
            set { SetValue(CardStyleProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            var rootPanel = GetTemplateChild("RootPanel") as CardPanel;
            rootPanel.Click -= OnRootPanelClickAsync;
            rootPanel.Click += OnRootPanelClickAsync;
        }

        private async void OnRootPanelClickAsync(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Uri))
            {
                await Launcher.LaunchUriAsync(new Uri(Uri));
            }
        }
    }
}
