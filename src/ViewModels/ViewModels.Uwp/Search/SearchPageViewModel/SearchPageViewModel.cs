// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Live;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索页码视图模型.
    /// </summary>
    public sealed partial class SearchPageViewModel : InformationFlowViewModelBase<SearchModuleItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPageViewModel"/> class.
        /// </summary>
        public SearchPageViewModel(
            ISearchProvider searchProvider,
            IResourceToolkit resourceToolkit,
            IHomeProvider homeProvider,
            IArticleProvider articleProvider,
            ISettingsToolkit settingsToolkit,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _searchProvider = searchProvider;
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
            _articleProvider = articleProvider;
            _settingsToolkit = settingsToolkit;

            _requestStatusCache = new Dictionary<SearchModuleType, bool>();
            _filters = new Dictionary<SearchModuleType, IEnumerable<SearchFilterViewModel>>();

            Videos = new ObservableCollection<VideoItemViewModel>();
            Animes = new ObservableCollection<SeasonItemViewModel>();
            Movies = new ObservableCollection<SeasonItemViewModel>();
            Users = new ObservableCollection<UserItemViewModel>();
            Articles = new ObservableCollection<ArticleItemViewModel>();
            Lives = new ObservableCollection<LiveItemViewModel>();
            CurrentFilters = new ObservableCollection<SearchFilterViewModel>();

            ReloadModuleCommand = ReactiveCommand.CreateFromTask(ReloadModuleAsync, outputScheduler: RxApp.MainThreadScheduler);
            SelectModuleCommand = ReactiveCommand.CreateFromTask<SearchModuleItemViewModel>(SelectModuleAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isReloadingModule = ReloadModuleCommand.IsExecuting
                .Merge(SelectModuleCommand.IsExecuting)
                .ToProperty(this, x => x.IsReloadingModule, scheduler: RxApp.MainThreadScheduler);

            ReloadModuleCommand.ThrownExceptions
                .Merge(SelectModuleCommand.ThrownExceptions)
                .Subscribe(ex => DisplayException(ex));

            this.WhenAnyValue(x => x.CurrentModule)
                .WhereNotNull()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => CheckModuleVisibility());
        }

        /// <summary>
        /// 设置搜索关键词.
        /// </summary>
        /// <param name="keyword">关键词.</param>
        public void SetKeyword(string keyword)
        {
            Keyword = keyword;
            TryClear(Items);
            BeforeReload();
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _requestStatusCache.Clear();
            _filters.Clear();
            TryClear(Videos);
            TryClear(Animes);
            TryClear(Articles);
            TryClear(Movies);
            TryClear(Users);
            TryClear(Lives);
            _searchProvider.ClearStatus();
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (Items.Count == 0)
            {
                InitializeSearchModules();
                await SelectModuleAsync(Items.First());
            }
            else
            {
                var moduleType = CurrentModule.Type;
                if (CurrentFilters.Count == 0)
                {
                    if (!_filters.ContainsKey(moduleType))
                    {
                        await InitializeFiltersAsync(moduleType);
                    }

                    var filters = _filters[moduleType];
                    filters.ToList().ForEach(p => CurrentFilters.Add(p));
                }

                IsCurrentFilterEmpty = CurrentFilters.Count == 0;

                if (_requestStatusCache.TryGetValue(moduleType, out var isEnd) && isEnd)
                {
                    return;
                }

                await RequestDataAsync();
            }
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestSearchResultFailed)}\n{errorMsg}";

        private async Task SelectModuleAsync(SearchModuleItemViewModel vm)
        {
            ClearException();
            TryClear(CurrentFilters);
            await FakeLoadingAsync();
            CurrentModule = vm;
            ClearCurrentModule();
            await GetDataAsync();
        }

        private async Task ReloadModuleAsync()
        {
            ClearException();
            ClearCurrentModule();
            await RequestDataAsync();
        }

        private void InitializeSearchModules()
        {
            TryClear(Items);
            Items.Add(new SearchModuleItemViewModel(SearchModuleType.Video, _resourceToolkit.GetLocaleString(LanguageNames.Video)));
            Items.Add(new SearchModuleItemViewModel(SearchModuleType.Anime, _resourceToolkit.GetLocaleString(LanguageNames.Anime)));
            Items.Add(new SearchModuleItemViewModel(SearchModuleType.Live, _resourceToolkit.GetLocaleString(LanguageNames.Live)));
            Items.Add(new SearchModuleItemViewModel(SearchModuleType.User, _resourceToolkit.GetLocaleString(LanguageNames.User)));
            Items.Add(new SearchModuleItemViewModel(SearchModuleType.Movie, _resourceToolkit.GetLocaleString(LanguageNames.Movie)));

            if (!Locator.Current.GetService<AppViewModel>().IsXbox)
            {
                Items.Add(new SearchModuleItemViewModel(SearchModuleType.Article, _resourceToolkit.GetLocaleString(LanguageNames.SpecialColumn)));
            }
        }

        private void ClearCurrentModule()
        {
            switch (CurrentModule.Type)
            {
                case SearchModuleType.Video:
                    TryClear(Videos);
                    _searchProvider.ResetComprehensiveStatus();
                    break;
                case SearchModuleType.Anime:
                    TryClear(Animes);
                    _searchProvider.ResetAnimeStatus();
                    break;
                case SearchModuleType.Live:
                    TryClear(Lives);
                    _searchProvider.ResetLiveStatus();
                    break;
                case SearchModuleType.User:
                    TryClear(Users);
                    _searchProvider.ResetUserStatus();
                    break;
                case SearchModuleType.Movie:
                    TryClear(Movies);
                    _searchProvider.ResetMovieStatus();
                    break;
                case SearchModuleType.Article:
                    TryClear(Articles);
                    _searchProvider.ResetArticleStatus();
                    break;
                default:
                    break;
            }
        }

        private void CheckModuleContentEmpty()
        {
            IsCurrentContentEmpty = CurrentModule.Type switch
            {
                SearchModuleType.Video => Videos.Count == 0,
                SearchModuleType.Anime => Animes.Count == 0,
                SearchModuleType.Live => Lives.Count == 0,
                SearchModuleType.User => Users.Count == 0,
                SearchModuleType.Movie => Movies.Count == 0,
                SearchModuleType.Article => Articles.Count == 0,
                _ => true,
            };
        }

        private void CheckModuleVisibility()
        {
            IsVideoModuleShown = CurrentModule.Type == SearchModuleType.Video;
            IsAnimeModuleShown = CurrentModule.Type == SearchModuleType.Anime;
            IsMovieModuleShown = CurrentModule.Type == SearchModuleType.Movie;
            IsArticleModuleShown = CurrentModule.Type == SearchModuleType.Article;
            IsLiveModuleShown = CurrentModule.Type == SearchModuleType.Live;
            IsUserModuleShown = CurrentModule.Type == SearchModuleType.User;
        }
    }
}
