// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频收藏夹详情视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderDetailViewModel
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly AccountViewModel _accountViewModel;
        private bool _isEnd;

        /// <summary>
        /// 收藏夹创建者信息.
        /// </summary>
        [Reactive]
        public UserProfile User { get; set; }

        /// <summary>
        /// 收藏夹信息..
        /// </summary>
        [Reactive]
        public VideoFavoriteFolder Folder { get; set; }

        /// <summary>
        /// 收藏夹是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }
    }
}
