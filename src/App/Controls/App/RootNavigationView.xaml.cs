// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using Richasy.Bili.App.Pages;
using Richasy.Bili.App.Pages.Overlay;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.Models.App;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Controls
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
        private bool _isFirstLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNavigationView"/> class.
        /// </summary>
        public RootNavigationView()
        {
            InitializeComponent();
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
            ViewModel.PropertyChanged += OnAppViewModelPropertyChanged;
            ViewModel.RequestOverlayNavigation += OnRequestOverlayNavigation;
            CheckMainContentNavigation();
            if (ViewModel.IsXbox)
            {
                RootNavView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.LeftMinimal;
            }
        }

        private void OnRequestOverlayNavigation(object sender, object e)
            => CheckOverlayContentNavigation(e);

        private void OnAppViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentMainContentId))
            {
                CheckMainContentNavigation();
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

        private void CheckMainContentNavigation()
        {
            var pageId = ViewModel.CurrentMainContentId;
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
                MainFrame.Navigate(pageType, null, new DrillInNavigationTransitionInfo());
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

        private void CheckOverlayContentNavigation(object param)
        {
            var pageId = ViewModel.CurrentOverlayContentId;
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

        private async void OnFixedPublisherClickAsync(object sender, RoutedEventArgs e)
        {
            var context = (sender as FrameworkElement).DataContext as FixedPublisher;
            await UserView.Instance.ShowAsync(Convert.ToInt32(context.UserId));
        }

        private void OnFixedPgcClickAsync(object sender, RoutedEventArgs e)
        {
            var context = (sender as FrameworkElement).DataContext as FixedPgc;
            var record = new CurrentPlayingRecord("0", context.SeasonId, VideoType.Pgc)
            {
                Title = context.Name,
            };
            AppViewModel.Instance.OpenPlayer(record);
        }

        private void OnDynamicNavViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            DynamicModuleViewModel.Instance.IsVideo = args.InvokedItemContainer.Equals(VideoDynamicItem);
        }
    }
}
