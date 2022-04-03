// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.App.Controls;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.ViewModels.Uwp.Live;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 直播分区详情页.
    /// </summary>
    public sealed partial class LiveAreaDetailPage : AppPage
    {
        private readonly LiveAreaViewModel _viewModel = LiveAreaViewModel.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveAreaDetailPage"/> class.
        /// </summary>
        public LiveAreaDetailPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is LiveArea area)
            {
                _viewModel.SetLiveArea(area);
                await _viewModel.RequestDataAsync();
            }
        }

        private async void OnTotalTagsButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => await new LiveAreaView().ShowAsync();

        private async void OnRefreshButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => await _viewModel.InitializeRequestAsync();

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
            => await _viewModel.RequestDataAsync();
    }
}
