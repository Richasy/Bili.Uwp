// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 动漫页面，属于番剧和国创的共有页面.
    /// </summary>
    public sealed partial class AnimePage : Page
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
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
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

            CheckCurrentTabAsync(true);
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            if (!ViewModel.CurrentTab.IsInitializeLoading && !ViewModel.CurrentTab.IsDeltaLoading)
            {
                await ViewModel.CurrentTab.DeltaPartitionRequestAsync();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.CurrentTab.IsInitializeLoading && !ViewModel.CurrentTab.IsDeltaLoading)
            {
                await ViewModel.CurrentTab.InitializePartitionRequestAsync();
            }
        }

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

        private void OnShowMoreButtonClickAsync(object sender, RoutedEventArgs e)
        {
        }

        private void OnIndexButtonClick(object sender, RoutedEventArgs e)
        {
            AppViewModel.Instance.SetOverlayContentId(PageIds.PgcIndex, ViewModel);
        }

        private void OnTimeChartButtonClick(object sender, RoutedEventArgs e)
        {
            AppViewModel.Instance.SetOverlayContentId(PageIds.TimeLine, ViewModel);
        }
    }
}
