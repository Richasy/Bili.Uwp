// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播分区页面视图模型.
    /// </summary>
    public sealed partial class LivePartitionPageViewModel
    {
        private readonly ILiveProvider _liveProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly CoreDispatcher _dispatcher;
        private readonly List<Partition> _partitions;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;

        /// <summary>
        /// 父分区集合.
        /// </summary>
        public ObservableCollection<Partition> ParentPartitions { get; }

        /// <summary>
        /// 显示的分区集合.
        /// </summary>
        public ObservableCollection<Partition> DisplayPartitions { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 选择分区命令.
        /// </summary>
        public ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前选中的父分区.
        /// </summary>
        [Reactive]
        public Partition CurrentParentPartition { get; set; }

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
        /// 是否正在初始化.
        /// </summary>
        public bool IsReloading => _isReloading.Value;
    }
}
