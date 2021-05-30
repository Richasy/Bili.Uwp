// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 分区视图模型属性集.
    /// </summary>
    public partial class PartitionViewModel
    {
        /// <summary>
        /// <see cref="PartitionViewModel"/>的单例.
        /// </summary>
        public static PartitionViewModel Instance { get; } = new Lazy<PartitionViewModel>(() => new PartitionViewModel()).Value;

        /// <summary>
        /// 分区索引集合.
        /// </summary>
        public ObservableCollection<Partition> PartitionCollection { get; }

        /// <summary>
        /// 是否正在加载.
        /// </summary>
        [Reactive]
        public bool IsLoading { get; set; }
    }
}
