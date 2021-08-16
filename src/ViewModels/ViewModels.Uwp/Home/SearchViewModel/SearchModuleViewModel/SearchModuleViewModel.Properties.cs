// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

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
        /// 视频模块.
        /// </summary>
        [Reactive]
        public SearchSubModuleViewModel VideoModule { get; set; }

        /// <summary>
        /// 番剧模块.
        /// </summary>
        [Reactive]
        public SearchSubModuleViewModel BangumiModule { get; set; }

        /// <summary>
        /// 电影电视剧模块.
        /// </summary>
        [Reactive]
        public SearchSubModuleViewModel MovieModule { get; set; }

        /// <summary>
        /// 专栏文章模块.
        /// </summary>
        [Reactive]
        public SearchSubModuleViewModel ArticleModule { get; set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [Reactive]
        public string Keyword { get; set; }

        /// <summary>
        /// 与用户交互的输入关键词.
        /// </summary>
        [Reactive]
        public string InputWords { get; set; }

        /// <summary>
        /// 是否启用热搜.
        /// </summary>
        [Reactive]
        public bool IsHotSearchFlyoutEnabled { get; set; }

        /// <summary>
        /// 当前选中的索引.
        /// </summary>
        [Reactive]
        public SearchModuleType CurrentType { get; set; }

        private BiliController Controller { get; }
    }
}
