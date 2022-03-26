// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Richasy.Bili.Controller.Uwp;
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
            DoubleClickInit();
            PlayerModeInit();
            MTCControlModeInit();
            StartupInitAsync();

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
                    AppViewModel.Instance.InitializeTheme();
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
                case nameof(DoubleClickBehavior):
                    WriteSetting(SettingNames.DoubleClickBehavior, DoubleClickBehavior);
                    break;
                case nameof(SingleFastForwardAndRewindSpan):
                    WriteSetting(SettingNames.SingleFastForwardAndRewindSpan, SingleFastForwardAndRewindSpan);
                    break;
                case nameof(DefaultMTCControlMode):
                    WriteSetting(SettingNames.DefaultMTCControlMode, DefaultMTCControlMode);
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

        private void DoubleClickInit()
        {
            if (DoubleClickBehaviorCollection == null || DoubleClickBehaviorCollection.Count == 0)
            {
                DoubleClickBehaviorCollection = new ObservableCollection<DoubleClickBehavior>
                {
                    DoubleClickBehavior.PlayPause,
                    DoubleClickBehavior.FullScreen,
                };
            }

            DoubleClickBehavior = ReadSetting(SettingNames.DoubleClickBehavior, DoubleClickBehavior.PlayPause);
        }

        private void WriteSetting(SettingNames name, object value) => _settingsToolkit.WriteLocalSetting(name, value);

        private T ReadSetting<T>(SettingNames name, T defaultValue) => _settingsToolkit.ReadLocalSetting(name, defaultValue);
    }
}
