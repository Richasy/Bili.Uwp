// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using Windows.Globalization;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Core
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
            NavigationViewModel navigationViewModel,
            CoreDispatcher dispatcher)
        {
            _callerViewModel = callerViewModel;
            _navigationViewModel = navigationViewModel;
            _resourceToolkit = resourceToolkit;
            _settingsToolkit = settingsToolkit;
            _appToolkit = appToolkit;
            _updateProvider = updateProvider;
            _networkHelper = Microsoft.Toolkit.Uwp.Connectivity.NetworkHelper.Instance;
            _dispatcher = dispatcher;

            IsXbox = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
            IsNavigatePaneOpen = true;
            _isWide = null;
            _networkHelper.NetworkChanged += OnNetworkChanged;
            IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;
            IsShowTitleBar = true;

            CheckUpdateCommand = ReactiveCommand.CreateFromTask(CheckUpdateAsync);
            CheckNewDynamicRegistrationCommand = ReactiveCommand.CreateFromTask(CheckNewDynamicRegistrationAsync);

            CheckUpdateCommand.ThrownExceptions.Subscribe(LogException);

            RxApp.DefaultExceptionHandler = new UnhandledExceptionHandler();

            var lan = ApplicationLanguages.Languages.First();
            _settingsToolkit.WriteLocalSetting(SettingNames.LastAppLanguage, lan);
            IsTraditionalChinese = lan.Contains("zh-hant", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 初始化边距设置.
        /// </summary>
        public void InitializePadding()
        {
            var width = Window.Current.Bounds.Width;
            if (IsXbox)
            {
                PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("XboxPageHorizontalPadding").Left;
                PageTopPadding = _resourceToolkit.GetResource<Thickness>("XboxPageTopPadding").Top;
            }
            else
            {
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
            }

            this.RaisePropertyChanged(nameof(PageHorizontalPadding));
        }
    }
}
