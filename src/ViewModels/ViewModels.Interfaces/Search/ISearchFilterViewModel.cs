// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Data.Appearance;

namespace Bili.ViewModels.Interfaces.Search
{
    /// <summary>
    /// 搜索过滤视图模型的接口定义.
    /// </summary>
    public interface ISearchFilterViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 筛选器.
        /// </summary>
        public Filter Filter { get; }

        /// <summary>
        /// 当前值.
        /// </summary>
        public Condition CurrentCondition { get; set; }

        /// <summary>
        /// 设置数据.
        /// </summary>
        /// <param name="filter">过滤器.</param>
        /// <param name="currentCondition">当前选中的条件.</param>
        void SetData(Filter filter, Condition currentCondition = default);
    }
}
