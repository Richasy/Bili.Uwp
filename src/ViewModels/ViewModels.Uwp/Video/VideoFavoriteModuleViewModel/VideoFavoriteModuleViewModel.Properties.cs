// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频收藏夹模块视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteModuleViewModel
    {
        private readonly AppViewModel _appViewModel;
        private readonly IAccountProvider _accountProvider;
        private readonly IFavoriteProvider _favoriteProvider;

        /// <summary>
        /// 默认收藏夹中的视频列表.
        /// </summary>
        public ObservableCollection<VideoItemViewModel> DefaultVideos { get; }

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

        /// <summary>
        /// 是否可以显示更多的默认收藏视频.
        /// </summary>
        [Reactive]
        public bool CanShowMoreDefaultVideos { get; set; }
    }
}
