// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using DI.Workspace;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace Bili.Workspace
{
    /// <summary>
    /// Single instance mode:
    /// https://blogs.windows.com/windowsdeveloper/2022/01/28/making-the-app-single-instanced-part-3/.
    /// </summary>
    public static class Program
    {
        private static App _app;

        // Note that [STAThread] doesn't work with "async Task Main(string[] args)"
        // https://github.com/dotnet/roslyn/issues/22112
        [STAThread]
        private static void Main(string[] args)
        {
            var mainAppInstance = AppInstance.FindOrRegisterForKey(App.Guid);
            if (!mainAppInstance.IsCurrent)
            {
                var current = AppInstance.GetCurrent();
                var actArgs = current.GetActivatedEventArgs();
                RedirectActivationTo(actArgs, mainAppInstance);
                return;
            }
            else
            {
                mainAppInstance.Activated += OnAppActivated;
            }

            WinRT.ComWrappersSupport.InitializeComWrappers();

            Application.Start(p =>
            {
                DIFactory.RegisterAppRequiredServices();
                var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);

                _app = new App();
            });
        }

        private static void OnAppActivated(object sender, AppActivationArguments e)
            => _app.ActivateWindow();

        private static void RedirectActivationTo(
           AppActivationArguments args, AppInstance keyInstance)
        {
            var redirectSemaphore = new Semaphore(0, 1);
            Task.Run(() =>
            {
                keyInstance.RedirectActivationToAsync(args).AsTask().Wait();
                redirectSemaphore.Release();
            });
            redirectSemaphore.WaitOne();
        }
    }
}
