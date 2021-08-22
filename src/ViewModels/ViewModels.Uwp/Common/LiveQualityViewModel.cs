// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 直播清晰度视图模型.
    /// </summary>
    public class LiveQualityViewModel : SelectableViewModelBase<LiveQualityDescription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveQualityViewModel"/> class.
        /// </summary>
        /// <param name="data">清晰度数据.</param>
        /// <param name="isSelected">是否选中.</param>
        public LiveQualityViewModel(LiveQualityDescription data, bool isSelected = false)
            : base(data, isSelected)
        {
        }
    }
}
