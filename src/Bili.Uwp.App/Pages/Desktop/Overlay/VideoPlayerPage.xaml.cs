// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Uwp.App.Controls.Dialogs;
using Bili.Uwp.App.Pages.Base;
using Bili.DI.Container;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Bili.Uwp.App.Pages.Desktop.Overlay
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
            ViewModel.ClearCommand.Execute(null);
            ViewModel.ClearPlaylistCommand.Execute(null);
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnSectionHeaderItemInvoked(object sender, Models.App.Other.PlayerSectionHeader e)
        {
            if (ViewModel.CurrentSection != e)
            {
                ViewModel.CurrentSection = e;
            }
        }

        private void OnRefreshFavoriteButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.RequestFavoriteFoldersCommand.ExecuteAsync(null);

        private void OnGiveCoinButtonClick(object sender, RoutedEventArgs e)
        {
            var num = int.Parse((sender as FrameworkElement).Tag.ToString());
            ViewModel.CoinCommand.ExecuteAsync(num);
            CoinFlyout.Hide();
        }

        private async void OnTagButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as Tag;
            var settingsToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
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

            ViewModel.SearchTagCommand.Execute(data);
        }

        private void OnLikeButtonHoldingCompleted(object sender, EventArgs e)
        {
            _isLikeHoldCompleted = true;
            ViewModel.TripleCommand.ExecuteAsync(null);
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

            ViewModel.LikeCommand.ExecuteAsync(null);
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
                ViewModel.RequestFavoriteFoldersCommand.ExecuteAsync(null);
            }

            FavoriteFlyout.ShowAt(FavoriteButton);
        }
    }
}
