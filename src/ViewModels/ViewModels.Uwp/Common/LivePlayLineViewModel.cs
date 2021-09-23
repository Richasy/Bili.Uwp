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
        /// <param name="data">播放线路数据.</param>
        /// <param name="isSelected">是否被选中.</param>
        /// <param name="sid">会话标识Id.</param>
        public LivePlayLineViewModel(LivePlayLine data, bool isSelected = false, string sid = null)
            : base(data, isSelected)
        {
            if (!data.Url.Contains("sid") && !string.IsNullOrEmpty(sid))
            {
                data.Url += $"&sid={sid}";
            }
        }
    }
}
