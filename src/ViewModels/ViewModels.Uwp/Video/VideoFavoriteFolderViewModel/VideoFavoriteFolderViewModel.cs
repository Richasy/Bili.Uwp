// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 收藏夹视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteFolderViewModel"/> class.
        /// </summary>
        public VideoFavoriteFolderViewModel(
            IFavoriteProvider favoriteProvider,
            IAccountProvider accountProvider,
            AppViewModel appViewModel)
        {
            _favoriteProvider = favoriteProvider;
            _accountProvider = accountProvider;
            _appViewModel = appViewModel;

            RemoveCommand = ReactiveCommand.CreateFromTask(RemoveAsync, outputScheduler: RxApp.MainThreadScheduler);
            ShowDetailCommand = ReactiveCommand.Create(ShowDetail, outputScheduler: RxApp.MainThreadScheduler);
            _isRemoving = RemoveCommand.IsExecuting.ToProperty(this, x => x.IsRemoving, scheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置收藏夹信息.
        /// </summary>
        /// <param name="folder">收藏夹.</param>
        /// <param name="groupRef">收藏夹分组视图模型引用.</param>
        public void SetFolder(VideoFavoriteFolder folder, VideoFavoriteFolderGroupViewModel groupRef)
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
            => _appViewModel.ShowVideoFavoriteFolderDetail(Folder);
    }
}
