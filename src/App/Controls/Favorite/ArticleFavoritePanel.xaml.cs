// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 文章收藏夹.
    /// </summary>
    public sealed partial class ArticleFavoritePanel : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(FavoriteArticleViewModel), typeof(ArticleFavoritePanel), new PropertyMetadata(FavoriteArticleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleFavoritePanel"/> class.
        /// </summary>
        public ArticleFavoritePanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public FavoriteArticleViewModel ViewModel
        {
            get { return (FavoriteArticleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnArticleRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }
    }
}
