// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Controller.Uwp;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
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
        /// 搜索建议集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<Bilibili.App.Interfaces.V1.ResultItem> SuggestionCollection { get; set; }

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
        /// 用户模块.
        /// </summary>
        [Reactive]
        public SearchSubModuleViewModel UserModule { get; set; }

        /// <summary>
        /// 直播模块.
        /// </summary>
        [Reactive]
        public SearchSubModuleViewModel LiveModule { get; set; }

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
