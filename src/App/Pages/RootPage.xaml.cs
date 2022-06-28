// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.App.Controls;
using Bili.App.Controls.App;
using Bili.App.Controls.Article;
using Bili.App.Controls.Base;
using Bili.App.Controls.Community;
using Bili.App.Controls.Dialogs;
using Bili.App.Pages.Desktop.Overlay;
using Bili.Models.App.Args;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Pgc;
using Splat;
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
            Loaded += OnLoaded;
            CoreViewModel.RequestShowTip += OnRequestShowTip;
            CoreViewModel.RequestShowUpdateDialog += OnRequestShowUpdateDialogAsync;
            CoreViewModel.RequestContinuePlay += OnRequestContinuePlayAsync;
            CoreViewModel.RequestShowImages += OnRequestShowImagesAsync;
            CoreViewModel.RequestShowPgcPlaylist += OnRequestShowPgcPlaylist;
            CoreViewModel.RequestShowArticleReader += OnRequestShowArticleReaderAsync;
            CoreViewModel.RequestShowReplyDetail += OnRequestShowReplyDetail;
            CoreViewModel.RequestShowUserDetail += OnRequestShowUserDetail;
            CoreViewModel.RequestShowVideoFavoriteFolderDetail += OnRequestShowVideoFavoriteFolderDetail;
            CoreViewModel.RequestShowPgcSeasonDetail += OnRequestShowPgcSeasonDetail;
            SizeChanged += OnSizeChanged;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            NavigationViewBase navView = CoreViewModel.IsXbox
                ? new XboxNavigationView()
                : new DesktopNavigationView();
            navView.FirstLoaded += OnRootNavViewFirstLoadAsync;
            NavViewPresenter.Content = navView;
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

            ViewModel.AddBackStack(
                BackBehavior.ShowHolder,
                ele =>
                {
                    RemoveFromHolder((UIElement)ele);
                },
                element);

            if (element is Control e)
            {
                e.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// 显示提示信息，并在指定延时后关闭.
        /// </summary>
        /// <param name="element">要插入的元素.</param>
        /// <param name="delaySeconds">延时时间(秒).</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowTipAsync(UIElement element, double delaySeconds)
        {
            TipContainer.Visibility = Visibility.Visible;
            TipContainer.Children.Add(element);
            element.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            element.Visibility = Visibility.Collapsed;
            TipContainer.Children.Remove(element);
            if (TipContainer.Children.Count == 0)
            {
                TipContainer.Visibility = Visibility.Collapsed;
            }
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

        private void OnNavigating(object sender, AppNavigationEventArgs e)
        {
            if (e.Type == NavigationType.Secondary)
            {
                var type = GetSecondaryViewType(e.PageId);
                PlayerFrame.Navigate(typeof(Page));
                SecondaryFrame.Navigate(type, e.Parameter, new DrillInNavigationTransitionInfo());
            }
            else if (e.Type == NavigationType.Player)
            {
                var type = GetPlayerViewType(e.PageId);
                PlayerFrame.Navigate(type, e.Parameter);
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
            => HolderContainer.Children.Remove(element);

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CoreViewModel.InitializePadding();
            Locator.Current.GetService<AccountViewModel>().TrySignInCommand.Execute(true).Subscribe();
#if !DEBUG
            CoreViewModel.CheckUpdateCommand.Execute().Subscribe();
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
            if (e != null && e.Images?.Count() != 0)
            {
                ShowOnHolder(viewer);
                await viewer.LoadImagesAsync(e.Images, e.ShowIndex);
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

        private void OnRequestShowPgcPlaylist(object sender, PgcPlaylistViewModel e)
        {
            var view = new PgcPlayListDetailView
            {
                ViewModel = e,
            };
            view.Show();
        }

        private async void OnRequestShowArticleReaderAsync(object sender, ArticleItemViewModel e)
        {
            var view = ArticleReaderView.Instance;
            await view.ShowAsync(e);
        }

        private void OnRequestShowVideoFavoriteFolderDetail(object sender, VideoFavoriteFolder e)
            => new Controls.VideoFavoriteView().Show(e);

        private void OnRequestShowReplyDetail(object sender, ShowCommentEventArgs e)
        {
            var commentPopup = new CommentPopup();
            commentPopup.Show(e);
        }

        private void OnRequestShowUserDetail(object sender, UserItemViewModel e)
            => new UserSpaceView().Show(e.User);

        private void OnRequestShowPgcSeasonDetail(object sender, EventArgs e)
            => new PgcSeasonDetailView().Show();

        private Type GetSecondaryViewType(PageIds pageId)
        {
            var pageType = pageId switch
            {
                PageIds.VideoPartitionDetail => CoreViewModel.IsXbox
                    ? typeof(Xbox.Overlay.VideoPartitionDetailPage)
                    : typeof(Desktop.Overlay.VideoPartitionDetailPage),
                PageIds.Search => typeof(SearchPage),
                PageIds.ViewHistory => typeof(HistoryPage),
                PageIds.Favorite => CoreViewModel.IsXbox
                    ? typeof(Xbox.Overlay.FavoritePage)
                    : typeof(Desktop.Overlay.FavoritePage),
                PageIds.ViewLater => typeof(ViewLaterPage),
                PageIds.Fans => typeof(FansPage),
                PageIds.Follows => typeof(FollowsPage),
                PageIds.PgcIndex => typeof(PgcIndexPage),
                PageIds.TimeLine => typeof(TimelinePage),
                PageIds.Message => typeof(MessagePage),
                PageIds.LivePartition => CoreViewModel.IsXbox
                    ? typeof(Xbox.Overlay.LivePartitionPage)
                    : typeof(Desktop.Overlay.LivePartitionPage),
                PageIds.LivePartitionDetail => CoreViewModel.IsXbox
                    ? typeof(Xbox.Overlay.LivePartitionDetailPage)
                    : typeof(Desktop.Overlay.LivePartitionDetailPage),
                PageIds.MyFollows => typeof(MyFollowsPage),
                _ => typeof(Page)
            };

            return pageType;
        }

        private Type GetPlayerViewType(PageIds pageId)
        {
            var pageType = pageId switch
            {
                PageIds.VideoPlayer => typeof(VideoPlayerPage),
                PageIds.PgcPlayer => typeof(PgcPlayerPage),
                PageIds.LivePlayer => typeof(LivePlayerPage),
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
                CoreViewModel.CheckContinuePlayCommand.Execute().Subscribe();
            }

            CoreViewModel.CheckNewDynamicRegistrationCommand.Execute().Subscribe();
        }
    }

    /// <summary>
    /// <see cref="RootPage"/> 的基类.
    /// </summary>
    public class RootPageBase : AppPage<NavigationViewModel>
    {
    }
}
