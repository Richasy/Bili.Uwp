// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.App.Other;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 排行榜视图模型的属性集.
    /// </summary>
    public partial class RankViewModel
    {
        private readonly Dictionary<RankPartition, List<VideoViewModel>> _cachedRankData;

        /// <summary>
        /// <see cref="RankViewModel"/>的静态实例.
        /// </summary>
        public static RankViewModel Instance { get; } = new Lazy<RankViewModel>(() => new RankViewModel()).Value;

        /// <summary>
        /// 当前显示的视频列表.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> DisplayVideoCollection { get; set; }

        /// <summary>
        /// 排行榜分区集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<RankPartition> PartitionCollection { get; set; }

        /// <summary>
        /// 当前选中的分区.
        /// </summary>
        [Reactive]
        public RankPartition CurrentPartition { get; set; }

        /// <summary>
        /// 分区是否正在加载.
        /// </summary>
        [Reactive]
        public bool IsRankLoading { get; set; }
    }
}
