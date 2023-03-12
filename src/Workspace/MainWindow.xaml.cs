// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

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
        }

        private void OnActivated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                Close();
            }
        }
    }
}
