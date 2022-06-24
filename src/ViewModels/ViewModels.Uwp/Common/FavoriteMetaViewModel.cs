// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 收藏夹元数据视图模型.
    /// </summary>
    public class FavoriteMetaViewModel : SelectableViewModelBase<VideoFavoriteFolder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteMetaViewModel"/> class.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <param name="isSelected">是否选中.</param>
        public FavoriteMetaViewModel(VideoFavoriteFolder data, bool isSelected)
            : base(data, isSelected)
        {
        }
    }
}
