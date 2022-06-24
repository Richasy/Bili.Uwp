// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 数字组件视图模型.
    /// </summary>
    public class NumberPartViewModel : SelectableViewModelBase<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberPartViewModel"/> class.
        /// </summary>
        /// <param name="i">索引.</param>
        /// <param name="isSelected">是否已选中.</param>
        public NumberPartViewModel(int i, bool isSelected)
            : base(i, isSelected)
        {
        }
    }
}
