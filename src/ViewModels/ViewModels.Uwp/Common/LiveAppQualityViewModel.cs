// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 应用直播清晰度视图模型.
    /// </summary>
    public class LiveAppQualityViewModel : SelectableViewModelBase<LiveAppQualityDescription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveQualityViewModel"/> class.
        /// </summary>
        /// <param name="data">清晰度数据.</param>
        /// <param name="isSelected">是否选中.</param>
        public LiveAppQualityViewModel(LiveAppQualityDescription data, bool isSelected = false)
            : base(data, isSelected)
        {
        }
    }
}
