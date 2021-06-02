// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;
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
        private bool _isRequested;

        /// <summary>
        /// 子分区Id.
        /// </summary>
        public int SubPartitionId => _partition.Tid;

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
        /// 横幅集合.
        /// </summary>
        public ObservableCollection<Banner> BannerCollection { get; set; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        public ObservableCollection<Video> VideoCollection { get; set; }

        /// <summary>
        /// 显示出来的标签集合.
        /// </summary>
        public ObservableCollection<Tag> TagCollection { get; set; }
    }
}
