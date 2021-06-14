// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 分区视图模型属性集.
    /// </summary>
    public partial class PartitionModuleViewModel
    {
        private readonly BiliController _controller;

        /// <summary>
        /// <see cref="PartitionModuleViewModel"/>的单例.
        /// </summary>
        public static PartitionModuleViewModel Instance { get; } = new Lazy<PartitionModuleViewModel>(() => new PartitionModuleViewModel()).Value;

        /// <summary>
        /// 分区索引集合.
        /// </summary>
        public ObservableCollection<PartitionViewModel> PartitionCollection { get; }

        /// <summary>
        /// 是否正在加载.
        /// </summary>
        [Reactive]
        public bool IsLoading { get; set; }

        /// <summary>
        /// 是否出现了错误以至于无法显示分区索引.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 当前选中的分区.
        /// </summary>
        [Reactive]
        public PartitionViewModel CurrentPartition { get; set; }
    }
}
