// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Search;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Search;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Windows.System;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 搜索框视图模型.
    /// </summary>
    public sealed partial class SearchBoxViewModel : ViewModelBase, ISearchBoxViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBoxViewModel"/> class.
        /// </summary>
        public SearchBoxViewModel(
            ISearchProvider searchProvider,
            ISettingsToolkit settingsToolkit)
        {
            _searchProvider = searchProvider;
            _settingsToolkit = settingsToolkit;
            _dispatcher = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

            HotSearchCollection = new ObservableCollection<SearchSuggest>();
            SearchSuggestion = new ObservableCollection<SearchSuggest>();

            SearchCommand = new AsyncRelayCommand<string>(SearchAsync);
            SelectSuggestCommand = new AsyncRelayCommand<SearchSuggest>(SearchAsync);
            InitializeCommand = new AsyncRelayCommand(LoadHotSearchAsync);

            _suggestionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(350),
            };
            _suggestionTimer.Tick += OnSuggestionTimerTickAsync;

            AttachExceptionHandlerToAsyncCommand(LogException, InitializeCommand);
        }

        private Task SearchAsync(SearchSuggest suggest)
            => SearchAsync(suggest.Keyword);

        private async Task SearchAsync(string keyword)
        {
            Keyword = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                var perferLaunch = _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.LaunchType, LaunchType.Web);
                var word = WebUtility.UrlEncode(keyword);
                var uri = perferLaunch == LaunchType.Web
                    ? new Uri($"https://search.bilibili.com/all?keyword={word}")
                    : new Uri($"richasy-bili://find?keyword={word}");
                await Launcher.LaunchUriAsync(uri);
            }
        }

        private void InitializeSuggestionCancellationTokenSource()
        {
            if (_suggestionCancellationTokenSource != null
                && !_suggestionCancellationTokenSource.IsCancellationRequested)
            {
                _suggestionCancellationTokenSource.Cancel();
                _suggestionCancellationTokenSource = null;
            }

            _suggestionCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        }

        private async Task LoadSearchSuggestionAsync()
        {
            if (string.IsNullOrEmpty(Keyword))
            {
                return;
            }

            TryClear(SearchSuggestion);
            try
            {
                var suggestion = await _searchProvider.GetSearchSuggestion(Keyword, _suggestionCancellationTokenSource.Token);
                if (suggestion == null)
                {
                    return;
                }

                _dispatcher.TryEnqueue(() =>
                {
                    suggestion.ToList().ForEach(p => SearchSuggestion.Add(p));
                });
            }
            catch (TaskCanceledException)
            {
                // 任务中止表示有新的搜索请求或者请求超时，这是预期的错误，不予处理.
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private async Task LoadHotSearchAsync()
        {
            if (HotSearchCollection.Count > 0)
            {
                return;
            }

            var data = await _searchProvider.GetHotSearchListAsync();
            _dispatcher.TryEnqueue(() =>
            {
                TryClear(HotSearchCollection);
                data.ToList().ForEach(p => HotSearchCollection.Add(p));
            });
        }

        private void HandleKeywordChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                // 搜索关键词为空，表示用户或应用清除了内容，此时不进行请求，并重置状态.
                _isKeywordChanged = false;
                _suggestionTimer.Stop();
                InitializeSuggestionCancellationTokenSource();
                TryClear(SearchSuggestion);
            }
            else
            {
                _isKeywordChanged = true;
                if (!_suggestionTimer.IsEnabled)
                {
                    _suggestionTimer.Start();
                }
            }
        }

        private async void OnSuggestionTimerTickAsync(object sender, object e)
        {
            if (_isKeywordChanged)
            {
                _isKeywordChanged = false;
                InitializeSuggestionCancellationTokenSource();
                await LoadSearchSuggestionAsync();
            }
        }

        partial void OnKeywordChanged(string value)
            => HandleKeywordChanged(value);
    }
}
