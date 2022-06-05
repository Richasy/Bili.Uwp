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

        /// <summary>
        /// 收藏夹分组信息.
        /// </summary>
        [Reactive]
        public VideoFavoriteFolderGroup Group { get; set; }

        /// <summary>
        /// 收藏夹分组下是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }

        /// <summary>
        /// 是否还有更多内容.
        /// </summary>
        [Reactive]
        public bool HasMore { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoFavoriteFolderGroupViewModel model && EqualityComparer<VideoFavoriteFolderGroup>.Default.Equals(Group, model.Group);

        /// <inheritdoc/>
        public override int GetHashCode() => Group.GetHashCode();
    }
}
