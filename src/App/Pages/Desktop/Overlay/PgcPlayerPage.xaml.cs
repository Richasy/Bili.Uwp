// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Pages.Base;
using Bili.Models.Data.Local;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// PGC 内容的播放页面.
    /// </summary>
    public sealed partial class PgcPlayerPage : PgcPlayerPageBase
    {
        private bool _isLikeHoldCompleted;
        private bool _isLikeHoldSuspend;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlayerPage"/> class.
        /// </summary>
        public PgcPlayerPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PlaySnapshot shot)
            {
                ViewModel.SetSnapshot(shot);
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
            => ViewModel.ClearCommand.Execute(null);

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

        private void OnRefreshFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
            => ViewModel.RequestFavoriteFoldersCommand.ExecuteAsync(null);

        private void OnGiveCoinButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var num = int.Parse((sender as FrameworkElement).Tag.ToString());
            ViewModel.CoinCommand.Execute(num);
        }

        private void OnLikeButtonHoldingCompleted(object sender, System.EventArgs e)
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

        private void OnFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
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
