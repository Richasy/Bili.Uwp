// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
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
        private readonly IAccountViewModel _accountViewModel;
        private bool _isEnd;

        /// <summary>
        /// 收藏夹创建者信息.
        /// </summary>
        [ObservableProperty]
        public UserProfile User { get; set; }

        /// <summary>
        /// 收藏夹信息..
        /// </summary>
        [ObservableProperty]
        public VideoFavoriteFolder Data { get; set; }

        /// <summary>
        /// 收藏夹是否为空.
        /// </summary>
        [ObservableProperty]
        public bool IsEmpty { get; set; }
    }
}
