// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bilibili.App.Dynamic.V2;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 动态模块视图模型.
    /// </summary>
    public partial class DynamicModuleViewModel
    {
        private string _videoUpdateOffset;
        private string _videoBaseLine;
        private string _allUpdateOffset;
        private string _allBaseLine;
        private bool _isLoadCompleted;

        /// <summary>
        /// 实例.
        /// </summary>
        public static DynamicModuleViewModel Instance { get; } = new Lazy<DynamicModuleViewModel>(() => new DynamicModuleViewModel()).Value;

        /// <summary>
        /// 视频动态集合.
        /// </summary>
        public ObservableCollection<DynamicItem> VideoDynamicCollection { get; }

        /// <summary>
        /// 全部动态集合.
        /// </summary>
        public ObservableCollection<DynamicItem> AllDynamicCollection { get; }

        /// <summary>
        /// 是否为视频动态.
        /// </summary>
        [Reactive]
        public bool IsVideo { get; set; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 是否显示需要登录的提示.
        /// </summary>
        [Reactive]
        public bool IsShowLogin { get; set; }
    }
}
