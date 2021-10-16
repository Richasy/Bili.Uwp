// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 播放数据面板.
    /// </summary>
    public sealed partial class PlayerDashboard : PlayerComponent
    {
        private bool _isLikeHoldCompleted;
        private bool _isLikeHoldSuspend;
        private PgcDetailView _detailView;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDashboard"/> class.
        /// </summary>
        public PlayerDashboard()
        {
            this.InitializeComponent();
        }

        private async void OnLikeButtonHoldingCompletedAsync(object sender, System.EventArgs e)
        {
            _isLikeHoldCompleted = true;
            await ViewModel.TripleAsync();
            CoinButton.ShowBubbles();
            FavoriteButton.ShowBubbles();
        }

        private void OnShareButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Share();
        }

        private void OnPgcDetailButtonClick(object sender, RoutedEventArgs e)
        {
            if (_detailView == null)
            {
                _detailView = new PgcDetailView();
            }

            _detailView.Show();
        }

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
        {
            ViewModel.IsFollow = !ViewModel.IsFollow;
            ViewModel.IsFollow = !ViewModel.IsFollow;

            await ViewModel.ToggleFollowAsync();
        }

        private async void OnLikeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (_isLikeHoldCompleted || _isLikeHoldSuspend)
            {
                _isLikeHoldCompleted = false;
                _isLikeHoldSuspend = false;
                return;
            }

            ViewModel.IsLikeChecked = !ViewModel.IsLikeChecked;
            ViewModel.IsLikeChecked = !ViewModel.IsLikeChecked;
            await ViewModel.LikeAsync();
        }

        private async void OnGiveCoinButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var coinNumber = Convert.ToInt32((sender as FrameworkElement).Tag);
            CoinFlyout.Hide();
            await ViewModel.CoinAsync(coinNumber, AlsoLikeCheckBox.IsChecked.Value);
        }

        private void OnCoinButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsCoinChecked = !ViewModel.IsCoinChecked;
            ViewModel.IsCoinChecked = !ViewModel.IsCoinChecked;

            if (!ViewModel.IsCoinChecked)
            {
                AlsoLikeCheckBox.IsChecked = true;
                CoinFlyout.ShowAt(CoinButton);
            }
        }

        private async void OnRefreshFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadFavoritesAsync();
        }

        private async void OnRequestFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
        {
            FavoriteFlyout.Hide();
            await ViewModel.FavoriteAsync();
        }

        private async void OnFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
        {
            ViewModel.IsFavoriteChecked = !ViewModel.IsFavoriteChecked;
            ViewModel.IsFavoriteChecked = !ViewModel.IsFavoriteChecked;

            if (AccountViewModel.Instance.Status == AccountViewModelStatus.Login)
            {
                FavoriteFlyout.ShowAt(FavoriteButton);
                await ViewModel.LoadFavoritesAsync();
            }
        }

        private void OnLikeButtonHoldingSuspend(object sender, EventArgs e)
        {
            _isLikeHoldSuspend = true;
        }
    }
}
