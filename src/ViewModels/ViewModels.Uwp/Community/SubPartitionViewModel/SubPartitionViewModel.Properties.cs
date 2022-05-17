// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 子分区视图模型属性集.
    /// </summary>
    public partial class SubPartitionViewModel
    {
        private readonly Partition _partition;
        private readonly bool _isRecommendPartition;

        private int _offsetId = 0;
        private int _pageNumber = 1;
        private DateTimeOffset _lastRequestTime = DateTimeOffset.MinValue;

        /// <summary>
        /// 子分区Id.
        /// </summary>
        public int SubPartitionId { get; internal set; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        [Reactive]
        public bool IsShowBanner { get; set; }

        /// <summary>
        /// 是否显示顶部标签组.
        /// </summary>
        [Reactive]
        public bool IsShowTags { get; set; }

        /// <summary>
        /// 子分区名称.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 当前排序方式.
        /// </summary>
        [Reactive]
        public VideoSortType CurrentSortType { get; set; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<BannerViewModel> BannerCollection { get; set; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }

        /// <summary>
        /// 显示出来的标签集合.
        /// </summary>
        public ObservableCollection<Tag> TagCollection { get; set; }

        /// <summary>
        /// 排序方式集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoSortType> SortTypeCollection { get; set; }
    }
}
