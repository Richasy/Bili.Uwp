// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bili.Models.Data.Appearance;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC索引筛选视图模型.
    /// </summary>
    public sealed partial class IndexFilterViewModel : ViewModelBase, IIndexFilterViewModel
    {
        [ObservableProperty]
        private Filter _data;

        [ObservableProperty]
        private int _selectedIndex;

        /// <inheritdoc/>
        public void SetData(Filter data, Condition selectedItem = null)
        {
            Data = data;
            SelectedIndex = selectedItem == null ? 0 : data.Conditions.ToList().IndexOf(selectedItem);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is IndexFilterViewModel model && EqualityComparer<Filter>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}
