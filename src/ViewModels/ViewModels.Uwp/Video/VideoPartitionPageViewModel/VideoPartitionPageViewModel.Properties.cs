// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 分区页面的视图模型.
    /// </summary>
    public sealed partial class VideoPartitionPageViewModel
    {
        private readonly IPartitionProvider _partitionProvider;
        private readonly ObservableAsPropertyHelper<bool> _isInitializing;

        /// <summary>
        /// 分区集合.
        /// </summary>
        public ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 初始化命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <summary>
        /// 是否正在初始化.
        /// </summary>
        public bool IsInitializing => _isInitializing.Value;
    }
}
