// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 搜索文章视图.
    /// </summary>
    public sealed partial class SearchArticleView : SearchComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchArticleView"/> class.
        /// </summary>
        public SearchArticleView()
        {
            InitializeComponent();
        }

        private async void OnArticleRefreshButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.ArticleModule.InitializeRequestAsync();
        }
    }
}
