// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using H.NotifyIcon;
using Microsoft.Graphics.Display;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.Windows.AppLifecycle;
using WinRT.Interop;

namespace Bili.Workspace
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用标识.
        /// </summary>
        public const string Guid = "443BB14D-025E-48A3-B726-33A7B0F66909";
        private Window _window;
        private DispatcherQueue _dispatcherQueue;
        private StartupPosition _lastWindowPosition;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        private TaskbarIcon TrayIcon { get; set; }

        private bool HandleCloseEvents { get; set; } = true;

        /// <summary>
        /// 激活窗口.
        /// </summary>
        public void ActivateWindow()
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                if (_window == null)
                {
                    LaunchWindow();
                }
                else if (_window.Visible)
                {
                    _window.Hide();
                }
                else
                {
                    _window.Show();
                    CheckStartupPosition();
                    PInvoke.User32.SetForegroundWindow(WindowNative.GetWindowHandle(_window));
                }
            });
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            InitializeTrayIcon();
            LaunchWindow();
        }

        private void InitializeTrayIcon()
        {
            var showHideWindowCommand = (XamlUICommand)Resources["ShowHideWindowCommand"];
            showHideWindowCommand.ExecuteRequested += OnShowHideWindowCommandExecuteRequested;

            var exitApplicationCommand = (XamlUICommand)Resources["QuitCommand"];
            exitApplicationCommand.ExecuteRequested += OnQuitCommandExecuteRequested;

            TrayIcon = (TaskbarIcon)Resources["TrayIcon"];
            TrayIcon.ForceCreate();
        }

        private void LaunchWindow()
        {
            _window = new MainWindow();
            var appWindow = _window.AppWindow;
            appWindow.IsShownInSwitchers = false;
            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            var presenter = appWindow.Presenter as OverlappedPresenter;
            presenter.SetBorderAndTitleBar(false, false);
            presenter.IsResizable = false;
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            MoveAndResize();
            HideWindowFromTaskBar();
            _window.Closed += (sender, args) =>
            {
                if (HandleCloseEvents)
                {
                    args.Handled = true;
                    _window.Hide();
                }
            };

            _window.Activate();
        }

        private void MoveAndResize()
        {
            var hwnd = WindowNative.GetWindowHandle(_window);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var rect = GetRenderRect(displayArea, windowId);
                _window.AppWindow.MoveAndResize(rect);
            }
        }

        private void HideWindowFromTaskBar()
        {
            var hwnd = WindowNative.GetWindowHandle(_window);
            PInvoke.User32.ShowWindow(hwnd, PInvoke.User32.WindowShowStyle.SW_HIDE);
            var flags = (PInvoke.User32.SetWindowLongFlags)PInvoke.User32.GetWindowLong(hwnd, PInvoke.User32.WindowLongIndexFlags.GWL_EXSTYLE);
            PInvoke.User32.SetWindowLong(hwnd, PInvoke.User32.WindowLongIndexFlags.GWL_EXSTYLE, flags | PInvoke.User32.SetWindowLongFlags.WS_EX_TOOLWINDOW);
            PInvoke.User32.ShowWindow(hwnd, PInvoke.User32.WindowShowStyle.SW_SHOW);
        }

        private void CheckStartupPosition()
        {
            var settingsToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            var perferPosition = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.StartupPosition, StartupPosition.BottomCenter);
            if (perferPosition != _lastWindowPosition)
            {
                MoveAndResize();
            }
        }

        private Windows.Graphics.RectInt32 GetRenderRect(DisplayArea displayArea, WindowId windowId)
        {
            var settingsToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            var perferPosition = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.StartupPosition, StartupPosition.BottomCenter);
            var displayInfo = DisplayInformation.CreateForWindowId(windowId);
            var scaleFactor = displayInfo.RawPixelsPerViewPixel;
            var width = Convert.ToInt32(400 * scaleFactor);
            var height = Convert.ToInt32(700 * scaleFactor);
            var workArea = displayArea.WorkArea;
            var left = 0d;
            var top = 0d;
            switch (perferPosition)
            {
                case StartupPosition.TopLeft:
                    left = 12 * scaleFactor;
                    top = 12 * scaleFactor;
                    break;
                case StartupPosition.TopCenter:
                    left = (workArea.Width - width) / 2d;
                    top = 12 * scaleFactor;
                    break;
                case StartupPosition.TopRight:
                    left = workArea.Width - width - (12 * scaleFactor);
                    top = 12 * scaleFactor;
                    break;
                case StartupPosition.Center:
                    left = (workArea.Width - width) / 2d;
                    top = (workArea.Height - height) / 2d;
                    break;
                case StartupPosition.BottomLeft:
                    left = 12 * scaleFactor;
                    top = workArea.Height - height - (12 * scaleFactor);
                    break;
                case StartupPosition.BottomRight:
                    left = workArea.Width - width - (12 * scaleFactor);
                    top = workArea.Height - height - (12 * scaleFactor);
                    break;
                case StartupPosition.BottomCenter:
                    left = (workArea.Width - width) / 2d;
                    top = workArea.Height - height - (12 * scaleFactor);
                    break;
                default:
                    break;
            }

            _lastWindowPosition = perferPosition;
            return new Windows.Graphics.RectInt32(Convert.ToInt32(left), Convert.ToInt32(top), width, height);
        }

        private void OnQuitCommandExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            HandleCloseEvents = false;
            TrayIcon?.Dispose();
            _window?.Close();

            if (_window == null)
            {
                Environment.Exit(0);
            }
        }

        private void OnShowHideWindowCommandExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
            => ActivateWindow();
    }
}
