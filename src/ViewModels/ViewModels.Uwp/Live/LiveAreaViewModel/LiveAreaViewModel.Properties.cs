// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.BiliBili;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播间分区视图模型.
    /// </summary>
    public sealed partial class LiveAreaViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;

        private LiveArea _area;
        private int _currentPage;
        private bool _isFirstSetTag = false;
        private bool _isFinish = false;

        /// <summary>
        /// <see cref="LiveAreaViewModel"/>的单例.
        /// </summary>
        public static LiveAreaViewModel Instance { get; } = new Lazy<LiveAreaViewModel>(() => new LiveAreaViewModel()).Value;

        /// <summary>
        /// 子标签集合.
        /// </summary>
        public ObservableCollection<LiveAreaDetailTag> TagCollection { get; }

        /// <summary>
        /// 直播间集合.
        /// </summary>
        public ObservableCollection<VideoViewModel> LiveRoomCollection { get; }

        /// <summary>
        /// 选中的子标签.
        /// </summary>
        [Reactive]
        public LiveAreaDetailTag SelectedTag { get; set; }

        /// <summary>
        /// 分区标志.
        /// </summary>
        [Reactive]
        public string AreaImage { get; set; }

        /// <summary>
        /// 分区名.
        /// </summary>
        [Reactive]
        public string AreaName { get; set; }

        /// <summary>
        /// 是否没有找到直播间.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }
    }
}
