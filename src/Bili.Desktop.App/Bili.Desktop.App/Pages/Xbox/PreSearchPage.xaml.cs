// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Search;
using Bili.ViewModels.Interfaces.Search;

namespace Bili.Desktop.App.Pages.Xbox
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

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnSuggstItemClick(object sender, Microsoft.UI.Xaml.Controls.ItemClickEventArgs e)
            => SelectSuggestItem(e.ClickedItem);

        private void OnHotItemClick(object sender, Microsoft.UI.Xaml.Controls.ItemClickEventArgs e)
            => SelectSuggestItem(e.ClickedItem);

        private void OnSearchBoxSubmitted(Microsoft.UI.Xaml.Controls.AutoSuggestBox sender, Microsoft.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
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
    /// <see cref="PreSearchPage"/> 的基类.
    /// </summary>
    public class PreSearchPageBase : AppPage<ISearchBoxViewModel>
    {
    }
}
