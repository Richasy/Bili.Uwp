// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bili.Models.Data.Appearance;
using DynamicData;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC索引筛选视图模型.
    /// </summary>
    public sealed class IndexFilterViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexFilterViewModel"/> class.
        /// </summary>
        /// <param name="data">筛选数据.</param>
        /// <param name="selectedItem">默认选中项.</param>
        public IndexFilterViewModel(Filter data, Condition selectedItem = null)
        {
            Data = data;
            SelectedIndex = selectedItem == null ? 0 : data.Conditions.IndexOf(selectedItem);
        }

        /// <summary>
        /// 数据.
        /// </summary>
        public Filter Data { get; }

        /// <summary>
        /// 选中项.
        /// </summary>
        [Reactive]
        public int SelectedIndex { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is IndexFilterViewModel model && EqualityComparer<Filter>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}
