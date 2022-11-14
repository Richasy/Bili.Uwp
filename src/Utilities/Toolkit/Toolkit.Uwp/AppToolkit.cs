// Copyright (c) Richasy. All rights reserved.

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Bili.Toolkit.Uwp
{
    /// <summary>
    /// Application Toolkit.
    /// </summary>
    public class AppToolkit : IAppToolkit
    {
        private readonly Application _app;
        private readonly ISettingsToolkit _settingsToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppToolkit"/> class.
        /// </summary>
        public AppToolkit(
            ISettingsToolkit settingsToolkit)
        {
            _app = Application.Current;
            _settingsToolkit = settingsToolkit;
        }

        /// <inheritdoc/>
        public string GetLanguageCode(bool isWindowsName = false)
        {
            var culture = CultureInfo.CurrentUICulture;
            return isWindowsName ? culture.ThreeLetterWindowsLanguageName : culture.Name;
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
            var view = ApplicationView.GetForCurrentView();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            if (_app.RequestedTheme == ApplicationTheme.Dark)
            {
                // active
                view.TitleBar.BackgroundColor = Colors.Transparent;
                view.TitleBar.ForegroundColor = Colors.White;

                // inactive
                view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.InactiveForegroundColor = Colors.Gray;

                // button
                view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonForegroundColor = Colors.White;

                view.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 20, 20, 20);
                view.TitleBar.ButtonHoverForegroundColor = Colors.White;

                view.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 40, 40, 40);
                view.TitleBar.ButtonPressedForegroundColor = Colors.White;

                view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }
            else
            {
                // active
                view.TitleBar.BackgroundColor = Colors.Transparent;
                view.TitleBar.ForegroundColor = Colors.Black;

                // inactive
                view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.InactiveForegroundColor = Colors.Gray;

                // button
                view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonForegroundColor = Colors.DarkGray;

                view.TitleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                view.TitleBar.ButtonHoverForegroundColor = Colors.DarkGray;

                view.TitleBar.ButtonPressedBackgroundColor = Colors.Gray;
                view.TitleBar.ButtonPressedForegroundColor = Colors.DarkGray;

                view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }

            return this;
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
        public string GetPackageVersion()
        {
            var appVersion = Package.Current.Id.Version;
            return $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}.{appVersion.Revision}";
        }

        /// <inheritdoc/>
        public string GetWindowIconPath()
            => throw new NotSupportedException("不支持 UWP 调用此方法");

        /// <inheritdoc/>
        public void InitializeAotWindow(object window, double width, double height, string title = "")
            => throw new NotSupportedException("不支持 UWP 调用此方法");

        /// <inheritdoc/>
        public void EnsureWindowsSystemDispatcherQueueController()
            => throw new NotSupportedException("不支持 UWP 调用此方法");

        /// <inheritdoc/>
        public void ResizeAndCenterWindow(double width, double height, IntPtr windowHandle)
            => throw new NotSupportedException("不支持 UWP 调用此方法");

        /// <inheritdoc/>
        public int GetScalePixel(double pixel, IntPtr windowHandle)
            => throw new NotSupportedException("不支持 UWP 调用此方法");

        /// <inheritdoc/>
        public System.Drawing.Size GetMonitorSize(IntPtr windowHandle)
            => throw new NotSupportedException("不支持 UWP 调用此方法");
    }
}
