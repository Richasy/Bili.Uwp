// Copyright (c) Richasy. All rights reserved.

using System;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC时间线条目视图模型.
    /// </summary>
    public class PgcTimeLineItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcTimeLineItemViewModel"/> class.
        /// </summary>
        /// <param name="item">条目.</param>
        /// <param name="isSelected">是否选中.</param>
        public PgcTimeLineItemViewModel(PgcTimeLineItem item)
        {
            IsToday = item.IsToday == 1;
        }

        /// <summary>
        /// 让视图进入界面.
        /// </summary>
        public event EventHandler BringIntoView;

        /// <summary>
        /// 是否是今天.
        /// </summary>
        [Reactive]
        public bool IsToday { get; set; }

        /// <summary>
        /// 发送进入视图事件.
        /// </summary>
        public void RaiseBringEvent()
        {
            BringIntoView?.Invoke(this, EventArgs.Empty);
        }
    }
}
