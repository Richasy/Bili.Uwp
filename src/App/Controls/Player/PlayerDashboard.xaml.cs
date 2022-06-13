﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Account;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// 播放数据面板.
    /// </summary>
    public sealed partial class PlayerDashboard : PlayerComponent
    {
        private bool _isLikeHoldCompleted;
        private bool _isLikeHoldSuspend;
        private PgcSeasonDetailView _detailView;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDashboard"/> class.
        /// </summary>
        public PlayerDashboard()
        {
            InitializeComponent();
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
                _detailView = new PgcSeasonDetailView();
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

            if (Splat.Locator.Current.GetService<AccountViewModel>().State == AuthorizeState.SignedIn)
            {
                FavoriteFlyout.ShowAt(FavoriteButton);
                await ViewModel.LoadFavoritesAsync();
            }
        }

        private void OnLikeButtonHoldingSuspend(object sender, EventArgs e)
        {
            _isLikeHoldSuspend = true;
        }

        private async void OnLiveOnlyAudioToggledAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsLive && ViewModel.IsLiveAudioOnly != LiveAudioOnlySwitch.IsOn)
            {
                await ViewModel.ToggleLiveAudioAsync(LiveAudioOnlySwitch.IsOn);
            }
        }

        private async void OnFixButtonClickAsync(object sender, RoutedEventArgs e)
            => await ViewModel.ToggleFixStateAsync();
    }
}
