// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using Richasy.Bili.App.Pages;
using Richasy.Bili.App.Pages.Overlay;
using Richasy.Bili.App.Resources.Extension;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNavigationView"/> class.
        /// </summary>
        public RootNavigationView()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

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
            ViewModel.PropertyChanged -= this.OnAppViewModelPropertyChanged;
            ViewModel.RequestOverlayNavigation -= this.OnRequestOverlayNavigation;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += this.OnAppViewModelPropertyChanged;
            ViewModel.RequestOverlayNavigation += this.OnRequestOverlayNavigation;
            CheckMainContentNavigation();
        }

        private void OnRequestOverlayNavigation(object sender, object e)
        {
            CheckOverlayContentNavigation(e);
        }

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
                case PageIds.Help:
                    pageType = typeof(HelpPage);
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
                case PageIds.MyFavorite:
                    pageType = typeof(MyFavoritePage);
                    break;
                case PageIds.SeeLater:
                    pageType = typeof(SeeLaterPage);
                    break;
                case PageIds.DynamicFeed:
                    pageType = typeof(DynamicFeedPage);
                    break;
                default:
                    break;
            }

            if (pageType != null)
            {
                OverlayFrame.Navigate(pageType, param, new EntranceNavigationTransitionInfo());
            }
        }
    }
}
