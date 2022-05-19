// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Models.BiliBili;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// 直播分区视图.
    /// </summary>
    public sealed partial class LiveAreaView : CenterPopup
    {
        private readonly LiveModuleViewModel _viewModel = LiveModuleViewModel.Instance;
        private readonly INavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveAreaView"/> class.
        /// </summary>
        public LiveAreaView()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<INavigationViewModel>();
        }

        /// <summary>
        /// 显示视图.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync()
        {
            Show();
            if (_viewModel.LiveAreaGroupCollection.Count == 0)
            {
                await _viewModel.InitializeAreaIndexAsync();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
            => await _viewModel.InitializeAreaIndexAsync();

        private void OnAreaClick(object sender, System.EventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as LiveArea;
            _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.LiveAreaDetail, data);
            Hide();
        }
    }
}
