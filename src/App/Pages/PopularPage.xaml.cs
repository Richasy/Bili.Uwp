// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 热门视频页面.
    /// </summary>
    public sealed partial class PopularPage : Page
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
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
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
            if (!this.ViewModel.VideoCollection.Any())
            {
                await this.ViewModel.RequestDataAsync();
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await this.ViewModel.RequestDataAsync();
        }

        private async void OnRefreshRequestedAsync(Microsoft.UI.Xaml.Controls.RefreshContainer sender, Microsoft.UI.Xaml.Controls.RefreshRequestedEventArgs args)
        {
            var def = args.GetDeferral();
            if (!ViewModel.IsInitializeLoading && !ViewModel.IsDeltaLoading)
            {
                ViewModel.Reset();
                await this.ViewModel.RequestDataAsync();
            }

            def.Complete();
            def.Dispose();
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }
    }
}
