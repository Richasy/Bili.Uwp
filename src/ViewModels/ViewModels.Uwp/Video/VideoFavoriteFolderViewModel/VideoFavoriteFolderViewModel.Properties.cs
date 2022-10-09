// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 收藏夹视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderViewModel
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly INavigationViewModel _navigationViewModel;

        private IVideoFavoriteFolderGroupViewModel _groupViewModel;

        [ObservableProperty]
        private VideoFavoriteFolder _folder;

        [ObservableProperty]
        private bool _isMine;

        [ObservableProperty]
        private bool _isRemoving;

        /// <inheritdoc/>
        public IAsyncRelayCommand RemoveCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowDetailCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoFavoriteFolderViewModel model && EqualityComparer<VideoFavoriteFolder>.Default.Equals(Folder, model.Folder);

        /// <inheritdoc/>
        public override int GetHashCode() => Folder.GetHashCode();
    }
}
