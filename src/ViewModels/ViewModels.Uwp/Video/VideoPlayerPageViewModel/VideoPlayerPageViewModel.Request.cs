// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Video;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private async Task GetFavoriteFoldersAsync()
        {
            IsFavoriteFoldersError = false;
            FavoriteFoldersErrorText = default;
            if (!IsSignedIn)
            {
                return;
            }

            TryClear(FavoriteFolders);
            var data = await _favoriteProvider.GetCurrentPlayerFavoriteListAsync(_authorizeProvider.CurrentUserId, View.Information.Identifier.Id);
            var selectIds = data.Item2;
            foreach (var item in data.Item1.Items)
            {
                var isSelected = selectIds != null && selectIds.Contains(item.Id);
                var vm = Locator.Instance.GetService<IVideoFavoriteFolderSelectableViewModel>();
                vm.InjectData(item);
                vm.IsSelected = isSelected;
                FavoriteFolders.Add(vm);
            }
        }

        private async Task GetOnlineCountAsync()
        {
            var text = await _playerProvider.GetOnlineViewerCountAsync(View.Information.Identifier.Id, CurrentVideoPart.Id);
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                WatchingCountText = text;
            });
        }

        private async Task FavoriteVideoAsync()
        {
            var selectedFolders = FavoriteFolders.Where(p => p.IsSelected).Select(p => p.Data.Id).ToList();
            var deselectedFolders = FavoriteFolders.Where(p => !p.IsSelected).Select(p => p.Data.Id).ToList();
            var result = await _playerProvider.FavoriteAsync(View.Information.Identifier.Id, selectedFolders, deselectedFolders, true);
            if (result == Models.Enums.Bili.FavoriteResult.Success || result == Models.Enums.Bili.FavoriteResult.InsufficientAccess)
            {
                IsFavorited = selectedFolders.Count > 0;
                _ = ReloadCommunityInformationCommand.ExecuteAsync(null);
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FavoriteFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private async Task CoinAsync(int coinNumber)
        {
            var isSuccess = await _playerProvider.CoinAsync(View.Information.Identifier.Id, coinNumber, IsCoinWithLiked);
            if (isSuccess)
            {
                IsCoined = true;
                if (IsCoinWithLiked)
                {
                    IsLiked = true;
                }

                _ = ReloadCommunityInformationCommand.ExecuteAsync(null);
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.CoinFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private async Task LikeAsync()
        {
            var isLike = !IsLiked;
            var isSuccess = await _playerProvider.LikeAsync(View.Information.Identifier.Id, isLike);
            if (isSuccess)
            {
                IsLiked = isLike;
                _ = ReloadCommunityInformationCommand.ExecuteAsync(null);
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private async Task TripleAsync()
        {
            var info = await _playerProvider.TripleAsync(View.Information.Identifier.Id);
            IsLiked = info.IsLiked;
            IsFavorited = info.IsFavorited;
            IsCoined = info.IsCoined;
            _ = ReloadCommunityInformationCommand.ExecuteAsync(null);
        }

        private async Task ReloadCommunityInformationAsync()
        {
            var data = await _playerProvider.GetVideoCommunityInformationAsync(View.Information.Identifier.Id);
            View.Information.CommunityInformation = data;
            InitializeCommunityInformation();
        }
    }
}
