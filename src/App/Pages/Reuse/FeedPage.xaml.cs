// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.App.Controls;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 数据源PGC页面.
    /// </summary>
    public sealed partial class FeedPage : AppPage, IRefreshPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(FeedPgcViewModelBase), typeof(FeedPage), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedPage"/> class.
        /// </summary>
        public FeedPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public FeedPgcViewModelBase ViewModel
        {
            get { return (FeedPgcViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        public async Task RefreshAsync()
        {
            await ViewModel.InitializeRequestAsync();
            ContentScrollViewer.ChangeView(0, 0, 1);
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PgcType type)
            {
                switch (type)
                {
                    case PgcType.Documentary:
                        ViewModel = DocumentaryViewModel.Instance;
                        break;
                    case PgcType.Movie:
                        ViewModel = MovieViewModel.Instance;
                        break;
                    case PgcType.TV:
                        ViewModel = TvViewModel.Instance;
                        break;
                    default:
                        break;
                }
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequestAsync();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }

        private async void OnFeedViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            if (!ViewModel.IsInitializeLoading && !ViewModel.IsDeltaLoading)
            {
                await ViewModel.DeltaRequestAsync();
            }
        }

        private void OnIndexButtonClick(object sender, RoutedEventArgs e)
        {
            AppViewModel.Instance.SetOverlayContentId(PageIds.PgcIndex, ViewModel);
        }
    }
}
