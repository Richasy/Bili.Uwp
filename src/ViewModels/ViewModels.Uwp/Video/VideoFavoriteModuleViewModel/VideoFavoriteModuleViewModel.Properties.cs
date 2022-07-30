// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频收藏夹模块视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteModuleViewModel
    {
        private readonly INavigationViewModel _navigationViewModel;
        private readonly IAccountProvider _accountProvider;
        private readonly IFavoriteProvider _favoriteProvider;

        /// <summary>
        /// 显示默认收藏夹详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowDefaultFolderDetailCommand { get; }

        /// <summary>
        /// 默认收藏夹.
        /// </summary>
        [Reactive]
        public VideoFavoriteFolder DefaultFolder { get; set; }

        /// <summary>
        /// 默认视频收藏夹是否为空.
        /// </summary>
        [Reactive]
        public bool IsDefaultFolderEmpty { get; set; }
    }
}
