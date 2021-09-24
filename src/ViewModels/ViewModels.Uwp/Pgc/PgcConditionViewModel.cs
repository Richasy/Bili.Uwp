// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC条件视图模型.
    /// </summary>
    public class PgcConditionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcConditionViewModel"/> class.
        /// </summary>
        /// <param name="filter">筛选类型.</param>
        public PgcConditionViewModel(PgcIndexFilter filter)
            : this()
        {
            Name = filter.Name;
            Id = filter.Field;
            filter.Values.ForEach(p => ConditionCollection.Add(new PgcConditionItemViewModel(p)));
            SelectedItem = ConditionCollection.First();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcConditionViewModel"/> class.
        /// </summary>
        /// <param name="orders">排序集合.</param>
        public PgcConditionViewModel(List<PgcIndexOrder> orders)
            : this()
        {
            Name = "排序方式";
            Id = "order";
            orders.ForEach(p => ConditionCollection.Add(new PgcConditionItemViewModel(p)));
            SelectedItem = ConditionCollection.First();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcConditionViewModel"/> class.
        /// </summary>
        public PgcConditionViewModel()
        {
            ConditionCollection = new ObservableCollection<PgcConditionItemViewModel>();
        }

        /// <summary>
        /// 条件集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcConditionItemViewModel> ConditionCollection { get; set; }

        /// <summary>
        /// 选中项.
        /// </summary>
        [Reactive]
        public PgcConditionItemViewModel SelectedItem { get; set; }

        /// <summary>
        /// 条件名.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <summary>
        /// 条件Id.
        /// </summary>
        [Reactive]
        public string Id { get; set; }
    }

    /// <summary>
    /// PGC条件条目视图模型.
    /// </summary>
    public class PgcConditionItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcConditionItemViewModel"/> class.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <param name="isSelected">是否选中.</param>
        public PgcConditionItemViewModel(PgcIndexFilterValue data)
        {
            Name = data.Name;
            Id = data.Keyword;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcConditionItemViewModel"/> class.
        /// </summary>
        /// <param name="data">排序条目.</param>
        public PgcConditionItemViewModel(PgcIndexOrder data)
        {
            Name = data.Name;
            Id = data.Field;
        }

        /// <summary>
        /// 条目名.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <summary>
        /// 条目值.
        /// </summary>
        [Reactive]
        public string Id { get; set; }
    }
}
