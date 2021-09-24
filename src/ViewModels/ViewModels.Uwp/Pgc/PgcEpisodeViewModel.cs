// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC单集视图模型.
    /// </summary>
    public class PgcEpisodeViewModel : SelectableViewModelBase<PgcEpisodeDetail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcEpisodeViewModel"/> class.
        /// </summary>
        /// <param name="detail">单集详情.</param>
        /// <param name="isSelected">是否选中.</param>
        public PgcEpisodeViewModel(PgcEpisodeDetail detail, bool isSelected)
            : base(detail, isSelected)
        {
        }
    }
}
