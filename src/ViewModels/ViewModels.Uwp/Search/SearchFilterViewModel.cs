// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bili.Models.Data.Appearance;
using Bili.ViewModels.Interfaces.Search;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索过滤视图模型.
    /// </summary>
    public sealed class SearchFilterViewModel : ViewModelBase, ISearchFilterViewModel
    {
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
        public void SetData(Filter filter, Condition currentCondition = null)
        {
            Filter = filter;
            CurrentCondition = currentCondition ?? filter.Conditions.First();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SearchFilterViewModel model && EqualityComparer<Filter>.Default.Equals(Filter, model.Filter);

        /// <inheritdoc/>
        public override int GetHashCode() => Filter.GetHashCode();
    }
}
