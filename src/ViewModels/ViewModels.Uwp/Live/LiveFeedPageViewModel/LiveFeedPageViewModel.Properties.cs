// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Live;
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
        private readonly INavigationViewModel _navigationViewModel;

        /// <inheritdoc/>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <inheritdoc/>
        public ObservableCollection<ILiveItemViewModel> Follows { get; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> HotPartitions { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> SeeAllPartitionsCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsLoggedIn { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsFollowsEmpty { get; set; }
    }
}
