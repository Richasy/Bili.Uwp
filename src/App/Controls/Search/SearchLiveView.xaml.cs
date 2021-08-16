// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 搜索直播视图.
    /// </summary>
    public sealed partial class SearchLiveView : SearchComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchLiveView"/> class.
        /// </summary>
        public SearchLiveView()
        {
            this.InitializeComponent();
        }

        private async void OnLiveRefreshButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LiveModule.InitializeRequestAsync();
        }
    }
}
