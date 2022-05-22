// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls;
using Bili.App.Controls.Dialogs;
using Bili.App.Pages.Desktop.Overlay;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// The page is used for default loading.
    /// </summary>
    public sealed partial class RootPage : RootPageBase
    {
        private string _initialCommandParameters = null;
        private Uri _initialUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootPage"/> class.
        /// </summary>
        public RootPage()
        {
            InitializeComponent();
            Current = this;
            ViewModel.Navigating += OnNavigating;
            ViewModel.ExitPlayer += OnExitPlayer;
            Loaded += OnLoadedAsync;
            CoreViewModel.RequestShowTip += OnRequestShowTip;
            CoreViewModel.RequestShowUpdateDialog += OnRequestShowUpdateDialogAsync;
            CoreViewModel.RequestContinuePlay += OnRequestContinuePlayAsync;
            CoreViewModel.RequestShowImages += OnRequestShowImagesAsync;
            SizeChanged += OnSizeChanged;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        /// <summary>
        /// 根页面实例.
        /// </summary>
        public static RootPage Current { get; private set; }

        /// <summary>
        /// 显示顶层视图.
        /// </summary>
        /// <param name="element">要显示的元素.</param>
        public void ShowOnHolder(UIElement element)
        {
            if (!HolderContainer.Children.Contains(element))
            {
                HolderContainer.Children.Add(element);
            }

            HolderContainer.Visibility = Visibility.Visible;
            CoreViewModel.CanShowBackButton = false;

            ViewModel.AddBackStack(
                BackBehavior.ShowHolder,
                ele =>
                {
                    RemoveFromHolder((UIElement)ele);
                },
                element);
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is CommandLineActivatedEventArgs command)
            {
                _initialCommandParameters = command.Operation.Arguments;
            }
            else if (e.Parameter is IProtocolActivatedEventArgs protocol)
            {
                _initialUri = protocol.Uri;
            }
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            var kind = e.GetCurrentPoint(this).Properties.PointerUpdateKind;
            if (kind == Windows.UI.Input.PointerUpdateKind.XButton1Released
                || kind == Windows.UI.Input.PointerUpdateKind.MiddleButtonReleased)
            {
                e.Handled = true;
                Back();
            }

            base.OnPointerReleased(e);
        }

        private void OnNavigating(object sender, AppNavigationEventArgs e)
        {
            if (e.Type == NavigationType.Secondary)
            {
                var type = GetSecondaryViewType(e.PageId);
                SecondaryFrame.Navigate(type, e.Parameter, new DrillInNavigationTransitionInfo());
            }
            else if (e.Type == NavigationType.Player)
            {
                PlayerFrame.Navigate(typeof(PlayerPage), e.Parameter);
            }
            else if (e.Type == NavigationType.Main && SecondaryFrame.Content is AppPage)
            {
                SecondaryFrame.Navigate(typeof(Page));
            }
        }

        private void OnExitPlayer(object sender, EventArgs e)
            => PlayerFrame.Navigate(typeof(Page));

        /// <summary>
        /// 从顶层视图中移除元素.
        /// </summary>
        /// <param name="element">UI元素.</param>
        private void RemoveFromHolder(UIElement element)
        {
            HolderContainer.Children.Remove(element);
            CoreViewModel.CanShowBackButton = HolderContainer.Children.Count == 0;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            CoreViewModel.InitializePadding();
            await AccountViewModel.Instance.TrySignInAsync(true);
#if !DEBUG
            await CoreViewModel.CheckUpdateAsync();
#endif
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            Back();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
            => CoreViewModel.InitializePadding();

        private async void OnRequestShowImagesAsync(object sender, ShowImageEventArgs e)
        {
            var viewer = ImageViewer.Instance ?? new ImageViewer();
            if (e != null && e.ImageUrls?.Count != 0)
            {
                ShowOnHolder(viewer);
                await viewer.LoadImagesAsync(e.ImageUrls, e.ShowIndex);
            }
        }

        private void OnRequestShowTip(object sender, AppTipNotificationEventArgs e)
            => new TipPopup(e.Message).ShowAsync(e.Type);

        private async void OnRequestShowUpdateDialogAsync(object sender, UpdateEventArgs e)
        {
            try
            {
                await new UpgradeDialog(e).ShowAsync();
            }
            catch (Exception)
            {
            }
        }

        private async void OnRequestContinuePlayAsync(object sender, EventArgs e)
        {
            try
            {
                await new ContinuePlayDialog().ShowAsync();
            }
            catch (Exception)
            {
            }
        }

        private Type GetSecondaryViewType(PageIds pageId)
        {
            var pageType = pageId switch
            {
                PageIds.VideoPartitionDetail => typeof(VideoPartitionDetailPage),
                PageIds.Search => typeof(SearchPage),
                PageIds.ViewHistory => typeof(HistoryPage),
                PageIds.Favorite => typeof(FavoritePage),
                PageIds.ViewLater => typeof(ViewLaterPage),
                PageIds.Fans => typeof(FansPage),
                PageIds.Follows => typeof(FollowsPage),
                PageIds.PgcIndex => typeof(PgcIndexPage),
                PageIds.TimeLine => typeof(TimeLinePage),
                PageIds.Message => typeof(MessagePage),
                PageIds.LivePartition => typeof(LivePartitionPage),
                PageIds.LivePartitionDetail => typeof(LivePartitionDetailPage),
                PageIds.MyFollows => typeof(MyFollowsPage),
                _ => typeof(Page)
            };

            return pageType;
        }

        private void Back()
        {
            if (ViewModel.CanBack)
            {
                ViewModel.BackCommand.Execute().Subscribe();
            }
            else if (CoreViewModel.IsXbox)
            {
                Application.Current.Exit();
            }
        }

        private async void OnRootNavViewFirstLoadAsync(object sender, EventArgs e)
        {
            ViewModel.NavigateToMainView(PageIds.Recommend);
            if (!string.IsNullOrEmpty(_initialCommandParameters))
            {
                await CoreViewModel.InitializeCommandFromArgumentsAsync(_initialCommandParameters);
                _initialCommandParameters = null;
            }
            else if (_initialUri != null)
            {
                await CoreViewModel.InitializeProtocolFromQueryAsync(_initialUri);
                _initialUri = null;
            }
            else
            {
                CoreViewModel.CheckContinuePlay();
            }

            await CoreViewModel.CheckNewDynamicRegistrationAsync();
        }
    }

    /// <summary>
    /// <see cref="RootPage"/> 的基类.
    /// </summary>
    public class RootPageBase : AppPage<NavigationViewModel>
    {
    }
}
