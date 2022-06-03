// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Controller.Uwp;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using ReactiveUI;
using Splat;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsPageViewModel : ViewModelBase, IInitializeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel"/> class.
        /// </summary>
        internal SettingsPageViewModel(
            AppViewModel appViewModel,
            ISettingsToolkit settingsToolkit,
            IResourceToolkit resourceToolkit)
        {
            _appViewModel = appViewModel;
            _settingsToolkit = settingsToolkit;
            _resourceToolkit = resourceToolkit;

            InitializeCommand = ReactiveCommand.Create(InitializeSettings, outputScheduler: RxApp.MainThreadScheduler);

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
            IsAutoPlayNextRelatedVideo = ReadSetting(SettingNames.IsAutoPlayNextRelatedVideo, false);
            IsPreferHighQuality = ReadSetting(SettingNames.IsPreferHighQuality, false);
            DisableP2PCdn = ReadSetting(SettingNames.DisableP2PCdn, false);
            IsContinusPlay = ReadSetting(SettingNames.IsContinusPlay, true);
            SingleFastForwardAndRewindSpan = ReadSetting(SettingNames.SingleFastForwardAndRewindSpan, 30d);
            IsSupportContinuePlay = ReadSetting(SettingNames.SupportContinuePlay, true);
            IsCopyScreenshot = ReadSetting(SettingNames.CopyScreenshotAfterSave, true);
            IsOpenScreenshotFile = ReadSetting(SettingNames.OpenScreenshotAfterSave, false);
            PlaybackRateEnhancement = ReadSetting(SettingNames.PlaybackRateEnhancement, false);
            GlobalPlaybackRate = ReadSetting(SettingNames.GlobalPlaybackRate, false);
            PreferCodecInit();
            PlayerModeInit();
            StartupInitAsync();
            BackgroundTaskInitAsync();
            RoamingInit();

            Version = BiliController.Instance.GetCurrentAppVersion();
            PropertyChanged += OnPropertyChangedAsync;
        }

        private async void OnPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AppTheme):
                    WriteSetting(SettingNames.AppTheme, AppTheme);
                    IsShowThemeRestartTip = AppTheme != _initializeTheme;
                    Splat.Locator.Current.GetService<AppViewModel>().InitializeTheme();
                    break;
                case nameof(IsPrelaunch):
                    WriteSetting(SettingNames.IsPrelaunch, IsPrelaunch);
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
                case nameof(IsPreferHighQuality):
                    WriteSetting(SettingNames.IsPreferHighQuality, IsPreferHighQuality);
                    break;
                case nameof(DisableP2PCdn):
                    WriteSetting(SettingNames.DisableP2PCdn, DisableP2PCdn);
                    break;
                case nameof(IsContinusPlay):
                    WriteSetting(SettingNames.IsContinusPlay, IsContinusPlay);
                    break;
                case nameof(PreferCodec):
                    WriteSetting(SettingNames.PreferCodec, PreferCodec);
                    break;
                case nameof(SingleFastForwardAndRewindSpan):
                    WriteSetting(SettingNames.SingleFastForwardAndRewindSpan, SingleFastForwardAndRewindSpan);
                    break;
                case nameof(IsSupportContinuePlay):
                    WriteSetting(SettingNames.SupportContinuePlay, IsSupportContinuePlay);
                    break;
                case nameof(IsCopyScreenshot):
                    WriteSetting(SettingNames.CopyScreenshotAfterSave, IsCopyScreenshot);
                    break;
                case nameof(IsOpenScreenshotFile):
                    WriteSetting(SettingNames.OpenScreenshotAfterSave, IsOpenScreenshotFile);
                    break;
                case nameof(PlaybackRateEnhancement):
                    WriteSetting(SettingNames.PlaybackRateEnhancement, PlaybackRateEnhancement);
                    break;
                case nameof(GlobalPlaybackRate):
                    WriteSetting(SettingNames.GlobalPlaybackRate, GlobalPlaybackRate);
                    break;
                case nameof(IsAutoPlayNextRelatedVideo):
                    WriteSetting(SettingNames.IsAutoPlayNextRelatedVideo, IsAutoPlayNextRelatedVideo);
                    break;
                case nameof(IsOpenRoaming):
                    WriteSetting(SettingNames.IsOpenRoaming, IsOpenRoaming);
                    break;
                case nameof(IsGlobeProxy):
                    WriteSetting(SettingNames.IsGlobeProxy, IsGlobeProxy);
                    break;
                case nameof(RoamingVideoAddress):
                    WriteSetting(SettingNames.RoamingVideoAddress, RoamingVideoAddress);
                    break;
                case nameof(RoamingViewAddress):
                    WriteSetting(SettingNames.RoamingViewAddress, RoamingViewAddress);
                    break;
                case nameof(RoamingSearchAddress):
                    WriteSetting(SettingNames.RoamingSearchAddress, RoamingSearchAddress);
                    break;
                case nameof(IsOpenDynamicNotification):
                    WriteSetting(SettingNames.IsOpenNewDynamicNotify, IsOpenDynamicNotification);
                    await _appViewModel.CheckNewDynamicRegistrationAsync();
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

        private async void BackgroundTaskInitAsync()
        {
            IsOpenDynamicNotification = ReadSetting(SettingNames.IsOpenNewDynamicNotify, true);
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            IsEnableBackgroundTask = status.ToString().Contains("Allowed");
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

        private void PreferCodecInit()
        {
            if (PreferCodecCollection == null || PreferCodecCollection.Count == 0)
            {
                PreferCodecCollection = new ObservableCollection<PreferCodec>
                {
                    PreferCodec.H264,
                    PreferCodec.H265,
                    PreferCodec.Av1,
                };
            }

            PreferCodec = ReadSetting(SettingNames.PreferCodec, PreferCodec.H264);
        }

        private void RoamingInit()
        {
            IsOpenRoaming = _settingsToolkit.ReadLocalSetting(SettingNames.IsOpenRoaming, false);
            IsGlobeProxy = _settingsToolkit.ReadLocalSetting(SettingNames.IsGlobeProxy, false);
            RoamingVideoAddress = _settingsToolkit.ReadLocalSetting(SettingNames.RoamingVideoAddress, string.Empty);
            RoamingViewAddress = _settingsToolkit.ReadLocalSetting(SettingNames.RoamingViewAddress, string.Empty);
            RoamingSearchAddress = _settingsToolkit.ReadLocalSetting(SettingNames.RoamingSearchAddress, string.Empty);
        }

        private void WriteSetting(SettingNames name, object value) => _settingsToolkit.WriteLocalSetting(name, value);

        private T ReadSetting<T>(SettingNames name, T defaultValue) => _settingsToolkit.ReadLocalSetting(name, defaultValue);
    }
}
