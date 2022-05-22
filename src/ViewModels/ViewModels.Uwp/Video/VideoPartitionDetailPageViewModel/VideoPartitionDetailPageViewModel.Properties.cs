// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
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
        private readonly ObservableAsPropertyHelper<bool> _isShowBanner;
        private readonly ObservableAsPropertyHelper<bool> _isRecommendPartition;

        /// <summary>
        /// 横幅集合.
        /// </summary>
        public ObservableCollection<BannerViewModel> Banners { get; }

        /// <summary>
        /// 子分区集合.
        /// </summary>
        public ObservableCollection<Partition> SubPartitions { get; }

        /// <summary>
        /// 视频排序方式集合.
        /// </summary>
        public ObservableCollection<VideoSortType> SortTypes { get; }

        /// <summary>
        /// 选中子分区命令.
        /// </summary>
        public ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <summary>
        /// 父分区.
        /// </summary>
        [Reactive]
        public Partition OriginPartition { get; private set; }

        /// <summary>
        /// 当前选中的子分区.
        /// </summary>
        [Reactive]
        public Partition CurrentSubPartition { get; set; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        [Reactive]
        public VideoSortType SortType { get; set; }

        /// <summary>
        /// 是否为推荐子分区.
        /// </summary>
        public bool IsRecommendPartition => _isRecommendPartition.Value;

        /// <summary>
        /// 是否显示横幅内容.
        /// </summary>
        public bool IsShowBanner => _isShowBanner.Value;
    }
}
