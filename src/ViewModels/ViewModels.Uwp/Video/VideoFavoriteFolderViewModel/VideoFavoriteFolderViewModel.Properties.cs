// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Reactive;
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
    public sealed partial class VideoFavoriteFolderViewModel
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly INavigationViewModel _navigationViewModel;

        private IVideoFavoriteFolderGroupViewModel _groupViewModel;

        /// <inheritdoc/>
        public IRelayCommand RemoveCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowDetailCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public VideoFavoriteFolder Folder { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsMine { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsRemoving { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoFavoriteFolderViewModel model && EqualityComparer<VideoFavoriteFolder>.Default.Equals(Folder, model.Folder);

        /// <inheritdoc/>
        public override int GetHashCode() => Folder.GetHashCode();
    }
}
