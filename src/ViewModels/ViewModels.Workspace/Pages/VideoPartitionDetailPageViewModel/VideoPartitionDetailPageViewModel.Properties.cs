// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Workspace.Pages
{
    /// <summary>
    /// 分区详情页视图模型.
    /// </summary>
    public sealed partial class VideoPartitionDetailPageViewModel
    {
        private readonly IHomeProvider _homeProvider;
        private readonly IResourceToolkit _resourceToolkit;

        [ObservableProperty]
        private Partition _originPartition;

        [ObservableProperty]
        private Partition _currentSubPartition;

        [ObservableProperty]
        private VideoSortType _sortType;

        [ObservableProperty]
        private bool _isRecommendPartition;

        [ObservableProperty]
        private bool _isShowBanner;

        /// <inheritdoc/>
        public IRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> SubPartitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<VideoSortType> SortTypes { get; }
    }
}
