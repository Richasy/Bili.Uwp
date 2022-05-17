// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.BiliBili;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 子分区视图模型属性集.
    /// </summary>
    public partial class PartitionViewModel
    {
        private readonly Partition _partition;

        /// <summary>
        /// 分区Logo.
        /// </summary>
        [Reactive]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 分区名称.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 子分区集合.
        /// </summary>
        public ObservableCollection<SubPartitionViewModel> SubPartitionCollection { get; set; }

        /// <summary>
        /// 分区Id.
        /// </summary>
        public int PartitionId => _partition?.Tid ?? 0;

        /// <summary>
        /// 当前选中的子分区.
        /// </summary>
        [Reactive]
        public SubPartitionViewModel CurrentSelectedSubPartition { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PartitionViewModel model && PartitionId == model.PartitionId;

        /// <inheritdoc/>
        public override int GetHashCode() => -352390548 + PartitionId.GetHashCode();
    }
}
