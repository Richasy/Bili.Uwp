// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Live;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Search;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Search
{
    /// <summary>
    /// 搜索页面视图模型.
    /// </summary>
    public sealed partial class SearchPageViewModel : InformationFlowViewModelBase<ISearchModuleItemViewModel>, ISearchPageViewModel
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
            IAppViewModel appViewModel)
        {
            _searchProvider = searchProvider;
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
            _articleProvider = articleProvider;
            _settingsToolkit = settingsToolkit;
            _appViewModel = appViewModel;

            _requestStatusCache = new Dictionary<SearchModuleType, bool>();
            _filters = new Dictionary<SearchModuleType, IEnumerable<ISearchFilterViewModel>>();

            Videos = new ObservableCollection<IVideoItemViewModel>();
            Animes = new ObservableCollection<ISeasonItemViewModel>();
            Movies = new ObservableCollection<ISeasonItemViewModel>();
            Users = new ObservableCollection<IUserItemViewModel>();
            Articles = new ObservableCollection<IArticleItemViewModel>();
            Lives = new ObservableCollection<ILiveItemViewModel>();
            CurrentFilters = new ObservableCollection<ISearchFilterViewModel>();

            ReloadModuleCommand = new AsyncRelayCommand(ReloadModuleAsync);
            SelectModuleCommand = new AsyncRelayCommand<ISearchModuleItemViewModel>(SelectModuleAsync);

            AttachIsRunningToAsyncCommand(p => IsReloadingModule = p, ReloadModuleCommand, SelectModuleCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayException, ReloadModuleCommand, SelectModuleCommand);
        }

        /// <inheritdoc/>
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

        private async Task SelectModuleAsync(ISearchModuleItemViewModel vm)
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
            Items.Add(GetModuleItemViewModel(SearchModuleType.Video, _resourceToolkit.GetLocaleString(LanguageNames.Video)));
            Items.Add(GetModuleItemViewModel(SearchModuleType.Anime, _resourceToolkit.GetLocaleString(LanguageNames.Anime)));
            Items.Add(GetModuleItemViewModel(SearchModuleType.Live, _resourceToolkit.GetLocaleString(LanguageNames.Live)));
            Items.Add(GetModuleItemViewModel(SearchModuleType.User, _resourceToolkit.GetLocaleString(LanguageNames.User)));
            Items.Add(GetModuleItemViewModel(SearchModuleType.Movie, _resourceToolkit.GetLocaleString(LanguageNames.Movie)));

            if (!_appViewModel.IsXbox)
            {
                Items.Add(GetModuleItemViewModel(SearchModuleType.Article, _resourceToolkit.GetLocaleString(LanguageNames.SpecialColumn)));
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

        private ISearchModuleItemViewModel GetModuleItemViewModel(SearchModuleType type, string title, bool isEnabled = true)
        {
            var vm = Locator.Instance.GetService<ISearchModuleItemViewModel>();
            vm.SetData(type, title, isEnabled);
            return vm;
        }

        partial void OnCurrentModuleChanged(ISearchModuleItemViewModel value)
        {
            if (value != null)
            {
                CheckModuleVisibility();
            }
        }
    }
}
