// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Search;
using Bili.ViewModels.Uwp.Search;

namespace Bili.App.Pages.Xbox
{
    /// <summary>
    /// 预搜索页面.
    /// </summary>
    public sealed partial class PreSearchPage : PreSearchPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreSearchPage"/> class.
        /// </summary>
        public PreSearchPage() => InitializeComponent();

        private void OnSuggstItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
            => SelectSuggestItem(e.ClickedItem);

        private void OnHotItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
            => SelectSuggestItem(e.ClickedItem);

        private void OnSearchBoxSubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(sender.Text))
            {
                ViewModel.SearchCommand.Execute(sender.Text).Subscribe();
            }
        }

        private void SelectSuggestItem(object suggestObj)
        {
            var data = suggestObj as SearchSuggest;
            ViewModel.SelectSuggestCommand.Execute(data).Subscribe();
        }
    }

    /// <summary>
    /// <see cref="PreSearchPage"/> 的基类.
    /// </summary>
    public class PreSearchPageBase : AppPage<SearchBoxViewModel>
    {
    }
}
