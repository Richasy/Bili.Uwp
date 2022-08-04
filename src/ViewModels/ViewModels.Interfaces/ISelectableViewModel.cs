// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 可选择的视图模型的接口定义.
    /// </summary>
    /// <typeparam name="T">数据类型.</typeparam>
    public interface ISelectableViewModel<T> : IInjectDataViewModel<T>
    {
        /// <summary>
        /// 是否被选中.
        /// </summary>
        bool IsSelected { get; set; }
    }
}
