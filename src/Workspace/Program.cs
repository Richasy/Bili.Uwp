// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using DI.Workspace;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace Bili.Workspace
{
    /// <summary>
    /// Single instance mode:
    /// https://blogs.windows.com/windowsdeveloper/2022/01/28/making-the-app-single-instanced-part-3/.
    /// </summary>
    public static class Program
    {
        // Note that [STAThread] doesn't work with "async Task Main(string[] args)"
        // https://github.com/dotnet/roslyn/issues/22112
        [STAThread]
        private static void Main(string[] args)
        {
            var mainAppInstance = AppInstance.FindOrRegisterForKey(App.Guid);
            if (!mainAppInstance.IsCurrent)
            {
                var alreadyRunningNotification = new AppNotificationBuilder().AddText("应用已经启动").BuildNotification();
                AppNotificationManager.Default.Show(alreadyRunningNotification);

                Task.Run(async () =>
                {
                    // Clear the toast notification after several seconds
                    await Task.Delay(TimeSpan.FromSeconds(4));
                    await AppNotificationManager.Default.RemoveAllAsync();
                }).GetAwaiter().GetResult();

                return;
            }

            WinRT.ComWrappersSupport.InitializeComWrappers();

            Application.Start(p =>
            {
                DIFactory.RegisterAppRequiredServices();
                var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);

                new App();
            });
        }
    }
}
