// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 搜索页面.
    /// </summary>
    public sealed partial class SearchPage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SearchModuleViewModel), typeof(SearchPage), new PropertyMetadata(SearchModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPage"/> class.
        /// </summary>
        public SearchPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.ViewModel.PropertyChanged += OnViewModelPropertyChangedAsync;
            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SearchModuleViewModel ViewModel
        {
            get { return (SearchModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                await ViewModel.SearchAsync();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckCurrentType();
        }

        private void OnNavItemInvokedAsync(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItemContainer as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            var vm = item.Tag as SearchSubModuleViewModel;
            ViewModel.CurrentType = vm.Type;
        }

        private async void OnViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentType))
            {
                CheckCurrentType();
                await ViewModel.ShowModuleAsync(ViewModel.CurrentType);
            }
        }

        private void CheckCurrentType()
        {
            VideoFilter.Visibility = Visibility.Collapsed;
            VideoView.Visibility = Visibility.Collapsed;
            BangumiView.Visibility = Visibility.Collapsed;
            MovieView.Visibility = Visibility.Collapsed;
            UserFilter.Visibility = Visibility.Collapsed;
            UserView.Visibility = Visibility.Collapsed;
            ArticleFilter.Visibility = Visibility.Collapsed;
            ArticleView.Visibility = Visibility.Collapsed;
            LiveView.Visibility = Visibility.Collapsed;
            FilterContainer.Visibility = Visibility.Collapsed;

            switch (ViewModel.CurrentType)
            {
                case Models.Enums.SearchModuleType.Video:
                    VideoFilter.Visibility = Visibility.Visible;
                    VideoView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = VideoNavItem;
                    FilterContainer.Visibility = Visibility.Visible;
                    break;
                case Models.Enums.SearchModuleType.Bangumi:
                    BangumiView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = BangumiNavItem;
                    break;
                case Models.Enums.SearchModuleType.Live:
                    LiveView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = LiveNavItem;
                    break;
                case Models.Enums.SearchModuleType.User:
                    UserFilter.Visibility = Visibility.Visible;
                    UserView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = UserNavItem;
                    FilterContainer.Visibility = Visibility.Visible;
                    break;
                case Models.Enums.SearchModuleType.Movie:
                    MovieView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = MovieNavItem;
                    break;
                case Models.Enums.SearchModuleType.Article:
                    ArticleFilter.Visibility = Visibility.Visible;
                    ArticleView.Visibility = Visibility.Visible;
                    FilterContainer.Visibility = Visibility.Visible;
                    Nav.SelectedItem = ArticleNavItem;
                    break;
                default:
                    break;
            }
        }
    }
}
