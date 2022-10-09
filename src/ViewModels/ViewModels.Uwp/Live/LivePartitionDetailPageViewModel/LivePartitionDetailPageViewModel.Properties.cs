// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Live;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播分区详情页面视图模型.
    /// </summary>
    public sealed partial class LivePartitionDetailPageViewModel
    {
        private readonly ILiveProvider _liveProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly Dictionary<LiveTag, IEnumerable<LiveInformation>> _caches;

        private int _totalCount;

        [ObservableProperty]
        private Partition _originPartition;

        [ObservableProperty]
        private LiveTag _currentTag;

        [ObservableProperty]
        private bool _isEmpty;

        /// <inheritdoc/>
        public ObservableCollection<LiveTag> Tags { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<LiveTag> SelectTagCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand SeeAllPartitionsCommand { get; }
    }
}
