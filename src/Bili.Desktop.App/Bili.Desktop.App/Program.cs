// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;

namespace Bili.Desktop.App
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public static class Program
    {
        private static bool _isInitialized;
        private static App _app;

        /// <summary>
        /// Intervene in the application startup process, redirecting to the activated instance in case of multi-instance requests.
        /// </summary>
        /// <param name="args">Startup parameters.</param>
        [STAThread]
        internal static void Main(string[] args)
        {
            WinRT.ComWrappersSupport.InitializeComWrappers();
            var isRedirect = DecideRedirection();
            if (!isRedirect && !_isInitialized)
            {
                Microsoft.UI.Xaml.Application.Start((p) =>
                {
                    var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                    SynchronizationContext.SetSynchronizationContext(context);
                    _app = new App();
                });

                _isInitialized = true;
            }
        }

        private static bool DecideRedirection()
        {
            var isRedirect = false;

            var args = AppInstance.GetCurrent().GetActivatedEventArgs();

            try
            {
                var keyInstance = AppInstance.FindOrRegisterForKey("Richasy.Bili.Desktop");

                if (keyInstance.IsCurrent)
                {
                    keyInstance.Activated += OnActivated;
                }
                else
                {
                    isRedirect = true;
                    _ = keyInstance.RedirectActivationToAsync(args);
                }
            }
            catch (Exception)
            {
                isRedirect = true;
            }

            return isRedirect;
        }

        private static void OnActivated(object sender, AppActivationArguments args)
            => _app?.ActivateWindow(args);
    }
}
