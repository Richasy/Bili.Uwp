// Copyright (c) Richasy. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using PInvoke;
using Windows.ApplicationModel;
using Windows.Graphics;
using WinRT.Interop;
using static PInvoke.User32;

namespace Bili.Toolkit.Desktop
{
    /// <summary>
    /// 应用工具组.
    /// </summary>
    public sealed class AppToolkit : IAppToolkit
    {
        private readonly Application _app;
        private readonly ISettingsToolkit _settingsToolkit;
        private object _dispatcherQueueController = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppToolkit"/> class.
        /// </summary>
        /// <param name="toolkit">设置工具.</param>
        public AppToolkit(ISettingsToolkit toolkit)
        {
            _app = Application.Current;
            _settingsToolkit = toolkit;
        }

        /// <inheritdoc/>
        public string GetWindowIconPath()
            => Path.Combine(Package.Current.InstalledPath, "Assets/favicon.ico");

        /// <inheritdoc/>
        public string GetLanguageCode(bool isWindowsName = false)
        {
            var culture = CultureInfo.CurrentUICulture;
            return isWindowsName ? culture.ThreeLetterWindowsLanguageName : culture.Name;
        }

        /// <inheritdoc/>
        public string GetPackageVersion()
        {
            var appVersion = Package.Current.Id.Version;
            return $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}.{appVersion.Revision}";
        }

        /// <inheritdoc/>
        public Tuple<string, string> GetProxyAndArea(string title, bool isVideo)
        {
            var proxy = string.Empty;
            var area = string.Empty;

            var isOpenRoaming = _settingsToolkit.ReadLocalSetting(SettingNames.IsOpenRoaming, false);
            var localProxy = isVideo
                ? _settingsToolkit.ReadLocalSetting(SettingNames.RoamingVideoAddress, string.Empty)
                : _settingsToolkit.ReadLocalSetting(SettingNames.RoamingViewAddress, string.Empty);
            if (isOpenRoaming && !string.IsNullOrEmpty(localProxy))
            {
                if (!string.IsNullOrEmpty(title))
                {
                    if (Regex.IsMatch(title, @"僅.*港.*地區"))
                    {
                        area = "hk";
                    }
                    else if (Regex.IsMatch(title, @"僅.*台.*地區"))
                    {
                        area = "tw";
                    }
                }

                var isForceProxy = _settingsToolkit.ReadLocalSetting(SettingNames.IsGlobeProxy, false);
                if ((isForceProxy && string.IsNullOrEmpty(area))
                    || !string.IsNullOrEmpty(area))
                {
                    proxy = localProxy;
                }
            }

            return new Tuple<string, string>(proxy, area);
        }

        /// <inheritdoc/>
        public IAppToolkit InitializeTheme()
        {
            var localTheme = _settingsToolkit.ReadLocalSetting(SettingNames.AppTheme, AppConstants.ThemeDefault);

            if (localTheme != AppConstants.ThemeDefault)
            {
                _app.RequestedTheme = localTheme == AppConstants.ThemeLight ?
                                        ApplicationTheme.Light :
                                        ApplicationTheme.Dark;
            }

            return this;
        }

        /// <inheritdoc/>
        public IAppToolkit InitializeTitleBar(object titleBar)
        {
            var bar = (AppWindowTitleBar)titleBar;
            bar.ExtendsContentIntoTitleBar = true;
            bar.BackgroundColor = Colors.Transparent;
            bar.InactiveBackgroundColor = Colors.Transparent;
            bar.ButtonBackgroundColor = Colors.Transparent;
            bar.ButtonInactiveBackgroundColor = Colors.Transparent;

            return this;
        }

        /// <inheritdoc/>
        public void InitializeAotWindow(object window, double width, double height, string title = "")
        {
            var hwnd = WindowNative.GetWindowHandle(window);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.SetIcon(GetWindowIconPath());
            InitializeTitleBar(appWindow.TitleBar);

            appWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);

            var actualWidth = GetScalePixel(width, hwnd);
            var actualHeight = GetScalePixel(height, hwnd);

            var rect = GetMonitorRect(hwnd);

            var cx = rect.right - 40 - (actualWidth / 2);
            var cy = rect.top + 40 + (actualHeight / 2);
            var left = cx - (actualWidth / 2);
            var top = cy - (actualHeight / 2);
            appWindow.Resize(new SizeInt32(actualWidth, actualHeight));
            appWindow.Move(new PointInt32(left, top));
            appWindow.Title = title;
        }

        /// <inheritdoc/>
        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                // one already exists, so we'll just use it.
                return;
            }

            if (_dispatcherQueueController == null)
            {
                NativeMethods.DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(NativeMethods.DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                _ = NativeMethods.CreateDispatcherQueueController(options, ref _dispatcherQueueController);
            }
        }

        /// <inheritdoc/>
        public void ResizeAndCenterWindow(double width, double height, IntPtr windowHandle)
        {
            var actualWidth = GetScalePixel(width, windowHandle);
            var actualHeight = GetScalePixel(height, windowHandle);

            var rect = GetMonitorRect(windowHandle);

            var cx = (rect.left + rect.right) / 2;
            var cy = (rect.bottom + rect.top) / 2;
            var left = cx - (actualWidth / 2);
            var top = cy - (actualHeight / 2);
            SetWindowPos(windowHandle, SpecialWindowHandles.HWND_NOTOPMOST, left, top, actualWidth, actualHeight, SetWindowPosFlags.SWP_SHOWWINDOW);
        }

        private static int GetScalePixel(double pixel, IntPtr windowHandle)
        {
            var dpi = GetDpiForWindow(windowHandle);
            return Convert.ToInt32(pixel * (dpi / 96.0));
        }

        private static RECT GetMonitorRect(IntPtr windowHandle)
        {
            var hwndDesktop = MonitorFromWindow(windowHandle, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            var info = new MONITORINFO
            {
                cbSize = 40,
            };
            GetMonitorInfo(hwndDesktop, ref info);

            return info.rcMonitor;
        }

        internal static class NativeMethods
        {
            [DllImport("CoreMessaging.dll")]
            internal static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

            [StructLayout(LayoutKind.Sequential)]
            internal struct DispatcherQueueOptions
            {
                internal int dwSize;
                internal int threadType;
                internal int apartmentType;
            }
        }
    }
}
