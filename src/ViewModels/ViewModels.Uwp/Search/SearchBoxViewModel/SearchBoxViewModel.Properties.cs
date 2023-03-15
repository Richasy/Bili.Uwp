// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Search;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索框视图模型.
    /// </summary>
    public sealed partial class SearchBoxViewModel
    {
        private readonly ISearchProvider _searchProvider;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly DispatcherTimer _suggestionTimer;
        private readonly CoreDispatcher _dispatcher;

        private CancellationTokenSource _suggestionCancellationTokenSource;
        private bool _isKeywordChanged;

        [ObservableProperty]
        private string _keyword;

        /// <inheritdoc/>
        public ObservableCollection<SearchSuggest> HotSearchCollection { get; }

        /// <inheritdoc/>
        public ObservableCollection<SearchSuggest> SearchSuggestion { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<string> SearchCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<SearchSuggest> SelectSuggestCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }
    }
}
