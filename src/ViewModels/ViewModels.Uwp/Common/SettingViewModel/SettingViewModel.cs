// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.Enums;
using Windows.ApplicationModel;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public partial class SettingViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel"/> class.
        /// </summary>
        public SettingViewModel()
        {
            ServiceLocator.Instance.LoadService(out _settingsToolkit)
                                   .LoadService(out _resourceToolkit);
            InitializeSettings();
        }

        /// <summary>
        /// 初始化设置.
        /// </summary>
        public void InitializeSettings()
        {
            PropertyChanged -= OnPropertyChangedAsync;
            AppTheme = ReadSetting(SettingNames.AppTheme, AppConstants.ThemeDefault);
            _initializeTheme = AppTheme;
            IsPrelaunch = ReadSetting(SettingNames.IsPrelaunch, true);
            IsAutoPlayWhenLoaded = ReadSetting(SettingNames.IsAutoPlayWhenLoaded, true);
            IsEnableHideRepeaterItemWhenScrolling = ReadSetting(SettingNames.IsEnableHideRepeaterItemWhenScrolling, true);
            IsPrefer4K = ReadSetting(SettingNames.IsPrefer4K, false);
            IsPreferHEVC = ReadSetting(SettingNames.IsPreferHEVC, false);
            SingleFastForwardAndRewindSpan = ReadSetting(SettingNames.SingleFastForwardAndRewindSpan, 30d);
            PlayerModeInit();
            MTCControlModeInit();
            StartupInitAsync();

            PropertyChanged += OnPropertyChangedAsync;
        }

        private async void OnPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AppTheme):
                    WriteSetting(SettingNames.AppTheme, AppTheme);
                    IsShowThemeRestartTip = AppTheme != _initializeTheme;
                    break;
                case nameof(IsPrelaunch):
                    SetPrelaunch();
                    break;
                case nameof(IsStartup):
                    await TrySetStartupAsync();
                    break;
                case nameof(IsAutoPlayWhenLoaded):
                    WriteSetting(SettingNames.IsAutoPlayWhenLoaded, IsAutoPlayWhenLoaded);
                    break;
                case nameof(DefaultPlayerDisplayMode):
                    WriteSetting(SettingNames.DefaultPlayerDisplayMode, DefaultPlayerDisplayMode);
                    break;
                case nameof(IsEnableHideRepeaterItemWhenScrolling):
                    WriteSetting(SettingNames.IsEnableHideRepeaterItemWhenScrolling, IsEnableHideRepeaterItemWhenScrolling);
                    break;
                case nameof(IsPrefer4K):
                    WriteSetting(SettingNames.IsPrefer4K, IsPrefer4K);
                    break;
                case nameof(IsPreferHEVC):
                    WriteSetting(SettingNames.IsPreferHEVC, IsPreferHEVC);
                    break;
                case nameof(SingleFastForwardAndRewindSpan):
                    WriteSetting(SettingNames.SingleFastForwardAndRewindSpan, SingleFastForwardAndRewindSpan);
                    break;
                case nameof(DefaultMTCControlMode):
                    WriteSetting(SettingNames.DefaultMTCControlMode, DefaultMTCControlMode);
                    break;
                default:
                    break;
            }
        }

        private async void StartupInitAsync()
        {
            var task = await StartupTask.GetAsync(AppConstants.StartupTaskId);
            IsStartup = task.State.ToString().Contains("enable", StringComparison.OrdinalIgnoreCase);
            StartupWarningText = string.Empty;
        }

        private void PlayerModeInit()
        {
            if (PlayerDisplayModeCollection == null || PlayerDisplayModeCollection.Count == 0)
            {
                PlayerDisplayModeCollection = new ObservableCollection<PlayerDisplayMode>
                {
                    PlayerDisplayMode.Default,
                    PlayerDisplayMode.FullWindow,
                    PlayerDisplayMode.FullScreen,
                };
            }

            DefaultPlayerDisplayMode = ReadSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
        }

        private void MTCControlModeInit()
        {
            if (MTCControlModeCollection == null || MTCControlModeCollection.Count == 0)
            {
                MTCControlModeCollection = new ObservableCollection<MTCControlMode>
                {
                    MTCControlMode.Automatic,
                    MTCControlMode.Manual,
                };
            }

            DefaultMTCControlMode = ReadSetting(SettingNames.DefaultMTCControlMode, MTCControlMode.Manual);
        }

        private void WriteSetting(SettingNames name, object value) => _settingsToolkit.WriteLocalSetting(name, value);

        private T ReadSetting<T>(SettingNames name, T defaultValue) => _settingsToolkit.ReadLocalSetting(name, defaultValue);
    }
}
