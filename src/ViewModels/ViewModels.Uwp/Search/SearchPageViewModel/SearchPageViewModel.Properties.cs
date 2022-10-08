// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Live;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Search;
using Bili.ViewModels.Interfaces.Video;
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
        private readonly IAppViewModel _appViewModel;
        private readonly Dictionary<SearchModuleType, bool> _requestStatusCache;
        private readonly Dictionary<SearchModuleType, IEnumerable<ISearchFilterViewModel>> _filters;

        /// <inheritdoc/>
        public IRelayCommand ReloadModuleCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<ISearchModuleItemViewModel> SelectModuleCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> Videos { get; }

        /// <inheritdoc/>
        public ObservableCollection<ISeasonItemViewModel> Animes { get; }

        /// <inheritdoc/>
        public ObservableCollection<ISeasonItemViewModel> Movies { get; }

        /// <inheritdoc/>
        public ObservableCollection<IUserItemViewModel> Users { get; }

        /// <inheritdoc/>
        public ObservableCollection<IArticleItemViewModel> Articles { get; }

        /// <inheritdoc/>
        public ObservableCollection<ILiveItemViewModel> Lives { get; }

        /// <inheritdoc/>
        public ObservableCollection<ISearchFilterViewModel> CurrentFilters { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public ISearchModuleItemViewModel CurrentModule { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsCurrentContentEmpty { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsCurrentFilterEmpty { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsVideoModuleShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsAnimeModuleShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsMovieModuleShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsArticleModuleShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsLiveModuleShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsUserModuleShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string Keyword { get; internal set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloadingModule { get; set; }
    }
}
