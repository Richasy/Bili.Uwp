// Copyright (c) Richasy. All rights reserved.

using System.IO;
using System.Text;
using Bili.DI.Container;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using DI.Desktop;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using NLog;

namespace Bili.Desktop.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window _mainWindow;
        private DispatcherQueue _dispatcherQueue;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            DIFactory.RegisterAppRequiredServices();
            Locator.Instance.GetService<IAppToolkit>()
                .InitializeTheme();

            var provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
            UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// 激活窗口.
        /// </summary>
        public void ActivateWindow(AppActivationArguments activationArguments)
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                _mainWindow?.Activate();
            });
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _mainWindow = new MainWindow();

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            Locator.Instance.GetService<IAppViewModel>()
                .InjectMainWindow(_mainWindow);

            _mainWindow.Activate();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Locator.Instance.GetService<ILogger>().Error(e.Exception);
        }
    }
}
