﻿// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 热门视频页面.
    /// </summary>
    public sealed partial class PopularPage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PopularViewModel), typeof(RecommendPage), new PropertyMetadata(PopularViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="PopularPage"/> class.
        /// </summary>
        public PopularPage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 热门视频视图模型.
        /// </summary>
        public PopularViewModel ViewModel
        {
            get { return (PopularViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.VideoCollection.Any())
            {
                await ViewModel.RequestDataAsync();
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }
    }
}
