// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Search;
using Bili.ViewModels.Uwp.Core;
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
        private readonly NavigationViewModel _navigationViewModel;
        private readonly DispatcherTimer _suggestionTimer;
        private readonly CoreDispatcher _dispatcher;

        private CancellationTokenSource _suggestionCancellationTokenSource;
        private bool _isKeywordChanged;

        /// <summary>
        /// 热搜集合.
        /// </summary>
        public ObservableCollection<SearchSuggest> HotSearchCollection { get; }

        /// <summary>
        /// 搜索建议集合.
        /// </summary>
        public ObservableCollection<SearchSuggest> SearchSuggestion { get; }

        /// <summary>
        /// 执行搜索的命令.
        /// </summary>
        public ReactiveCommand<string, Unit> SearchCommand { get; }

        /// <summary>
        /// 选中搜索建议并执行搜索的命令.
        /// </summary>
        public ReactiveCommand<SearchSuggest, Unit> SelectSuggestCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <summary>
        /// 关键词.
        /// </summary>
        [Reactive]
        public string Keyword { get; set; }
    }
}
