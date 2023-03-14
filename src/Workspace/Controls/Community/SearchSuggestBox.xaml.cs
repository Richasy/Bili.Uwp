// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Search;
using Bili.ViewModels.Interfaces.Search;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Workspace.Controls.Community
{
    /// <summary>
    /// 搜索框.
    /// </summary>
    public sealed partial class SearchSuggestBox : SearchSuggestBoxBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSuggestBox"/> class.
        /// </summary>
        public SearchSuggestBox()
        {
            InitializeComponent();
        }

        private void OnSearchBoxSubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is SearchSuggest)
            {
                SelectSuggestItem(args.ChosenSuggestion);
            }

            if (!string.IsNullOrEmpty(sender.Text))
            {
                ViewModel.SearchCommand.Execute(sender.Text);
            }
        }

        private void SelectSuggestItem(object suggestObj)
        {
            var data = suggestObj as SearchSuggest;
            ViewModel.SelectSuggestCommand.Execute(data);
        }
    }

    /// <summary>
    /// <see cref="SearchSuggestBox"/> 的基类.
    /// </summary>
    public class SearchSuggestBoxBase : ReactiveUserControl<ISearchBoxViewModel>
    {
    }
}
