// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Search;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 搜索框视图模型.
    /// </summary>
    public sealed partial class SearchBoxViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly ISearchProvider _searchProvider;
        private readonly DispatcherTimer _suggestionTimer;
        private readonly DispatcherQueue _dispatcher;

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
