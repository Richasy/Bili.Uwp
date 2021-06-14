// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 子分区视图模型属性集.
    /// </summary>
    public partial class SubPartitionViewModel
    {
        private readonly Partition _partition;
        private readonly bool _isRecommendPartition;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly BiliController _controller;

        private int _offsetId = 0;
        private int _pageNumber = 1;
        private DateTimeOffset _lastRequestTime = DateTimeOffset.MinValue;

        /// <summary>
        /// 子分区Id.
        /// </summary>
        public int SubPartitionId { get; internal set; }

        /// <summary>
        /// 标识该分区是否已经加载过数据.
        /// </summary>
        public bool IsRequested => _offsetId != 0 || _pageNumber > 1;

        /// <summary>
        /// 子分区名称.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 是否在执行初始化数据加载.
        /// </summary>
        [Reactive]
        public bool IsInitializeLoading { get; set; }

        /// <summary>
        /// 是否在执行增量加载.
        /// </summary>
        [Reactive]
        public bool IsDeltaLoading { get; set; }

        /// <summary>
        /// 当前排序方式.
        /// </summary>
        [Reactive]
        public VideoSortType CurrentSortType { get; set; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<Banner> BannerCollection { get; set; }

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
