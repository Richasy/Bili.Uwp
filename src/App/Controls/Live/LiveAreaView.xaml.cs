// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.ViewModels.Uwp;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 直播分区视图.
    /// </summary>
    public sealed partial class LiveAreaView : CenterPopup
    {
        private readonly LiveModuleViewModel _viewModel = LiveModuleViewModel.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveAreaView"/> class.
        /// </summary>
        public LiveAreaView() => InitializeComponent();

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

        private void OnAreaClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }

        private async void OnRefreshButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => await _viewModel.InitializeAreaIndexAsync();
    }
}
