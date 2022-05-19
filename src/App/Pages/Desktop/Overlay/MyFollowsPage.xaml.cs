// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.App.Controls;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 我的关注页面.
    /// </summary>
    public sealed partial class MyFollowsPage : AppPage, IRefreshPage
    {
        private readonly MyFollowingViewModel _viewModel = MyFollowingViewModel.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyFollowsPage"/> class.
        /// </summary>
        public MyFollowsPage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
        }

        /// <inheritdoc/>
        public async Task RefreshAsync()
        {
            _viewModel.IsRequested = false;
            await _viewModel.InitializeTagsAsync();
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.IsRequested)
            {
                await _viewModel.InitializeTagsAsync();
            }
        }

        private async void OnViewRequestLoadMoreAsync(object sender, EventArgs e)
            => await _viewModel.RequestDataAsync();

        private async void OnUserCardClickAsync(object sender, EventArgs e)
            => await UserView.Instance.ShowAsync((sender as UserSlimCard).ViewModel);

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
            => await RefreshAsync();
    }
}
