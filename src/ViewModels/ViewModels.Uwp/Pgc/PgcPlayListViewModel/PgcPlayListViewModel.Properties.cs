// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC播放列表视图模型.
    /// </summary>
    public partial class PgcPlayListViewModel
    {
        /// <summary>
        /// 实例.
        /// </summary>
        public static PgcPlayListViewModel Instance { get; } = new Lazy<PgcPlayListViewModel>(() => new PgcPlayListViewModel()).Value;

        /// <summary>
        /// 播放列表Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 显示标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [Reactive]
        public string TotalCount { get; set; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SeasonViewModel> SeasonCollection { get; set; }
    }
}
