// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 字幕索引条目视图模型.
    /// </summary>
    public class SubtitleIndexItemViewModel : SelectableViewModelBase<SubtitleIndexItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleIndexItemViewModel"/> class.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <param name="isSelected">是否选中.</param>
        public SubtitleIndexItemViewModel(SubtitleIndexItem data, bool isSelected)
            : base(data, isSelected)
        {
        }
    }
}
