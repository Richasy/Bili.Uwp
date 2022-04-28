// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.BiliBili;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
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
        public ObservableCollection<BannerViewModel> BannerCollection { get; }

        /// <summary>
        /// 关注的直播间集合.
        /// </summary>
        public ObservableCollection<VideoViewModel> FollowLiveRoomCollection { get; }

        /// <summary>
        /// 推荐的直播间集合.
        /// </summary>
        public ObservableCollection<VideoViewModel> RecommendLiveRoomCollection { get; }

        /// <summary>
        /// 热门标签集合.
        /// </summary>
        public ObservableCollection<LiveFeedHotArea> HotTagCollection { get; }

        /// <summary>
        /// 直播分区组集合.
        /// </summary>
        public ObservableCollection<LiveAreaGroup> LiveAreaGroupCollection { get; }

        /// <summary>
        /// 显示的直播分区集合.
        /// </summary>
        public ObservableCollection<LiveArea> DisplayAreaCollection { get; }

        /// <summary>
        /// 选中的分区组.
        /// </summary>
        [Reactive]
        public LiveAreaGroup SelectedAreaGroup { get; set; }

        /// <summary>
        /// 是否显示关注列表.
        /// </summary>
        [Reactive]
        public bool IsShowFollowList { get; set; }

        /// <summary>
        /// 直播间分区是否正在请求中.
        /// </summary>
        [Reactive]
        public bool IsLiveAreaRequesting { get; set; }

        /// <summary>
        /// 分区请求失败.
        /// </summary>
        [Reactive]
        public bool IsAreaError { get; set; }
    }
}
