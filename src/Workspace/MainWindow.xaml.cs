// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Models.Enums.Workspace;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Workspace;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = Locator.Instance.GetService<IWorkspaceViewModel>();
            _accountViewModel = Locator.Instance.GetService<IAccountViewModel>();
            SystemBackdrop = new MicaBackdrop();
            Activated += OnActivated;
            _viewModel.RequestNavigating += OnViewModelRequestNavigating;
            _accountViewModel.PropertyChanged += OnAccountViewModelPropertyChanged;
            CheckSignState();
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
                        break;
                    case NavigateTarget.Dynamic:
                        break;
                    case NavigateTarget.Live:
                        break;
                    case NavigateTarget.History:
                        break;
                    case NavigateTarget.Settings:
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

        private void OnMainFrameLoaded(object sender, RoutedEventArgs e)
            => InitializeHomePage();

        private void OnRootContainerLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.XamlRoot = Content.XamlRoot;
        }
    }
}
