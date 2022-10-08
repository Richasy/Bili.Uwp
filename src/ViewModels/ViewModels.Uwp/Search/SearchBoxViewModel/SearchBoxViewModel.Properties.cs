﻿// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Search;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

        /// <inheritdoc/>
        public ObservableCollection<SearchSuggest> HotSearchCollection { get; }

        /// <inheritdoc/>
        public ObservableCollection<SearchSuggest> SearchSuggestion { get; }

        /// <inheritdoc/>
        public IRelayCommand<string> SearchCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<SearchSuggest> SelectSuggestCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string Keyword { get; set; }
    }
}
