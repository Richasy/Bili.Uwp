// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 搜索模块视图模型.
    /// </summary>
    public partial class SearchModuleViewModel
    {
        /// <summary>
        /// 单例.
        /// </summary>
        public static SearchModuleViewModel Instance { get; } = new Lazy<SearchModuleViewModel>(() => new SearchModuleViewModel()).Value;

        /// <summary>
        /// 热搜条目集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SearchRecommendItem> HotSearchCollection { get; set; }

        /// <summary>
        /// 模块集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SearchSubModuleViewModel> ModuleCollection { get; set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [Reactive]
        public string Keyword { get; set; }

        /// <summary>
        /// 是否启用热搜.
        /// </summary>
        [Reactive]
        public bool IsHotSearchFlyoutEnabled { get; set; }

        private BiliController Controller { get; }
    }
}
