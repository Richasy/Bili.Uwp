// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

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

        [ObservableProperty]
        private VideoFavoriteFolderGroup _data;

        [ObservableProperty]
        private bool _isEmpty;

        [ObservableProperty]
        private bool _hasMore;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoFavoriteFolderGroupViewModel model && EqualityComparer<VideoFavoriteFolderGroup>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}
