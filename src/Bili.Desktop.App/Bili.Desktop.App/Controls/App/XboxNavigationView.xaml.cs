// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Desktop.App.Controls.Base;
using Bili.Desktop.App.Pages.Xbox;
using Bili.Desktop.App.Resources.Extension;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;

namespace Bili.Desktop.App.Controls.App
{
    /// <summary>
    /// 适用于 XBOX 的导航框架.
    /// </summary>
    public sealed partial class XboxNavigationView : NavigationViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XboxNavigationView"/> class.
        /// </summary>
        public XboxNavigationView()
            : base()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
            => ViewModel.Navigating += OnNavigating;

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => ViewModel.Navigating -= OnNavigating;

        private void OnNavigating(object sender, AppNavigationEventArgs e)
        {
            if (e.Type == Models.Enums.App.NavigationType.Main)
            {
                NavigateToMainView(e.PageId);
            }
        }

        private void NavigateToMainView(PageIds pageId, object parameter = null)
        {
            var pageType = pageId switch
            {
                PageIds.XboxAccount => typeof(XboxAccountPage),
                PageIds.PreSearch => typeof(PreSearchPage),
                PageIds.Recommend => typeof(RecommendPage),
                PageIds.Popular => typeof(PopularPage),
                PageIds.Rank => typeof(RankPage),
                PageIds.VideoPartition => typeof(VideoPartitionPage),
                PageIds.Bangumi => typeof(BangumiPage),
                PageIds.DomesticAnime => typeof(DomesticPage),
                PageIds.Documentary => typeof(DocumentaryPage),
                PageIds.Movie => typeof(MoviePage),
                PageIds.TV => typeof(TvPage),
                PageIds.Live => typeof(LiveFeedPage),
                PageIds.Dynamic => typeof(DynamicPage),
                PageIds.Settings => typeof(SettingsPage),
                _ => typeof(Page),
            };

            if (pageType != null)
            {
                MainFrame.Navigate(pageType, parameter, new DrillInNavigationTransitionInfo());
            }

            if (RootNavView.SelectedItem == null ||
                (RootNavView.SelectedItem is Microsoft.UI.Xaml.Controls.NavigationViewItem navItem &&
                NavigationExtension.GetPageId(navItem) != pageId))
            {
                var shouldSelectedItem = pageId == PageIds.Settings
                    ? RootNavView.SettingsItem as Microsoft.UI.Xaml.Controls.NavigationViewItem
                    : RootNavView.MenuItems.Concat(RootNavView.FooterMenuItems).OfType<Microsoft.UI.Xaml.Controls.NavigationViewItem>()
                                           .Where(p => NavigationExtension.GetPageId(p) == pageId).FirstOrDefault();

                RootNavView.SelectedItem = shouldSelectedItem;
            }

            if (RootNavView.SelectedItem != null && RootNavView.SelectedItem is Microsoft.UI.Xaml.Controls.NavigationViewItem selectItem)
            {
                AppViewModel.HeaderText = selectItem.Content.ToString();
            }
        }

        private async void OnRootNavViewItemInvokedAsync(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ViewModel.NavigateToMainView(PageIds.Settings);
            }
            else
            {
                if (args.InvokedItemContainer == SignInItem)
                {
                    await AccountViewModel.TrySignInCommand.ExecuteAsync(false);
                    if (AccountViewModel.State == AuthorizeState.SignedIn)
                    {
                        ViewModel.Navigate(PageIds.XboxAccount);
                    }

                    return;
                }

                var pageId = NavigationExtension.GetPageId(args.InvokedItemContainer);

                if (pageId != PageIds.None)
                {
                    ViewModel.Navigate(pageId);
                }
            }
        }

        private void OnFrameLoaded(object sender, RoutedEventArgs e)
        {
            if (!IsFirstLoaded)
            {
                FireFirstLoadedEvent();
                IsFirstLoaded = true;
            }
        }

        private void OnRootNavViewNoFocusCandidateFound(UIElement sender, NoFocusCandidateFoundEventArgs args)
        {
            if (args.Direction == FocusNavigationDirection.Left)
            {
                if (args.InputDevice == FocusInputDeviceKind.Keyboard ||
                args.InputDevice == FocusInputDeviceKind.GameController)
                {
                    RootNavView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Left;
                }

                args.Handled = true;
            }
        }

        private void OnRootNavViewLosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            if (RootNavView.PaneDisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Left
                && args.NewFocusedElement is not Microsoft.UI.Xaml.Controls.NavigationViewItem)
            {
                RootNavView.PaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.LeftCompact;
            }

            args.Handled = true;
        }
    }
}
