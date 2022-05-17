// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 排行榜页面视图模型.
    /// </summary>
    public sealed partial class RankPageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IPartitionProvider _partitionProvider;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly Dictionary<Partition, IEnumerable<VideoInformation>> _caches;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 选择分区命令.
        /// </summary>
        public ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前的分区.
        /// </summary>
        [Reactive]
        public Partition CurrentPartition { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 请求过程中出现了问题.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 全部分区.
        /// </summary>
        public ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 当前展示的视频集合.
        /// </summary>
        public ObservableCollection<VideoItemViewModel> VideoCollection { get; }

        /// <summary>
        /// 是否正在加载.
        /// </summary>
        public bool IsReloading => _isReloading?.Value ?? false;
    }
}
