// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 直播线路视图模型.
    /// </summary>
    public class LivePlayLineViewModel : SelectableViewModelBase<LivePlayLine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlayLineViewModel"/> class.
        /// </summary>
        /// <param name="data">线路数据.</param>
        /// <param name="isSelected">是否被选中.</param>
        public LivePlayLineViewModel(LivePlayLine data, bool isSelected)
            : base(data, isSelected)
        {
        }
    }
}
