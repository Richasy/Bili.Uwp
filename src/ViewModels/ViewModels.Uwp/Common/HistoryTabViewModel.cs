// Copyright (c) Richasy. All rights reserved.

using Bilibili.App.Interfaces.V1;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 历史记录标签页视图模型.
    /// </summary>
    public class HistoryTabViewModel : SelectableViewModelBase<CursorTab>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryTabViewModel"/> class.
        /// </summary>
        /// <param name="tab">标签页数据.</param>
        /// <param name="isSelected">是否选中.</param>
        public HistoryTabViewModel(CursorTab tab, bool isSelected)
            : base(tab, isSelected)
        {
        }
    }
}
