// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Appearance;
using Bili.ViewModels.Interfaces.Pgc;
using DynamicData;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC索引筛选视图模型.
    /// </summary>
    public sealed class IndexFilterViewModel : ViewModelBase, IIndexFilterViewModel
    {
        /// <inheritdoc/>
        [ObservableProperty]
        public Filter Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public int SelectedIndex { get; set; }

        /// <inheritdoc/>
        public void SetData(Filter data, Condition selectedItem = null)
        {
            Data = data;
            SelectedIndex = selectedItem == null ? 0 : data.Conditions.IndexOf(selectedItem);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is IndexFilterViewModel model && EqualityComparer<Filter>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}
