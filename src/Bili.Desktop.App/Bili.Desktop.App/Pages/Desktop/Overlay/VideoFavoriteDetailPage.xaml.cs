// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Base;
using Bili.Models.Data.Video;
using Microsoft.UI.Xaml.Navigation;

namespace Bili.Desktop.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 视频收藏详情页面.
    /// </summary>
    public sealed partial class VideoFavoriteDetailPage : VideoFavoriteDetailPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteDetailPage"/> class.
        /// </summary>
        public VideoFavoriteDetailPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is VideoFavoriteFolder folder)
            {
                ViewModel.InjectData(folder);
                ViewModel.ReloadCommand.ExecuteAsync(null);
            }
        }
    }
}
