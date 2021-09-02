// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频收藏夹面板.
    /// </summary>
    public sealed partial class VideoFavoritePanel : FavoriteComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoritePanel"/> class.
        /// </summary>
        public VideoFavoritePanel()
        {
            this.InitializeComponent();
        }

        private void OnDefaultSeeAllButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }

        private async void OnRefreshButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync(Models.Enums.App.FavoriteType.Video);
        }
    }
}
