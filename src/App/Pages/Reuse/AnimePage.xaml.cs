// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.App.Controls;
using Bili.Models.Enums;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages
{
    /// <summary>
    /// 动漫页面，属于番剧和国创的共有页面.
    /// </summary>
    public sealed partial class AnimePage : AppPage, IRefreshPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AnimeViewModelBase), typeof(AnimePage), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimePage"/> class.
        /// </summary>
        public AnimePage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public AnimeViewModelBase ViewModel
        {
            get { return (AnimeViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        public Task RefreshAsync()
            => ViewModel.CurrentTab.InitializePartitionRequestAsync();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PgcType type)
            {
                switch (type)
                {
                    case PgcType.Bangumi:
                        ViewModel = BangumiViewModel.Instance;
                        break;
                    case PgcType.Domestic:
                        ViewModel = DomesticViewModel.Instance;
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

            ViewModel.IsShowMyFavoriteButton = AccountViewModel.Instance.Status == AccountViewModelStatus.Login;
            CheckCurrentTabAsync(true);
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
            => await RefreshAsync();

        private async void CheckCurrentTabAsync(bool needDelay = false)
        {
            if (!(RootNavView.SelectedItem is PgcTabViewModel selectedItem) || selectedItem != ViewModel.CurrentTab)
            {
                if (needDelay)
                {
                    await Task.Delay(100);
                }

                RootNavView.SelectedItem = ViewModel.CurrentTab;
            }
        }

        private void OnRootNavViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            ViewModel.CurrentTab = args.InvokedItem as PgcTabViewModel;
            CheckCurrentTabAsync();
        }

        private async void OnShowMoreButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as Button).Tag as PgcModuleViewModel;
            if (vm.Id > 0)
            {
                await new PgcPlayListView().ShowAsync(vm.Id);
            }
        }

        private void OnIndexButtonClick(object sender, RoutedEventArgs e)
            => AppViewModel.Instance.SetOverlayContentId(PageIds.PgcIndex, ViewModel);

        private void OnTimeChartButtonClick(object sender, RoutedEventArgs e)
            => AppViewModel.Instance.SetOverlayContentId(PageIds.TimeLine, ViewModel);

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
            => await ViewModel.CurrentTab.DeltaPartitionRequestAsync();

        private async void OnMyFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var accVM = AccountViewModel.Instance;

            if (accVM.Status == AccountViewModelStatus.Login)
            {
                FavoriteViewModel.Instance.SetUser(accVM.Mid.Value, accVM.DisplayName);
                FavoriteViewModel.Instance.CurrentType = Models.Enums.App.FavoriteType.Anime;
                AppViewModel.Instance.SetOverlayContentId(PageIds.Favorite);
            }
            else
            {
                await accVM.TrySignInAsync();
            }
        }
    }
}
