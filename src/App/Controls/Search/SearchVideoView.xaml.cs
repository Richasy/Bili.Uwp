// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// 搜索视频视图.
    /// </summary>
    public sealed partial class SearchVideoView : SearchComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchVideoView"/> class.
        /// </summary>
        public SearchVideoView()
        {
            InitializeComponent();
        }

        private async void OnVideoRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.VideoModule.InitializeRequestAsync();
        }
    }
}
