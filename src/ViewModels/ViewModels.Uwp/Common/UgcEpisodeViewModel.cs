// Copyright (c) Richasy. All rights reserved.

using Bilibili.App.View.V1;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频合集单话视图模型.
    /// </summary>
    public class UgcEpisodeViewModel : SelectableViewModelBase<Episode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UgcEpisodeViewModel"/> class.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <param name="isSelected">是否被选中.</param>
        public UgcEpisodeViewModel(Episode data, bool isSelected = false)
            : base(data, isSelected)
        {
        }
    }
}
