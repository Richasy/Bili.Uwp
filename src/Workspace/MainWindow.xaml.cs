// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.Models.Enums.Workspace;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Workspace;
using Bili.Workspace.Controls.App;
using Bili.Workspace.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;

namespace Bili.Workspace
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly IWorkspaceViewModel _viewModel;
        private readonly IAccountViewModel _accountViewModel;
        private readonly ICallerViewModel _callerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            _viewModel = Locator.Instance.GetService<IWorkspaceViewModel>();
            _accountViewModel = Locator.Instance.GetService<IAccountViewModel>();
            _callerViewModel = Locator.Instance.GetService<ICallerViewModel>();
            SystemBackdrop = new MicaBackdrop();
            Activated += OnActivated;
            _viewModel.RequestNavigating += OnViewModelRequestNavigating;
            _accountViewModel.PropertyChanged += OnAccountViewModelPropertyChanged;
            _callerViewModel.RequestShowTip += OnRequestShowTip;
            CheckSignState();
        }

        /// <summary>
        /// 当前窗口实例.
        /// </summary>
        public static MainWindow Instance { get; private set; }

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

        private void LoadPage()
        {
            var isSettings = _viewModel.IsSettingsOpen;
            if (isSettings)
            {
                MainFrame.Navigate(typeof(SettingsPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
            else
            {
                switch (_viewModel.CurrentItem.Target)
                {
                    case NavigateTarget.Home:
                        MainFrame.Navigate(typeof(HomePage), null, new DrillInNavigationTransitionInfo());
                        break;
                    case NavigateTarget.Hot:
                        MainFrame.Navigate(typeof(PopularPage), null, new SlideNavigationTransitionInfo());
                        break;
                    case NavigateTarget.Dynamic:
                        MainFrame.Navigate(typeof(DynamicPage), null, new SlideNavigationTransitionInfo());
                        break;
                    case NavigateTarget.Rank:
                        MainFrame.Navigate(typeof(RankPage), null, new SlideNavigationTransitionInfo());
                        break;
                    case NavigateTarget.History:
                        MainFrame.Navigate(typeof(HistoryPage), null, new SlideNavigationTransitionInfo());
                        break;
                    default:
                        break;
                }
            }
        }

        private void CheckSignState()
        {
            switch (_accountViewModel.State)
            {
                case AuthorizeState.Loading:
                    SignInRing.Visibility = Visibility.Visible;
                    LandingPane.Visibility = Visibility.Collapsed;
                    MainContainer.Visibility = Visibility.Collapsed;
                    break;
                case AuthorizeState.SignedOut:
                    SignInRing.Visibility = Visibility.Collapsed;
                    LandingPane.Visibility = Visibility.Visible;
                    MainContainer.Visibility = Visibility.Collapsed;
                    break;
                case AuthorizeState.SignedIn:
                    SignInRing.Visibility = Visibility.Collapsed;
                    LandingPane.Visibility = Visibility.Collapsed;
                    MainContainer.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void InitializeHomePage()
        {
            if (MainFrame.Content == null && _accountViewModel.State == AuthorizeState.SignedIn)
            {
                LoadPage();
            }
        }

        private void OnActivated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                Close();
            }
            else
            {
                _accountViewModel.TrySignInCommand.Execute(true);
            }
        }

        private void OnViewModelRequestNavigating(object sender, EventArgs e)
            => LoadPage();

        private void OnAccountViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_accountViewModel.State))
            {
                CheckSignState();
                InitializeHomePage();
            }
        }

        private void OnRequestShowTip(object sender, AppTipNotificationEventArgs e)
            => new TipPopup(e.Message).ShowAsync(e.Type);

        private void OnMainFrameLoaded(object sender, RoutedEventArgs e)
            => InitializeHomePage();

        private void OnRootContainerLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.XamlRoot = Content.XamlRoot;
        }
    }
}
