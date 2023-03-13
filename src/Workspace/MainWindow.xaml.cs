// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.Workspace;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = Locator.Instance.GetService<IWorkspaceViewModel>();
            SystemBackdrop = new MicaBackdrop();
            Activated += OnActivated;
            _viewModel.RequestNavigating += OnViewModelRequestNavigating;
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

        private void OnActivated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                Close();
            }
        }

        private void OnViewModelRequestNavigating(object sender, EventArgs e)
            => LoadPage();

        private void OnMainFrameLoaded(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content == null)
            {
                LoadPage();
            }
        }
    }
}
