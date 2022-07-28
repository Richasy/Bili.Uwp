// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Pgc;
using ReactiveUI;
using Splat;
using Windows.Globalization;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 应用ViewModel.
    /// </summary>
    public sealed partial class AppViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppViewModel"/> class.
        /// </summary>
        public AppViewModel()
        {
            _callerViewModel = Locator.Current.GetService<ICallerViewModel>();
            _navigationViewModel = Locator.Current.GetService<NavigationViewModel>();
            _resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
            _settingsToolkit = Locator.Current.GetService<ISettingsToolkit>();
            _fileToolkit = Locator.Current.GetService<IFileToolkit>();
            _appToolkit = Locator.Current.GetService<IAppToolkit>();
            _updateProvider = Locator.Current.GetService<IUpdateProvider>();
            _networkHelper = Microsoft.Toolkit.Uwp.Connectivity.NetworkHelper.Instance;

            IsXbox = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
            IsNavigatePaneOpen = true;
            _isWide = null;
            _networkHelper.NetworkChanged += OnNetworkChanged;
            IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;
            IsShowTitleBar = true;
            IsShowPlayRecordButton = false;
            PlayRecords = new ObservableCollection<PlayRecord>();

            CheckUpdateCommand = ReactiveCommand.CreateFromTask(CheckUpdateAsync);
            CheckContinuePlayCommand = ReactiveCommand.Create(CheckContinuePlay);
            CheckNewDynamicRegistrationCommand = ReactiveCommand.CreateFromTask(CheckNewDynamicRegistrationAsync);
            AddLastPlayItemCommand = ReactiveCommand.CreateFromTask<PlaySnapshot>(AddLastPlayItemAsync);
            DeleteLastPlayItemCommand = ReactiveCommand.CreateFromTask(DeleteLastPlayItemAsync);
            AddPlayRecordCommand = ReactiveCommand.Create<PlayRecord>(AddPlayRecord);
            RemovePlayRecordCommand = ReactiveCommand.Create<PlayRecord>(RemovePlayRecord);
            ClearPlayRecordCommand = ReactiveCommand.Create(ClearPlayRecords);

            CheckUpdateCommand.ThrownExceptions.Subscribe(LogException);
            PlayRecords.CollectionChanged += OnPlayRecordsCollectionChanged;

            RxApp.DefaultExceptionHandler = new UnhandledExceptionHandler();

            var lan = ApplicationLanguages.Languages.First();
            _settingsToolkit.WriteLocalSetting(SettingNames.LastAppLanguage, lan);
            IsTraditionalChinese = lan.Contains("zh-hant", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 初始化主题.
        /// </summary>
        public void InitializeTheme()
        {
            var theme = _settingsToolkit.ReadLocalSetting(SettingNames.AppTheme, AppConstants.ThemeDefault);
            Theme = theme switch
            {
                AppConstants.ThemeLight => ElementTheme.Light,
                AppConstants.ThemeDark => ElementTheme.Dark,
                _ => ElementTheme.Default
            };
        }

        /// <summary>
        /// 初始化边距设置.
        /// </summary>
        public void InitializePadding()
        {
            var width = Window.Current.Bounds.Width;
            if (IsXbox)
            {
                PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("XboxPageHorizontalPadding");
                PageTopPadding = _resourceToolkit.GetResource<Thickness>("XboxPageTopPadding");
            }
            else
            {
                var isWide = _isWide.HasValue && _isWide.Value;
                if (width >= MediumWindowThresholdWidth)
                {
                    if (!isWide)
                    {
                        _isWide = true;
                        PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("DefaultPageHorizontalPadding");
                        PageTopPadding = _resourceToolkit.GetResource<Thickness>("DefaultPageTopPadding");
                    }
                }
                else
                {
                    _isWide = false;
                    PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("NarrowPageHorizontalPadding");
                    PageTopPadding = _resourceToolkit.GetResource<Thickness>("NarrowPageTopPadding");
                }
            }
        }

        /// <summary>
        /// 获取上一次播放的条目.
        /// </summary>
        /// <returns><see cref="PlaySnapshot"/>.</returns>
        public Task<PlaySnapshot> GetLastPlayItemAsync()
            => _fileToolkit.ReadLocalDataAsync<PlaySnapshot>(AppConstants.LastOpenVideoFileName);
    }
}
