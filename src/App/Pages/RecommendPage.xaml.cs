// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 首页.
    /// </summary>
    public sealed partial class RecommendPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(RecommendViewModel), typeof(RecommendPage), new PropertyMetadata(RecommendViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendPage"/> class.
        /// </summary>
        public RecommendPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视频推荐视图模型.
        /// </summary>
        public RecommendViewModel ViewModel
        {
            get { return (RecommendViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.VideoCollection.Any())
            {
                await this.ViewModel.RequestDataAsync();
                this.FindName(nameof(VideoView));
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnRefreshRequestedAsync(Microsoft.UI.Xaml.Controls.RefreshContainer sender, Microsoft.UI.Xaml.Controls.RefreshRequestedEventArgs args)
        {
            var def = args.GetDeferral();
            if (!ViewModel.IsInitializeLoading && !ViewModel.IsDeltaLoading)
            {
                ViewModel.Reset();
                await ViewModel.RequestDataAsync();
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
