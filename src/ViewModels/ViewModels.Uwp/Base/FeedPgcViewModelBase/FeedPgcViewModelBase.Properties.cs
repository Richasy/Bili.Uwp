// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 数据源式PGC视图模型基类.
    /// </summary>
    public partial class FeedPgcViewModelBase
    {
        private string _cursor;

        /// <summary>
        /// 横幅集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<BannerViewModel> BannerCollection { get; set; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SeasonViewModel> SeasonCollection { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        [Reactive]
        public bool IsShowBanner { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FeedPgcViewModelBase model && Type == model.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Type.GetHashCode();
    }
}
