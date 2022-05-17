// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 分区内容连接处理视图模型.
    /// </summary>
    public sealed partial class PartitionServiceViewModel
    {
        private readonly INavigationViewModel _navigation;

        /// <summary>
        /// 返回主视图命令已发起.
        /// </summary>
        /// <remarks>
        /// 这个事件可以通知主视图启动连接动画.
        /// </remarks>
        public event EventHandler<Partition> BackRequested;

        /// <summary>
        /// 进入详情视图的命令.
        /// </summary>
        public ReactiveCommand<Partition, Unit> GotoDetailViewCommand { get; }
    }
}
