// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 稍后再看视图模型.
    /// </summary>
    public partial class ViewLaterViewModel
    {
        private int _pageNumber;
        private bool _isLoadCompleted;

        /// <summary>
        /// 实例.
        /// </summary>
        public static ViewLaterViewModel Instance { get; } = new Lazy<ViewLaterViewModel>(() => new ViewLaterViewModel()).Value;

        /// <summary>
        /// 视频集合.
        /// </summary>
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }

        /// <summary>
        /// 是否显示空白占位符.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 清空按钮是否可用.
        /// </summary>
        [Reactive]
        public bool IsClearButtonEnabled { get; set; }
    }
}
