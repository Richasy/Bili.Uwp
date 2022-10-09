// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.ViewModels.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 可选择的视图模型.
    /// </summary>
    /// <typeparam name="T">数据类型.</typeparam>
    public partial class SelectableViewModelBase<T> : ViewModelBase, ISelectableViewModel<T>
    {
        [ObservableProperty]
        private T _data;

        [ObservableProperty]
        private bool _isSelected;

        /// <inheritdoc/>
        public void InjectData(T data)
            => Data = data;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SelectableViewModelBase<T> model && EqualityComparer<T>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}
