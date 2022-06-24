// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 可选择的视频收藏夹视图模型.
    /// </summary>
    public sealed class VideoFavoriteFolderSelectableViewModel : SelectableViewModelBase<VideoFavoriteFolder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteFolderSelectableViewModel"/> class.
        /// </summary>
        public VideoFavoriteFolderSelectableViewModel(VideoFavoriteFolder folder, bool isSelected)
            : base(folder, isSelected)
        {
        }
    }
}
