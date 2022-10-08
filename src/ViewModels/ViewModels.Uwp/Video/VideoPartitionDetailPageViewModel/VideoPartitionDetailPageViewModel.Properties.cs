// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 分区详情页视图模型.
    /// </summary>
    public sealed partial class VideoPartitionDetailPageViewModel
    {
        private readonly IHomeProvider _homeProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly Dictionary<Partition, IEnumerable<VideoInformation>> _caches;

        /// <inheritdoc/>
        public IRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> SubPartitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<VideoSortType> SortTypes { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public Partition OriginPartition { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public Partition CurrentSubPartition { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public VideoSortType SortType { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsRecommendPartition { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsShowBanner { get; set; }
    }
}
