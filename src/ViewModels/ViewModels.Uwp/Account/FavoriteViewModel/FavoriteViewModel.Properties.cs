// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 收藏夹视图模型.
    /// </summary>
    public partial class FavoriteViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;

        /// <summary>
        /// 实例.
        /// </summary>
        public static FavoriteViewModel Instance { get; } = new Lazy<FavoriteViewModel>(() => new FavoriteViewModel()).Value;

        /// <summary>
        /// 用户Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 当前显示的收藏夹类型.
        /// </summary>
        public FavoriteType CurrentType { get; set; }

        /// <summary>
        /// 收藏夹类型集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<FavoriteType> TypeCollection { get; set; }

        /// <summary>
        /// 视频收藏夹分类集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<FavoriteVideoFolderViewModel> VideoFolderCollection { get; set; }

        /// <summary>
        /// 默认视频收藏夹视图模型.
        /// </summary>
        [Reactive]
        public FavoriteVideoViewModel DefaultVideoViewModel { get; set; }

        /// <summary>
        /// 视频收藏夹是否已请求.
        /// </summary>
        [Reactive]
        public bool IsVideoRequested { get; set; }

        /// <summary>
        /// 视频收藏夹是否正在初始加载.
        /// </summary>
        [Reactive]
        public bool IsVideoInitializeLoading { get; set; }

        /// <summary>
        /// 视频收藏夹是否请求失败.
        /// </summary>
        [Reactive]
        public bool IsVideoError { get; set; }

        /// <summary>
        /// 视频收藏夹失败错误信息.
        /// </summary>
        [Reactive]
        public string VideoErrorText { get; set; }

        private BiliController Controller { get; } = BiliController.Instance;
    }
}
