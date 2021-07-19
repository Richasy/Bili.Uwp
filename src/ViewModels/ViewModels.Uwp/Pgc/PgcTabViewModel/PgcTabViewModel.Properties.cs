// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC标签页视图模型.
    /// </summary>
    public partial class PgcTabViewModel
    {
        private int _offsetId;

        /// <summary>
        /// 横幅集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<BannerViewModel> BannerCollection { get; set; }

        /// <summary>
        /// 排行榜集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcModuleViewModel> RankCollection { get; set; }

        /// <summary>
        /// 模块集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcModuleViewModel> ModuleCollection { get; set; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }

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

        /// <summary>
        /// 是否显示视频列表.
        /// </summary>
        [Reactive]
        public bool IsShowVideo { get; set; }

        /// <summary>
        /// 是否显示排行榜.
        /// </summary>
        [Reactive]
        public bool IsShowRank { get; set; }

        /// <summary>
        /// 分区是否正在初始化加载.
        /// </summary>
        [Reactive]
        public bool IsPartitionInitializeLoading { get; set; }

        /// <summary>
        /// 标签页Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分区Id.
        /// </summary>
        public int PartitionId { get; set; }

        /// <summary>
        /// 是否已激活.
        /// </summary>
        public bool IsActivate { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PgcTabViewModel model && Id == model.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => Title;
    }
}
