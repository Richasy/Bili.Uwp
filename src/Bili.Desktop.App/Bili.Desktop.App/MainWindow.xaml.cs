// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Desktop;
using Bili.Models.App.Constants;
using Microsoft.UI.Xaml;

namespace Bili.Desktop.App
{
    /// <summary>
    /// 主窗口.
    /// </summary>
    public sealed partial class MainWindow : WindowBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            SetMinWindowSize(AppConstants.AppMinWidth, AppConstants.AppMinHeight);
            InitializeComponent();
            AppFrame.Navigate(typeof(RootPage));
            AppFrame.Loaded += OnFrameLoaded;
        }

        private void OnFrameLoaded(object sender, RoutedEventArgs e)
            => AttachThemeChangedCallback();
    }
}
