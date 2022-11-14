// Copyright (c) Richasy. All rights reserved.

using System;
using System.Runtime.InteropServices;
using Bili.DI.Container;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using WinRT;
using WinRT.Interop;

namespace Bili.Desktop.App
{
    /// <summary>
    /// Base class for application windows.
    /// </summary>
#pragma warning disable CA1001
    public class WindowBase : Window
    {
        private readonly IAppToolkit _appToolkit;

        private WinProc _newWndProc = null;
        private IntPtr _oldWndProc = IntPtr.Zero;
        private MicaController _micaController;
        private SystemBackdropConfiguration _configurationSource;

        private double _minWindowWidth = 100d;
        private double _minWindowHeight = 100d;

        private double _maxWindowWidth = 0d;
        private double _maxWindowHeight = 0d;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBase"/> class.
        /// </summary>
        public WindowBase()
        {
            _appToolkit = Locator.Instance.GetService<IAppToolkit>();
            _appToolkit.EnsureWindowsSystemDispatcherQueueController();

            SubClassing();
            TrySetMicaBackdrop();
        }

        private delegate IntPtr WinProc(IntPtr hWnd, PInvoke.User32.WindowMessage msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Set minimum window size.
        /// </summary>
        /// <param name="width">Window min width.</param>
        /// <param name="height">Window min height.</param>
        protected void SetMinWindowSize(double width, double height)
        {
            _minWindowWidth = width;
            _minWindowHeight = height;
        }

        /// <summary>
        /// Set maximum window size.
        /// </summary>
        /// <param name="width">Window max width.</param>
        /// <param name="height">Window max height.</param>
        protected void SetMaxWindowSize(double width, double height)
        {
            _maxWindowWidth = width;
            _maxWindowHeight = height;
        }

        /// <summary>
        /// If Mica is supported, append theme change callback after element is loaded.
        /// </summary>
        protected void AttachThemeChangedCallback()
        {
            SetConfigurationSourceTheme();
            if (MicaController.IsSupported())
            {
                ((FrameworkElement)Content).ActualThemeChanged += OnThemeChanged;
            }
        }

        private void SubClassing()
        {
            var windowHandle = WindowNative.GetWindowHandle(this);
            _newWndProc = new WinProc(NewWindowProc);
            _oldWndProc = NativeMethods.SetWindowLongPtr(windowHandle, PInvoke.User32.WindowLongIndexFlags.GWL_WNDPROC, _newWndProc);
        }

        private IntPtr NewWindowProc(IntPtr hWnd, PInvoke.User32.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case PInvoke.User32.WindowMessage.WM_GETMINMAXINFO:
                    {
                        var windowHandle = WindowNative.GetWindowHandle(this);
                        var minMaxInfo = Marshal.PtrToStructure<PInvoke.User32.MINMAXINFO>(lParam);

                        var monitorSize = _appToolkit.GetMonitorSize(windowHandle);
                        var screenWidth = monitorSize.Width;
                        var screenHeight = monitorSize.Height;

                        var width = screenWidth < _minWindowWidth ? screenWidth - 40 : _minWindowHeight;
                        var height = screenHeight < _minWindowWidth ? screenHeight - 40 : _minWindowHeight;
                        minMaxInfo.ptMinTrackSize.x = _appToolkit.GetScalePixel(width, hWnd);
                        minMaxInfo.ptMinTrackSize.y = _appToolkit.GetScalePixel(height, hWnd);

                        if (_maxWindowWidth > _minWindowWidth && _maxWindowHeight > _minWindowHeight)
                        {
                            minMaxInfo.ptMaxTrackSize.x = _appToolkit.GetScalePixel(_maxWindowWidth, hWnd);
                            minMaxInfo.ptMaxTrackSize.y = _appToolkit.GetScalePixel(_maxWindowHeight, hWnd);
                        }

                        Marshal.StructureToPtr(minMaxInfo, lParam, true);
                        break;
                    }
            }

            return NativeMethods.CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        private bool TrySetMicaBackdrop()
        {
            if (MicaController.IsSupported())
            {
                // Hooking up the policy object
                _configurationSource = new SystemBackdropConfiguration();
                Activated += OnActivated;
                Closed += OnClosed;

                // Initial configuration state.
                _configurationSource.IsInputActive = true;
                _micaController = new MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                _micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                _micaController.SetSystemBackdropConfiguration(_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        private void OnActivated(object sender, WindowActivatedEventArgs args)
            => _configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;

        private void OnClosed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
            // use this closed window.
            if (_micaController != null)
            {
                _micaController.Dispose();
                _micaController = null;
            }

            Activated -= OnActivated;
            _configurationSource = null;
        }

        private void OnThemeChanged(FrameworkElement sender, object args)
        {
            if (_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }

        private void SetConfigurationSourceTheme()
        {
            _configurationSource.Theme = ((FrameworkElement)Content).ActualTheme switch
            {
                ElementTheme.Dark => SystemBackdropTheme.Dark,
                ElementTheme.Light => SystemBackdropTheme.Light,
                _ => SystemBackdropTheme.Default,
            };
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, PInvoke.User32.WindowMessage msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32")]
            internal static extern IntPtr SetWindowLongPtr(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc);
        }
    }
}
