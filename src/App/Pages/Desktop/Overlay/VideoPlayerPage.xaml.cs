// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.App.Controls.Dialogs;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Video;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 视频播放页面.
    /// </summary>
    public sealed partial class VideoPlayerPage : VideoPlayerPageBase
    {
        private bool _isLikeHoldCompleted;
        private bool _isLikeHoldSuspend;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerPage"/> class.
        /// </summary>
        public VideoPlayerPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PlaySnapshot shot)
            {
                ViewModel.SetSnapshot(shot);
            }
            else if (e.Parameter is Tuple<IEnumerable<VideoInformation>, int> playlist)
            {
                ViewModel.SetPlaylist(playlist.Item1, playlist.Item2);
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.ClearCommand.Execute().Subscribe();
            ViewModel.ClearPlaylistCommand.Execute().Subscribe();
        }

        private void OnSectionHeaderItemInvoked(object sender, Models.App.Other.PlayerSectionHeader e)
        {
            if (ViewModel.CurrentSection != e)
            {
                ViewModel.CurrentSection = e;
            }
        }

        private void OnRefreshFavoriteButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.RequestFavoriteFoldersCommand.Execute().Subscribe();

        private void OnGiveCoinButtonClick(object sender, RoutedEventArgs e)
        {
            var num = int.Parse((sender as FrameworkElement).Tag.ToString());
            ViewModel.CoinCommand.Execute(num).Subscribe();
            CoinFlyout.Hide();
        }

        private async void OnTagButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as Tag;
            var settingsToolkit = Locator.Current.GetService<ISettingsToolkit>();
            var resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
            var isFirstClick = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.IsFirstClickTag, true);

            if (isFirstClick)
            {
                var dialog = new ConfirmDialog(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FirstClickTagTip));
                var result = await dialog.ShowAsync();
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }

                settingsToolkit.WriteLocalSetting(Models.Enums.SettingNames.IsFirstClickTag, false);
            }

            ViewModel.SearchTagCommand.Execute(data).Subscribe();
        }

        private void OnLikeButtonHoldingCompleted(object sender, EventArgs e)
        {
            _isLikeHoldCompleted = true;
            ViewModel.TripleCommand.Execute().Subscribe();
            CoinButton.ShowBubbles();
            FavoriteButton.ShowBubbles();
        }

        private void OnLikeButtonHoldingSuspend(object sender, EventArgs e)
        {
            _isLikeHoldSuspend = true;
        }

        private void OnLikeButtonClick(object sender, RoutedEventArgs e)
        {
            if (_isLikeHoldCompleted || _isLikeHoldSuspend)
            {
                _isLikeHoldCompleted = false;
                _isLikeHoldSuspend = false;
                return;
            }

            ViewModel.LikeCommand.Execute().Subscribe();
        }

        private void OnCoinButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsCoined = !ViewModel.IsCoined;
            ViewModel.IsCoined = !ViewModel.IsCoined;

            if (!ViewModel.IsCoined)
            {
                CoinFlyout.ShowAt(CoinButton);
            }
        }

        private void OnFavoriteButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsFavorited = !ViewModel.IsFavorited;
            ViewModel.IsFavorited = !ViewModel.IsFavorited;

            if (ViewModel.FavoriteFolders.Count == 0)
            {
                ViewModel.RequestFavoriteFoldersCommand.Execute().Subscribe();
            }

            FavoriteFlyout.ShowAt(FavoriteButton);
        }
    }

    /// <summary>
    /// 视频播放器页面基类.
    /// </summary>
    public class VideoPlayerPageBase : AppPage<VideoPlayerPageViewModel>
    {
    }
}
