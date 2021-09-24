// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.App.Controls.Dialogs;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private async void OnLoadMoreButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var btn = sender as HyperlinkButton;
            btn.IsEnabled = false;
            var vm = btn.DataContext as FavoriteVideoFolderViewModel;
            await vm.LoadMoreAsync();
            btn.IsEnabled = true;
        }

        private async void OnDeleteFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var vm = btn.DataContext as FavoriteListDetailViewModel;
            btn.IsEnabled = false;

            var warning = ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(Models.Enums.LanguageNames.DeleteFavoriteWarning);
            var dialog = new ConfirmDialog(warning);
            var isConfirm = (await dialog.ShowAsync()) == ContentDialogResult.Primary;

            if (isConfirm)
            {
                var result = await vm.DeleteAsync();
                if (result)
                {
                    var myFolder = ViewModel.VideoFolderCollection.Where(p => p.IsMine).FirstOrDefault();
                    if (myFolder != null)
                    {
                        myFolder.FavoriteCollection.Remove(vm);
                        myFolder.IsShowEmpty = myFolder.FavoriteCollection.Count == 0;
                    }
                }
            }

            btn.IsEnabled = true;
        }

        private async void OnUnFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var vm = btn.DataContext as FavoriteListDetailViewModel;
            btn.IsEnabled = false;

            var warning = ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(Models.Enums.LanguageNames.UnFavoriteWarning);
            var dialog = new ConfirmDialog(warning);
            var isConfirm = (await dialog.ShowAsync()) == ContentDialogResult.Primary;

            if (isConfirm)
            {
                var result = await vm.UnFavoriteAsync();
                if (result)
                {
                    var folder = ViewModel.VideoFolderCollection.Where(p => !p.IsMine).FirstOrDefault();
                    if (folder != null)
                    {
                        folder.FavoriteCollection.Remove(vm);
                        folder.IsShowEmpty = folder.FavoriteCollection.Count == 0;
                    }
                }
            }

            btn.IsEnabled = true;
        }

        private async void OnAddToViewLaterButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).DataContext as VideoViewModel;
            await ViewLaterViewModel.Instance.AddAsync(vm);
        }

        private async void OnUnFavoriteVideoButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).DataContext as VideoViewModel;
            var result = await ViewModel.RemoveFavoriteVideoAsync(ViewModel.DefaultVideoViewModel.Id, Convert.ToInt32(vm.VideoId));
            if (result)
            {
                ViewModel.DefaultVideoViewModel.VideoCollection.Remove(vm);
            }
        }

        private async void OnRefreshRequestedAsync(Microsoft.UI.Xaml.Controls.RefreshContainer sender, Microsoft.UI.Xaml.Controls.RefreshRequestedEventArgs args)
        {
            await ViewModel.InitializeRequestAsync(Models.Enums.App.FavoriteType.Video);
        }
    }
}
