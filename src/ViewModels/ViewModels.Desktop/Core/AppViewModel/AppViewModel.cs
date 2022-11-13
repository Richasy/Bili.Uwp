// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Connectivity;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Windows.Globalization;
using WinRT.Interop;

namespace Bili.ViewModels.Desktop.Core
{
    /// <summary>
    /// 应用ViewModel.
    /// </summary>
    public sealed partial class AppViewModel : ViewModelBase, IAppViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppViewModel"/> class.
        /// </summary>
        public AppViewModel(
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            IAppToolkit appToolkit,
            IUpdateProvider updateProvider,
            ICallerViewModel callerViewModel,
            INavigationViewModel navigationViewModel)
        {
            _callerViewModel = callerViewModel;
            _navigationViewModel = navigationViewModel;
            _resourceToolkit = resourceToolkit;
            _settingsToolkit = settingsToolkit;
            _appToolkit = appToolkit;
            _updateProvider = updateProvider;
            _networkHelper = NetworkHelper.Instance;
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            IsXbox = false;
            IsNavigatePaneOpen = true;
            _isWide = null;
            _networkHelper.NetworkChanged += OnNetworkChanged;
            IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;
            IsShowTitleBar = true;

            CheckUpdateCommand = new AsyncRelayCommand(CheckUpdateAsync);
            CheckNewDynamicRegistrationCommand = new AsyncRelayCommand(CheckNewDynamicRegistrationAsync);

            AttachExceptionHandlerToAsyncCommand(LogException, CheckUpdateCommand);

            var lan = ApplicationLanguages.Languages[0];
            _settingsToolkit.WriteLocalSetting(SettingNames.LastAppLanguage, lan);
            IsTraditionalChinese = lan.Contains("zh-hant", StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public void InjectMainWindow(object mainWindow)
        {
            MainWindow = mainWindow;
            var windowInstance = (Window)mainWindow;
            MainWindowHandle = WindowNative.GetWindowHandle(windowInstance);
            var windowId = Win32Interop.GetWindowIdFromWindow(MainWindowHandle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            appWindow.Title = _resourceToolkit.GetLocaleString(LanguageNames.AppName);
            appWindow.SetIcon(_appToolkit.GetWindowIconPath());
            _appToolkit.InitializeTitleBar(appWindow.TitleBar);
            _appToolkit.ResizeAndCenterWindow(AppConstants.AppLaunchWidth, AppConstants.AppLaunchHeight, MainWindowHandle);
            AppWindow = appWindow;
        }

        /// <inheritdoc/>
        public void InitializePadding()
        {
            var width = Window.Current.Bounds.Width;
            var isWide = _isWide.HasValue && _isWide.Value;
            if (width >= _resourceToolkit.GetResource<double>(AppConstants.MediumWindowThresholdWidthKey))
            {
                if (!isWide)
                {
                    _isWide = true;
                    PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("DefaultPageHorizontalPadding").Left;
                    PageTopPadding = _resourceToolkit.GetResource<Thickness>("DefaultPageTopPadding").Top;
                }
            }
            else
            {
                _isWide = false;
                PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("NarrowPageHorizontalPadding").Left;
                PageTopPadding = _resourceToolkit.GetResource<Thickness>("NarrowPageTopPadding").Top;
            }

            OnPropertyChanged(nameof(PageHorizontalPadding));
        }
    }
}
