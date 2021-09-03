// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频收藏夹面板.
    /// </summary>
    public sealed partial class VideoFavoritePanel : FavoriteComponent
    {
        private readonly List<FavoriteVideoViewModel> _tempFavoriteList;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoritePanel"/> class.
        /// </summary>
        public VideoFavoritePanel()
        {
            this.InitializeComponent();
            _tempFavoriteList = new List<FavoriteVideoViewModel>();
        }

        private async void OnDefaultSeeAllButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await new FavoriteVideoView().ShowAsync(ViewModel.DefaultVideoViewModel);
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync(Models.Enums.App.FavoriteType.Video);
        }

        private async void OnSeeDetailButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var tag = (int)(sender as FrameworkElement).Tag;
            var vm = _tempFavoriteList.Where(p => p.Id == tag).FirstOrDefault();
            if (vm == null)
            {
                vm = new FavoriteVideoViewModel(tag);
                _tempFavoriteList.Add(vm);
            }

            await new FavoriteVideoView().ShowAsync(vm);
        }
    }
}
