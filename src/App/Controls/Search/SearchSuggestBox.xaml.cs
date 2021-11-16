// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 搜索框.
    /// </summary>
    public sealed partial class SearchSuggestBox : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SearchModuleViewModel), typeof(SearchSuggestBox), new PropertyMetadata(SearchModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSuggestBox"/> class.
        /// </summary>
        public SearchSuggestBox()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SearchModuleViewModel ViewModel
        {
            get { return (SearchModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadHostSearchAsync();
        }

        private void OnHotItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as SearchRecommendItem;
            ViewModel.InputWords = item.Keyword;
            AppViewModel.Instance.SetOverlayContentId(Models.Enums.PageIds.Search);
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

        private void OnHotSearchOpening(object sender, object e)
        {
            if (!ViewModel.IsHotSearchFlyoutEnabled)
            {
                (sender as Flyout).Hide();
            }
        }

        private void OnSearchBoxSubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(sender.Text))
            {
                AppViewModel.Instance.SetOverlayContentId(Models.Enums.PageIds.Search);
            }
        }

        private void OnHotSearchButtonClick(object sender, RoutedEventArgs e)
        {
            HotSearchFlyout.ShowAt(AppSearchBox);
        }

        private async void AutoSuggestBox_TextChangedAsync(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && !string.IsNullOrEmpty(sender.Text))
            {
                await ViewModel.GetSearchSuggestTagAsync(sender.Text);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is SearchSuggestTag)
            {
                var tag = args.ChosenSuggestion as SearchSuggestTag;
                ViewModel.InputWords = tag.Value;
            }
            else if (!string.IsNullOrEmpty(args.QueryText))
            {
                ViewModel.InputWords = args.QueryText;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxSuggestionChosenEventArgs args)
        {
        }
    }
}
