// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播首页视图模型.
    /// </summary>
    public sealed partial class LiveFeedPageViewModel
    {
        private readonly ILiveProvider _liveProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// 横幅集合.
        /// </summary>
        public ObservableCollection<BannerViewModel> Banners { get; }

        /// <summary>
        /// 关注的直播间集合.
        /// </summary>
        public ObservableCollection<LiveItemViewModel> Follows { get; }

        /// <summary>
        /// 热门分区.
        /// </summary>
        public ObservableCollection<Partition> HotPartitions { get; }

        /// <summary>
        /// 查看全部分区的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SeeAllPartitionsCommand { get; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        [Reactive]
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// 关注的直播间是否为空.
        /// </summary>
        [Reactive]
        public bool IsFollowsEmpty { get; set; }
    }
}
