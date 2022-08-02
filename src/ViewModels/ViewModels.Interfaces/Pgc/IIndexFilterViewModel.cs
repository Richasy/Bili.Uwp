// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Appearance;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// 索引过滤条件视图模型的接口定义.
    /// </summary>
    public interface IIndexFilterViewModel : IReactiveObject
    {
        /// <summary>
        /// 数据.
        /// </summary>
        Filter Data { get; }

        /// <summary>
        /// 选中项.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// 设置数据.
        /// </summary>
        /// <param name="data">筛选数据.</param>
        /// <param name="selectedItem">默认选中项.</param>
        void SetData(Filter data, Condition selectedItem = null);
    }
}
