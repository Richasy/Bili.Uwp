﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using FFmpegInterop;
using Richasy.Bili.Controller.Uwp.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App
{
    /// <summary>
    /// Provide application-specific behaviors to supplement the default application classes.
    /// </summary>
    public sealed partial class App : Application, ILogProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.UnhandledException += this.OnUnhandledException;
            _ = AppViewModel.Instance;
            ServiceLocator.Instance.GetService<IAppToolkit>()
                                   .InitializeTheme();

            FFmpegInteropLogging.SetLogLevel(LogLevel.Error);
            FFmpegInteropLogging.SetLogProvider(this);
        }

        /// <inheritdoc/>
        public void Log(LogLevel level, string message)
        {
            Debug.WriteLine($"{level} | {message}");
        }

        /// <summary>
        /// Called when the application is normally launched by the end user.
        /// Will be used in situations such as launching an application to open a specific file.
        /// </summary>
        /// <param name="e">Detailed information about the start request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            this.OnLaunchedOrActivated(e);
        }

        /// <summary>
        /// Called when the application is activated by the end user.
        /// </summary>
        /// <param name="args">Detailed information about the active request and process.</param>
        protected override void OnActivated(IActivatedEventArgs args)
        {
            this.OnLaunchedOrActivated(args);
        }

        private void OnLaunchedOrActivated(IActivatedEventArgs e)
        {
            var appView = ApplicationView.GetForCurrentView();
            appView.SetPreferredMinSize(new Size(AppConstants.AppMinWidth, AppConstants.AppMinHeight));
            _ = SYEngine.Core.Initialize();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += this.OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e is LaunchActivatedEventArgs && (e as LaunchActivatedEventArgs).PrelaunchActivated == false)
            {
                SettingViewModel.Instance.SetPrelaunch();
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Pages.RootPage), (e as LaunchActivatedEventArgs).Arguments);
                }
            }

            // App launched or activated by link
            else if (e is ProtocolActivatedEventArgs protocalArgs)
            {
                var arg = protocalArgs.Uri.Query.Replace("?", string.Empty);
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Pages.RootPage), arg);
                }
            }

            // App is launched or activated by a satrtup task
            else if (e.Kind == ActivationKind.StartupTask)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Pages.RootPage));
                }
            }

            // App is launched or activated by notification
            else if (e is ToastNotificationActivatedEventArgs toastActivationArgs)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(Pages.RootPage));
                }

                // TODO: Parse toastActivationArgs.Argument
            }

            Window.Current.Activate();
            ServiceLocator.Instance.GetService<IAppToolkit>().InitializeTitleBar();
        }

        /// <summary>
        /// Called when navigation to a specific page fails.
        /// </summary>
        /// <param name="sender">Navigation failure frame.</param>
        /// <param name="e">Details about navigation failure.</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Called when the execution of the application is about to be suspended.
        /// </summary>
        /// <param name="sender">The source of the pending request.</param>
        /// <param name="e">Details about the pending request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Called when an uncaught error occurs.
        /// </summary>
        /// <param name="sender">The source of the error throw.</param>
        /// <param name="e">Details about unhandled exception.</param>
        private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            var logger = ServiceLocator.Instance.GetService<ILoggerModule>();
            logger.LogError(e.Exception);
        }
    }
}
