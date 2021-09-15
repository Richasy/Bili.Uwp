// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bilibili.App.Interfaces.V1;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 历史记录视图模型.
    /// </summary>
    public partial class HistoryViewModel
    {
        private Cursor _cursor;
        private bool _isLoadCompleted;

        /// <summary>
        /// 单例.
        /// </summary>
        public static HistoryViewModel Instance { get; } = new Lazy<HistoryViewModel>(() => new HistoryViewModel()).Value;

        /// <summary>
        /// 视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }

        /// <summary>
        /// 清除按钮是否可用.
        /// </summary>
        [Reactive]
        public bool IsClearButtonEnabled { get; set; }

        /// <summary>
        /// 是否显示空白占位符.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }
    }
}
