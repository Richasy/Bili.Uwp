// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class LivePage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(LiveModuleViewModel), typeof(LivePage), new PropertyMetadata(LiveModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="LivePage"/> class.
        /// </summary>
        public LivePage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
            SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// 直播模块视图模型.
        /// </summary>
        public LiveModuleViewModel ViewModel
        {
            get { return (LiveModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.BannerCollection.Count == 0)
            {
                await ViewModel.RequestDataAsync();
            }

            FindName(nameof(FollowLiveView));
            FindName(nameof(RootGrid));

            UpdateLayout();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) => UpdateLayout();

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }

        private void OnFollowListViewItemClick(object sender, EventArgs e)
        {
            StandardFollowFlyout.Hide();
        }
    }
}
