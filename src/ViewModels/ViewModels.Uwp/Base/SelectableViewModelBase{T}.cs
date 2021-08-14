// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 可选择的视图模型.
    /// </summary>
    /// <typeparam name="T">数据类型.</typeparam>
    public class SelectableViewModelBase<T> : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <param name="isSelected">是否被选中.</param>
        public SelectableViewModelBase(T data, bool isSelected = false)
        {
            Data = data;
            IsSelected = isSelected;
        }

        /// <summary>
        /// 核心数据.
        /// </summary>
        [Reactive]
        public T Data { get; set; }

        /// <summary>
        /// 是否被选中.
        /// </summary>
        [Reactive]
        public bool IsSelected { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SelectableViewModelBase<T> model && EqualityComparer<T>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => -301143667 + EqualityComparer<T>.Default.GetHashCode(Data);
    }
}
