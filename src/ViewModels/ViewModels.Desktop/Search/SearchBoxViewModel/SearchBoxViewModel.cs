// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Search;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Search;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace Bili.ViewModels.Desktop.Search
{
    /// <summary>
    /// 搜索框视图模型.
    /// </summary>
    public sealed partial class SearchBoxViewModel : ViewModelBase, ISearchBoxViewModel, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBoxViewModel"/> class.
        /// </summary>
        public SearchBoxViewModel(
            ISearchProvider searchProvider,
            INavigationViewModel navigationViewModel)
        {
            _searchProvider = searchProvider;
            _navigationViewModel = navigationViewModel;
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            HotSearchCollection = new ObservableCollection<SearchSuggest>();
            SearchSuggestion = new ObservableCollection<SearchSuggest>();

            SearchCommand = new RelayCommand<string>(Search);
            SelectSuggestCommand = new RelayCommand<SearchSuggest>(Search);
            InitializeCommand = new AsyncRelayCommand(LoadHotSearchAsync);

            _suggestionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(350),
            };
            _suggestionTimer.Tick += OnSuggestionTimerTickAsync;

            AttachExceptionHandlerToAsyncCommand(LogException, InitializeCommand);
        }

        /// <inheritdoc/>
        public void Dispose()
            => ((IDisposable)_suggestionCancellationTokenSource).Dispose();

        private void Search(SearchSuggest suggest)
            => Search(suggest.Keyword);

        private void Search(string keyword)
        {
            Keyword = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Search, keyword);
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

                _dispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
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
            _dispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
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
