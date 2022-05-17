// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using Bili.App.Pages;
using Bili.App.Pages.Overlay;
using Bili.App.Resources.Extension;
using Bili.Models.App;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Bili.App.Controls
{
    /// <summary>
    /// Root navigation view.
    /// </summary>
    public sealed partial class RootNavigationView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AppViewModel), typeof(RootNavigationView), new PropertyMetadata(AppViewModel.Instance));

        private readonly AccountViewModel _accountViewModel = AccountViewModel.Instance;
        private readonly INavigationViewModel _navigationViewModel;
        private bool _isFirstLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNavigationView"/> class.
        /// </summary>
        public RootNavigationView()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<INavigationViewModel>();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 在刚加在首页时发生.
        /// </summary>
        public event EventHandler FirstLoaded;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public AppViewModel ViewModel
        {
            get { return (AppViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnAppViewModelPropertyChanged;
            ViewModel.RequestOverlayNavigation -= OnRequestOverlayNavigation;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SecondaryFrame = OverlayFrame;
            ViewModel.PropertyChanged += OnAppViewModelPropertyChanged;
            ViewModel.RequestOverlayNavigation += OnRequestOverlayNavigation;
            _navigationViewModel.Navigating += OnNavigating;
            NavigateToMainView(ViewModel.CurrentMainContentId);
            if (ViewModel.IsXbox)
            {
                RootNavView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.LeftMinimal;
            }
        }

        private void OnNavigating(object sender, AppNavigationEventArgs e)
        {
            if (e.Type == Models.Enums.App.NavigationType.Main)
            {
                NavigateToMainView(e.PageId);
            }
            else if (e.Type == Models.Enums.App.NavigationType.Secondary)
            {
                ViewModel.SetOverlayContentId(e.PageId, e.Parameter);

                // NavigateToSecondaryView(e.PageId, e.Parameter);
            }
        }

        private void OnRequestOverlayNavigation(object sender, object e)
            => NavigateToSecondaryView(ViewModel.CurrentOverlayContentId, e);

        private void OnAppViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentMainContentId))
            {
                NavigateToMainView(ViewModel.CurrentMainContentId);
            }
            else if (e.PropertyName == nameof(ViewModel.IsShowOverlay))
            {
                if (!ViewModel.IsShowOverlay)
                {
                    if (OverlayFrame.Content != null)
                    {
                        OverlayFrame.Navigate(typeof(Page));
                    }

                    if (MainFrame.Content != null && MainFrame.Content is IConnectedAnimationPage animatePage)
                    {
                        animatePage.TryStartConnectedAnimation();
                    }
                }
            }
        }

        private void OnRootNavViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ViewModel.SetMainContentId(PageIds.Settings);
            }
            else
            {
                var pageId = NavigationExtension.GetPageId(args.InvokedItemContainer);
                ViewModel.SetMainContentId(pageId);
            }
        }

        private void NavigateToMainView(PageIds pageId, object parameter = null)
        {
            Type pageType = null;
            switch (pageId)
            {
                case PageIds.None:
                default:
                    break;
                case PageIds.Recommend:
                    pageType = typeof(RecommendPage);
                    break;
                case PageIds.Rank:
                    pageType = typeof(RankPage);
                    break;
                case PageIds.Partition:
                    pageType = typeof(PartitionPage);
                    break;
                case PageIds.Popular:
                    pageType = typeof(PopularPage);
                    break;
                case PageIds.SpecialColumn:
                    pageType = typeof(SpecialColumnPage);
                    break;
                case PageIds.Bangumi:
                    pageType = typeof(BangumiPage);
                    break;
                case PageIds.DomesticAnime:
                    pageType = typeof(DomesticAnimePage);
                    break;
                case PageIds.Movie:
                    pageType = typeof(MoviePage);
                    break;
                case PageIds.TV:
                    pageType = typeof(TVPage);
                    break;
                case PageIds.Documentary:
                    pageType = typeof(DocumentaryPage);
                    break;
                case PageIds.Live:
                    pageType = typeof(LivePage);
                    break;
                case PageIds.Dynamic:
                    pageType = typeof(DynamicFeedPage);
                    break;
                case PageIds.Help:
                    pageType = typeof(HelpPage);
                    break;
                case PageIds.Toolbox:
                    pageType = typeof(ToolboxPage);
                    break;
                case PageIds.Settings:
                    pageType = typeof(SettingPage);
                    break;
            }

            if (pageType != null)
            {
                MainFrame.Navigate(pageType, parameter, new DrillInNavigationTransitionInfo());
            }

            if (RootNavView.SelectedItem == null ||
                (RootNavView.SelectedItem is Microsoft.UI.Xaml.Controls.NavigationViewItem navItem &&
                NavigationExtension.GetPageId(navItem) != pageId))
            {
                Microsoft.UI.Xaml.Controls.NavigationViewItem shouldSelectedItem = null;
                if (pageId == PageIds.Settings)
                {
                    shouldSelectedItem = RootNavView.SettingsItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
                }
                else
                {
                    shouldSelectedItem = RootNavView.MenuItems.Concat(RootNavView.FooterMenuItems).OfType<Microsoft.UI.Xaml.Controls.NavigationViewItem>()
                    .Where(p => NavigationExtension.GetPageId(p) == pageId).FirstOrDefault();
                }

                RootNavView.SelectedItem = shouldSelectedItem;
            }

            if (RootNavView.SelectedItem != null && RootNavView.SelectedItem is Microsoft.UI.Xaml.Controls.NavigationViewItem selectItem)
            {
                ViewModel.HeaderText = selectItem.Content.ToString();
            }
        }

        private void NavigateToSecondaryView(PageIds pageId, object param)
        {
            Type pageType = null;

            switch (pageId)
            {
                case PageIds.PartitionDetail:
                    pageType = typeof(PartitionDetailPage);
                    break;
                case PageIds.Search:
                    pageType = typeof(SearchPage);
                    break;
                case PageIds.ViewHistory:
                    pageType = typeof(HistoryPage);
                    break;
                case PageIds.Favorite:
                    pageType = typeof(FavoritePage);
                    break;
                case PageIds.ViewLater:
                    pageType = typeof(ViewLaterPage);
                    break;
                case PageIds.Fans:
                    pageType = typeof(FansPage);
                    break;
                case PageIds.Follows:
                    pageType = typeof(FollowsPage);
                    break;
                case PageIds.PgcIndex:
                    pageType = typeof(PgcIndexPage);
                    break;
                case PageIds.TimeLine:
                    pageType = typeof(TimeLinePage);
                    break;
                case PageIds.Message:
                    pageType = typeof(MessagePage);
                    break;
                case PageIds.LiveAreaDetail:
                    pageType = typeof(LiveAreaDetailPage);
                    break;
                case PageIds.MyFollows:
                    pageType = typeof(MyFollowsPage);
                    break;
                default:
                    break;
            }

            if (pageType != null)
            {
                OverlayFrame.Navigate(pageType, param, new EntranceNavigationTransitionInfo());
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is IRefreshPage page)
            {
                RefreshButton.IsEnabled = false;
                await page.RefreshAsync();
                RefreshButton.IsEnabled = true;
            }
            else if (MainFrame.Content is AppPage appPage
                && appPage.GetViewModel() is IReloadViewModel reloadVM)
            {
                reloadVM.ReloadCommand.Execute().Subscribe();
            }
        }

        private void OnMainFrameNavigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (!_isFirstLoaded)
            {
                FirstLoaded?.Invoke(this, EventArgs.Empty);
                _isFirstLoaded = true;
            }

            RefreshButton.Visibility = MainFrame.Content is IRefreshPage ? Visibility.Visible : Visibility.Collapsed;

            if (MainFrame.Content is DynamicFeedPage)
            {
                DynamicNavView.Visibility = Visibility.Visible;
                DynamicNavView.SelectedItem = DynamicModuleViewModel.Instance.IsVideo ? VideoDynamicItem : AllDynamicItem;
            }
            else
            {
                DynamicNavView.Visibility = Visibility.Collapsed;
            }
        }

        private void OnDynamicNavViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
            => DynamicModuleViewModel.Instance.IsVideo = args.InvokedItemContainer.Equals(VideoDynamicItem);

        private async void OnFixedItemClickAsync(object sender, RoutedEventArgs e)
        {
            var context = (sender as FrameworkElement).DataContext as FixedItem;
            switch (context.Type)
            {
                case Models.Enums.App.FixedType.Publisher:
                    await UserView.Instance.ShowAsync(Convert.ToInt32(context.Id));
                    break;
                case Models.Enums.App.FixedType.Pgc:
                    {
                        var record = new CurrentPlayingRecord("0", Convert.ToInt32(context.Id), VideoType.Pgc)
                        {
                            Title = context.Title,
                        };
                        AppViewModel.Instance.OpenPlayer(record);
                    }

                    break;
                case Models.Enums.App.FixedType.Video:
                    {
                        var record = new CurrentPlayingRecord(context.Id, 0, VideoType.Video);
                        AppViewModel.Instance.OpenPlayer(record);
                    }

                    break;

                case Models.Enums.App.FixedType.Live:
                    {
                        var record = new CurrentPlayingRecord(context.Id, 0, VideoType.Live);
                        AppViewModel.Instance.OpenPlayer(record);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
