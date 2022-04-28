// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums.App;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC收藏夹视图模型.
    /// </summary>
    public partial class PgcFavoriteViewModelBase
    {
        private int _pageNumber;
        private bool _isCompleted;

        /// <summary>
        /// 收藏夹类型.
        /// </summary>
        [Reactive]
        public FavoriteType Type { get; set; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        public ObservableCollection<SeasonViewModel> SeasonCollection { get; }

        /// <summary>
        /// 状态集合.
        /// </summary>
        public ObservableCollection<int> StatusCollection { get; }

        /// <summary>
        /// 状态.
        /// </summary>
        [Reactive]
        public int Status { get; set; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }
    }
}
