// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;

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

            FavoriteFolders.Clear();
            var data = await _favoriteProvider.GetCurrentPlayerFavoriteListAsync(_authorizeProvider.CurrentUserId, View.Information.Identifier.Id);
            var selectIds = data.Item2;
            foreach (var item in data.Item1.Items)
            {
                var isSelected = selectIds != null && selectIds.Contains(item.Id);
                var vm = new VideoFavoriteFolderSelectableViewModel(item, isSelected);
                FavoriteFolders.Add(vm);
            }
        }
    }
}
