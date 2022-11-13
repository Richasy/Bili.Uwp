// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.Data.Search;
using Bili.ViewModels.Interfaces.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.Uwp.App.Controls
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
            ViewModel = Locator.Instance.GetService<ISearchBoxViewModel>();
            DataContext = ViewModel;
        }

        private void OnHotItemClick(object sender, ItemClickEventArgs e)
        {
            SelectSuggestItem(e.ClickedItem);
            HotSearchFlyout.Hide();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;

            if (width > 16)
            {
                HotSearchListView.Width = width - 16;
            }
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

        private void OnHotSearchButtonClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.HotSearchCollection.Count > 0)
            {
                HotSearchFlyout.ShowAt(AppSearchBox);
            }
        }
    }

    /// <summary>
    /// <see cref="SearchSuggestBox"/> 的基类.
    /// </summary>
    public class SearchSuggestBoxBase : ReactiveUserControl<ISearchBoxViewModel>
    {
    }
}
