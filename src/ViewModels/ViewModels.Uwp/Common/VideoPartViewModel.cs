// Copyright (c) GodLeaveMe. All rights reserved.

using Bilibili.App.View.V1;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 分P视图模型.
    /// </summary>
    public class VideoPartViewModel : SelectableViewModelBase<ViewPage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartViewModel"/> class.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <param name="isSelected">是否被选中.</param>
        public VideoPartViewModel(ViewPage data, bool isSelected = false)
            : base(data, isSelected)
        {
        }
    }
}
