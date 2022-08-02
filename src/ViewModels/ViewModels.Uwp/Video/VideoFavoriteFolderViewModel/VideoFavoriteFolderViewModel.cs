// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 收藏夹视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderViewModel : ViewModelBase, IVideoFavoriteFolderViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteFolderViewModel"/> class.
        /// </summary>
        public VideoFavoriteFolderViewModel(
            IFavoriteProvider favoriteProvider,
            IAccountProvider accountProvider,
            INavigationViewModel navigationViewModel)
        {
            _favoriteProvider = favoriteProvider;
            _accountProvider = accountProvider;
            _navigationViewModel = navigationViewModel;

            RemoveCommand = ReactiveCommand.CreateFromTask(RemoveAsync);
            ShowDetailCommand = ReactiveCommand.Create(ShowDetail);
            RemoveCommand.IsExecuting.ToPropertyEx(this, x => x.IsRemoving);
        }

        /// <inheritdoc/>
        public void SetFolder(VideoFavoriteFolder folder, IVideoFavoriteFolderGroupViewModel groupRef)
        {
            Folder = folder;
            _groupViewModel = groupRef;
            IsMine = (folder.User?.Id ?? string.Empty) == _accountProvider.UserId.ToString();
        }

        private async Task RemoveAsync()
        {
            if (_groupViewModel == null)
            {
                return;
            }

            var removed = await _favoriteProvider.RemoveFavoriteFolderAsync(Folder.Id, IsMine);
            if (removed)
            {
                _groupViewModel.Items.Remove(this);
            }
        }

        private void ShowDetail()
            => _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.VideoFavoriteDetail, Folder);
    }
}
