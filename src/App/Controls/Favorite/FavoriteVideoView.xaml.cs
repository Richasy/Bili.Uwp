// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.App.Pages;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频收藏夹视图.
    /// </summary>
    public sealed partial class FavoriteVideoView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(FavoriteVideoViewModel), typeof(FavoriteVideoView), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteVideoView"/> class.
        /// </summary>
        /// <param name="vm">视图模型.</param>
        public FavoriteVideoView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public FavoriteVideoViewModel ViewModel
        {
            get { return (FavoriteVideoViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="vm">视频收藏夹数据模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(FavoriteVideoViewModel vm)
        {
            Container.IsOpen = true;
            ((Window.Current.Content as Frame).Content as RootPage).ShowOnHolder(this);
            if (ViewModel == null || ViewModel.Id != vm.Id)
            {
                // 请求用户数据.
                ViewModel = vm;
                if (!vm.IsRequested)
                {
                    await ViewModel.InitializeRequestAsync();
                }
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private void OnContainerClosed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            ((Window.Current.Content as Frame).Content as RootPage).ClearHolder();
        }

        private void OnVideoItemClick(object sender, VideoViewModel e)
        {
            this.Container.IsOpen = false;
        }

        private async void OnAddToViewLaterButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).DataContext as VideoViewModel;
            await ViewLaterViewModel.Instance.AddAsync(vm);
        }

        private async void OnUnFavoriteVideoButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).DataContext as VideoViewModel;
            var result = await FavoriteViewModel.Instance.RemoveFavoriteVideoAsync(ViewModel.Id, Convert.ToInt32(vm.VideoId));
            if (result)
            {
                ViewModel.VideoCollection.Remove(vm);
            }
        }

        private void OnVideoFlyoutOpening(object sender, object e)
        {
            var flyout = sender as Microsoft.UI.Xaml.Controls.CommandBarFlyout;
            var element = flyout.SecondaryCommands.OfType<AppBarButton>().Last();
            element.IsEnabled = ViewModel.IsMine;
        }
    }
}
