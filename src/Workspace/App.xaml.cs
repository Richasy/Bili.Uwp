// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.Graphics.Display;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
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

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            var appWindow = _window.AppWindow;
            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            appWindow.TitleBar.ButtonForegroundColor = Colors.Transparent;
            var presenter = appWindow.Presenter as OverlappedPresenter;
            presenter.SetBorderAndTitleBar(false, false);
            presenter.IsResizable = false;
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            MoveAndResize();
            HideWindowFromTaskBar();
            _window.Activate();
        }

        private void MoveAndResize()
        {
            var hwnd = WindowNative.GetWindowHandle(_window);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var displayInfo = DisplayInformation.CreateForWindowId(windowId);
                var scaleFactor = displayInfo.RawPixelsPerViewPixel;
                var width = Convert.ToInt32(400 * scaleFactor);
                var height = Convert.ToInt32(700 * scaleFactor);
                var workArea = displayArea.WorkArea;
                var left = (workArea.Width - width) / 2;
                var top = Convert.ToInt32(workArea.Height - height - (12 * scaleFactor));
                _window.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(left, top, width, height));
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
    }
}
