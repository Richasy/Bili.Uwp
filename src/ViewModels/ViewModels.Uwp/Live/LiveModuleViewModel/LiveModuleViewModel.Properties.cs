// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 直播视图模型的属性集.
    /// </summary>
    public partial class LiveModuleViewModel
    {
        private int _currentPage;

        /// <summary>
        /// <see cref="LiveModuleViewModel"/>的单例.
        /// </summary>
        public static LiveModuleViewModel Instance { get; } = new Lazy<LiveModuleViewModel>(() => new LiveModuleViewModel()).Value;

        /// <summary>
        /// 横幅集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<BannerViewModel> BannerCollection { get; set; }

        /// <summary>
        /// 关注的直播间集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> FollowLiveRoomCollection { get; set; }

        /// <summary>
        /// 推荐的直播间集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> RecommendLiveRoomCollection { get; set; }

        /// <summary>
        /// 是否显示关注列表.
        /// </summary>
        [Reactive]
        public bool IsShowFollowList { get; set; }
    }
}
