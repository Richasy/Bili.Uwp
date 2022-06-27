// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.App.Controls.Base;
using Bili.App.Pages.Xbox;
using Bili.App.Resources.Extension;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Bili.App.Controls.App
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
                PageIds.Recommend => typeof(RecommendPage),
                PageIds.Popular => typeof(PopularPage),
                PageIds.Rank => typeof(RankPage),
                PageIds.VideoPartition => throw new NotImplementedException(),
                PageIds.SpecialColumn => throw new NotImplementedException(),
                PageIds.Bangumi => throw new NotImplementedException(),
                PageIds.DomesticAnime => throw new NotImplementedException(),
                PageIds.Documentary => throw new NotImplementedException(),
                PageIds.Movie => throw new NotImplementedException(),
                PageIds.TV => throw new NotImplementedException(),
                PageIds.Live => throw new NotImplementedException(),
                PageIds.Dynamic => throw new NotImplementedException(),
                PageIds.Settings => throw new NotImplementedException(),
                PageIds.Help => throw new NotImplementedException(),
                PageIds.Toolbox => throw new NotImplementedException(),
                PageIds.Favorite => throw new NotImplementedException(),
                PageIds.ViewLater => throw new NotImplementedException(),
                PageIds.ViewHistory => throw new NotImplementedException(),
                PageIds.Message => throw new NotImplementedException(),
                PageIds.Fans => throw new NotImplementedException(),
                PageIds.Follows => throw new NotImplementedException(),
                PageIds.MyFollows => throw new NotImplementedException(),
                PageIds.VideoPartitionDetail => throw new NotImplementedException(),
                PageIds.Search => throw new NotImplementedException(),
                PageIds.PgcIndex => throw new NotImplementedException(),
                PageIds.TimeLine => throw new NotImplementedException(),
                PageIds.LivePartition => throw new NotImplementedException(),
                PageIds.LivePartitionDetail => throw new NotImplementedException(),
                PageIds.VideoPlayer => throw new NotImplementedException(),
                PageIds.PgcPlayer => throw new NotImplementedException(),
                PageIds.LivePlayer => throw new NotImplementedException(),
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

        private void OnRootNavViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ViewModel.NavigateToMainView(PageIds.Settings);
            }
            else
            {
                if (args.InvokedItemContainer == SignInItem)
                {
                    AccountViewModel.TrySignInCommand.Execute(false).Subscribe(
                        _ =>
                        {
                            if (AccountViewModel.State == AuthorizeState.SignedIn)
                            {
                                ViewModel.Navigate(PageIds.XboxAccount);
                            }
                        });
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
