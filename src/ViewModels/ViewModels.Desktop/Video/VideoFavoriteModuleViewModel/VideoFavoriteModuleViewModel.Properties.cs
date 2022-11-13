// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Video
{
    /// <summary>
    /// 视频收藏夹模块视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteModuleViewModel
    {
        private readonly INavigationViewModel _navigationViewModel;
        private readonly IAccountProvider _accountProvider;
        private readonly IFavoriteProvider _favoriteProvider;

        [ObservableProperty]
        private VideoFavoriteFolder _defaultFolder;

        [ObservableProperty]
        private bool _isDefaultFolderEmpty;

        /// <summary>
        /// 显示默认收藏夹详情的命令.
        /// </summary>
        public IRelayCommand ShowDefaultFolderDetailCommand { get; }
    }
}
