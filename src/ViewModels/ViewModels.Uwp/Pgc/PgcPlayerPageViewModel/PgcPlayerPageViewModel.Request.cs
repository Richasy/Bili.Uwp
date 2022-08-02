// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.ViewModels.Interfaces.Video;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private async Task RequestEpisodeInteractionInformationAsync()
        {
            if (CurrentEpisode != null)
            {
                var data = await _playerProvider.GetEpisodeInteractionInformationAsync(CurrentEpisode.Identifier.Id);
                IsLiked = data.IsLiked;
                IsFavorited = data.IsFavorited;
                IsCoined = data.IsCoined;
            }
        }

        private async Task GetFavoriteFoldersAsync()
        {
            IsFavoriteFoldersError = false;
            FavoriteFoldersErrorText = default;
            if (!IsSignedIn)
            {
                return;
            }

            TryClear(FavoriteFolders);
            var data = await _favoriteProvider.GetCurrentPlayerFavoriteListAsync(_authorizeProvider.CurrentUserId, CurrentEpisode.VideoId);
            var selectIds = data.Item2;
            foreach (var item in data.Item1.Items)
            {
                var isSelected = selectIds != null && selectIds.Contains(item.Id);
                var vm = Locator.Current.GetService<IVideoFavoriteFolderSelectableViewModel>();
                vm.InjectData(item);
                vm.IsSelected = isSelected;
                FavoriteFolders.Add(vm);
            }
        }

        private async Task FavoriteVideoAsync()
        {
            var selectedFolders = FavoriteFolders.Where(p => p.IsSelected).Select(p => p.Data.Id).ToList();
            var deselectedFolders = FavoriteFolders.Where(p => !p.IsSelected).Select(p => p.Data.Id).ToList();
            var result = await _playerProvider.FavoriteAsync(CurrentEpisode.Identifier.Id, selectedFolders, deselectedFolders, false);
            if (result == Models.Enums.Bili.FavoriteResult.Success || result == Models.Enums.Bili.FavoriteResult.InsufficientAccess)
            {
                IsFavorited = selectedFolders.Count > 0;
                ReloadInteractionInformationCommand.Execute().Subscribe();
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FavoriteFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private async Task CoinAsync(int coinNumber)
        {
            var isSuccess = await _playerProvider.CoinAsync(CurrentEpisode.VideoId, coinNumber, IsCoinWithLiked);
            if (isSuccess)
            {
                IsCoined = true;
                if (IsCoinWithLiked)
                {
                    IsLiked = true;
                }

                ReloadInteractionInformationCommand.Execute().Subscribe();
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.CoinFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private async Task LikeAsync()
        {
            var isLike = !IsLiked;
            var isSuccess = await _playerProvider.LikeAsync(CurrentEpisode.VideoId, isLike);
            if (isSuccess)
            {
                IsLiked = isLike;
                ReloadInteractionInformationCommand.Execute().Subscribe();
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private async Task TripleAsync()
        {
            var info = await _playerProvider.TripleAsync(CurrentEpisode.VideoId);
            IsLiked = info.IsLiked;
            IsFavorited = info.IsFavorited;
            IsCoined = info.IsCoined;
            ReloadInteractionInformationCommand.Execute().Subscribe();
        }

        private async Task TrackAsync()
        {
            var isTrack = !IsTracking;
            var isSuccess = await _pgcProvider.FollowAsync(View.Information.Identifier.Id, isTrack);
            if (isSuccess)
            {
                IsTracking = isTrack;
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }
    }
}
