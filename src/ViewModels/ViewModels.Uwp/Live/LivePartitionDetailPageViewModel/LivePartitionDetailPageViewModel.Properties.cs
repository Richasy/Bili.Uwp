// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Live;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播分区详情页面视图模型.
    /// </summary>
    public sealed partial class LivePartitionDetailPageViewModel
    {
        private readonly ILiveProvider _liveProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly Dictionary<LiveTag, IEnumerable<LiveInformation>> _caches;

        private int _totalCount;

        /// <summary>
        /// 分区标签集合.
        /// </summary>
        public ObservableCollection<LiveTag> Tags { get; }

        /// <summary>
        /// 选中标签命令.
        /// </summary>
        public ReactiveCommand<LiveTag, Unit> SelectTagCommand { get; }

        /// <summary>
        /// 查看全部分区命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SeeAllPartitionsCommand { get; }

        /// <summary>
        /// 始分区.
        /// </summary>
        [Reactive]
        public Partition OriginPartition { get; private set; }

        /// <summary>
        /// 当前选中的标签.
        /// </summary>
        [Reactive]
        public LiveTag CurrentTag { get; set; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }
    }
}
