// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Search
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

        [ObservableProperty]
        private ISearchModuleItemViewModel _currentModule;

        [ObservableProperty]
        private bool _isCurrentContentEmpty;

        [ObservableProperty]
        private bool _isCurrentFilterEmpty;

        [ObservableProperty]
        private bool _isVideoModuleShown;

        [ObservableProperty]
        private bool _isAnimeModuleShown;

        [ObservableProperty]
        private bool _isMovieModuleShown;

        [ObservableProperty]
        private bool _isArticleModuleShown;

        [ObservableProperty]
        private bool _isLiveModuleShown;

        [ObservableProperty]
        private bool _isUserModuleShown;

        [ObservableProperty]
        private string _keyword;

        [ObservableProperty]
        private bool _isReloadingModule;

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadModuleCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<ISearchModuleItemViewModel> SelectModuleCommand { get; }

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
    }
}
