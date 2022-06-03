// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bili.Models.Data.Appearance;
using Bili.Models.Enums;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索过滤视图模型.
    /// </summary>
    public sealed class SearchFilterViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchFilterViewModel"/> class.
        /// </summary>
        /// <param name="filter">过滤器.</param>
        /// <param name="currentCondition">当前选中的条件.</param>
        public SearchFilterViewModel(
            Filter filter,
            Condition currentCondition = default)
        {
            Filter = filter;
            CurrentCondition = currentCondition ?? filter.Conditions.First();
        }

        /// <summary>
        /// 筛选器.
        /// </summary>
        [Reactive]
        public Filter Filter { get; set; }

        /// <summary>
        /// 当前值.
        /// </summary>
        [Reactive]
        public Condition CurrentCondition { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SearchFilterViewModel model && EqualityComparer<Filter>.Default.Equals(Filter, model.Filter);

        /// <inheritdoc/>
        public override int GetHashCode() => Filter.GetHashCode();
    }
}
