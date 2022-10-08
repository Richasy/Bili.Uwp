// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 收藏夹分组视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderGroupViewModel
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAccountProvider _accountProvider;

        /// <inheritdoc/>
        [ObservableProperty]
        public VideoFavoriteFolderGroup Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsEmpty { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool HasMore { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoFavoriteFolderGroupViewModel model && EqualityComparer<VideoFavoriteFolderGroup>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}
