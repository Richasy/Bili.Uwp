// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.ViewModels.Uwp.Core;
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
        private readonly NavigationViewModel _navigationViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isRemoving;

        private VideoFavoriteFolderGroupViewModel _groupViewModel;

        /// <summary>
        /// 移除收藏夹命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        /// <summary>
        /// 显示收藏夹详情命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowDetailCommand { get; }

        /// <summary>
        /// 收藏夹信息.
        /// </summary>
        [Reactive]
        public VideoFavoriteFolder Folder { get; set; }

        /// <summary>
        /// 是否由自己创建.
        /// </summary>
        [Reactive]
        public bool IsMine { get; set; }

        /// <summary>
        /// 是否正在移除.
        /// </summary>
        public bool IsRemoving => _isRemoving.Value;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoFavoriteFolderViewModel model && EqualityComparer<VideoFavoriteFolder>.Default.Equals(Folder, model.Folder);

        /// <inheritdoc/>
        public override int GetHashCode() => Folder.GetHashCode();
    }
}
