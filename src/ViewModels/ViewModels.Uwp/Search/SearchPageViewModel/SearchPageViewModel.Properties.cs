// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Live;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索页面视图模型.
    /// </summary>
    public sealed partial class SearchPageViewModel
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IHomeProvider _homeProvider;
        private readonly IArticleProvider _articleProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly Dictionary<SearchModuleType, bool> _requestStatusCache;
        private readonly Dictionary<SearchModuleType, IEnumerable<SearchFilterViewModel>> _filters;
        private readonly ObservableAsPropertyHelper<bool> _isReloadingModule;

        /// <summary>
        /// 重载搜索模块命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ReloadModuleCommand { get; }

        /// <summary>
        /// 选中模块命令.
        /// </summary>
        public ReactiveCommand<SearchModuleItemViewModel, Unit> SelectModuleCommand { get; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        public ObservableCollection<VideoItemViewModel> Videos { get; }

        /// <summary>
        /// 动画集合.
        /// </summary>
        public ObservableCollection<SeasonItemViewModel> Animes { get; }

        /// <summary>
        /// 影视集合.
        /// </summary>
        public ObservableCollection<SeasonItemViewModel> Movies { get; }

        /// <summary>
        /// 用户集合.
        /// </summary>
        public ObservableCollection<UserItemViewModel> Users { get; }

        /// <summary>
        /// 文章集合.
        /// </summary>
        public ObservableCollection<ArticleItemViewModel> Articles { get; }

        /// <summary>
        /// 直播集合.
        /// </summary>
        public ObservableCollection<LiveItemViewModel> Lives { get; }

        /// <summary>
        /// 当前的过滤器集合.
        /// </summary>
        public ObservableCollection<SearchFilterViewModel> CurrentFilters { get; }

        /// <summary>
        /// 当前选中的模块.
        /// </summary>
        [Reactive]
        public SearchModuleItemViewModel CurrentModule { get; set; }

        /// <summary>
        /// 当前内容是否为空.
        /// </summary>
        [Reactive]
        public bool IsCurrentContentEmpty { get; set; }

        /// <summary>
        /// 当前的过滤器是否为空.
        /// </summary>
        [Reactive]
        public bool IsCurrentFilterEmpty { get; set; }

        /// <summary>
        /// 是否显示视频模块.
        /// </summary>
        [Reactive]
        public bool IsVideoModuleShown { get; set; }

        /// <summary>
        /// 是否显示动漫模块.
        /// </summary>
        [Reactive]
        public bool IsAnimeModuleShown { get; set; }

        /// <summary>
        /// 是否显示影视模块.
        /// </summary>
        [Reactive]
        public bool IsMovieModuleShown { get; set; }

        /// <summary>
        /// 是否显示文章模块.
        /// </summary>
        [Reactive]
        public bool IsArticleModuleShown { get; set; }

        /// <summary>
        /// 是否显示直播模块.
        /// </summary>
        [Reactive]
        public bool IsLiveModuleShown { get; set; }

        /// <summary>
        /// 是否显示用户模块.
        /// </summary>
        [Reactive]
        public bool IsUserModuleShown { get; set; }

        /// <summary>
        /// 关键词.
        /// </summary>
        [Reactive]
        public string Keyword { get; internal set; }

        /// <summary>
        /// 是否正在重载模块.
        /// </summary>
        public bool IsReloadingModule => _isReloadingModule.Value;
    }
}
