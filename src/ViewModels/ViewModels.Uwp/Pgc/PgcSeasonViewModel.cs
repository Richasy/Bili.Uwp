// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC剧集/系列视图模型.
    /// </summary>
    public class PgcSeasonViewModel : SelectableViewModelBase<PgcSeason>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcSeasonViewModel"/> class.
        /// </summary>
        /// <param name="season">剧集数据.</param>
        /// <param name="isSelected">是否被选中.</param>
        public PgcSeasonViewModel(PgcSeason season, bool isSelected)
            : base(season, isSelected)
        {
        }
    }
}
