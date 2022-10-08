// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.ViewModels.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 可选择的视图模型.
    /// </summary>
    /// <typeparam name="T">数据类型.</typeparam>
    public class SelectableViewModelBase<T> : ViewModelBase, ISelectableViewModel<T>
    {
        /// <inheritdoc/>
        [ObservableProperty]
        public T Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSelected { get; set; }

        /// <inheritdoc/>
        public void InjectData(T data)
            => Data = data;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SelectableViewModelBase<T> model && EqualityComparer<T>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}
