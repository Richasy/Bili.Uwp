// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Home;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsPageViewModel : ViewModelBase, ISettingsPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel"/> class.
        /// </summary>
        public SettingsPageViewModel(
            ICallerViewModel callerViewModel,
            IAppViewModel appViewModel,
            ISettingsToolkit settingsToolkit,
            IResourceToolkit resourceToolkit,
            IAppToolkit appToolkit)
        {
            _appViewModel = appViewModel;
            _settingsToolkit = settingsToolkit;
            _resourceToolkit = resourceToolkit;
            _appToolkit = appToolkit;

            PlayerDisplayModeCollection = new ObservableCollection<PlayerDisplayMode>();
            PreferCodecCollection = new ObservableCollection<PreferCodec>();
            DecodeTypeCollection = new ObservableCollection<DecodeType>();
            PlayerTypeCollection = new ObservableCollection<PlayerType>();
            PreferQualities = new ObservableCollection<PreferQuality>();

            InitializeCommand = new AsyncRelayCommand(() =>
            {
                InitializeSettings();
                return Task.CompletedTask;
            });

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
            DisableP2PCdn = ReadSetting(SettingNames.DisableP2PCdn, false);
            IsContinusPlay = ReadSetting(SettingNames.IsContinusPlay, true);
            SingleFastForwardAndRewindSpan = ReadSetting(SettingNames.SingleFastForwardAndRewindSpan, 30d);
            IsSupportContinuePlay = ReadSetting(SettingNames.SupportContinuePlay, true);
            IsCopyScreenshot = ReadSetting(SettingNames.CopyScreenshotAfterSave, true);
            IsOpenScreenshotFile = ReadSetting(SettingNames.OpenScreenshotAfterSave, false);
            PlaybackRateEnhancement = ReadSetting(SettingNames.PlaybackRateEnhancement, false);
            GlobalPlaybackRate = ReadSetting(SettingNames.GlobalPlaybackRate, false);
            IsFullTraditionalChinese = ReadSetting(SettingNames.IsFullTraditionalChinese, false);
            PreferCodecInit();
            DecodeInit();
            PlayerModeInit();
            StartupInitAsync();
            BackgroundTaskInitAsync();
            RoamingInit();
            PlayerTypeInit();
            PreferQualityInit();

            Version = _appToolkit.GetPackageVersion();
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
                    _ = _appViewModel.CheckNewDynamicRegistrationCommand.ExecuteAsync(null);
                    break;
                case nameof(IsFullTraditionalChinese):
                    WriteSetting(SettingNames.IsFullTraditionalChinese, IsFullTraditionalChinese);
                    break;
                case nameof(DecodeType):
                    WriteSetting(SettingNames.DecodeType, DecodeType);
                    break;
                case nameof(PlayerType):
                    WriteSetting(SettingNames.PlayerType, PlayerType);
                    IsFFmpegPlayer = PlayerType == PlayerType.FFmpeg;
                    break;
                case nameof(PreferQuality):
                    WriteSetting(SettingNames.PreferQuality, PreferQuality);
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
            if (PlayerDisplayModeCollection.Count == 0)
            {
                PlayerDisplayModeCollection.Add(PlayerDisplayMode.Default);
                PlayerDisplayModeCollection.Add(PlayerDisplayMode.FullScreen);

                if (!_appViewModel.IsXbox)
                {
                    PlayerDisplayModeCollection.Insert(1, PlayerDisplayMode.FullWindow);
                }
            }

            DefaultPlayerDisplayMode = ReadSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
        }

        private void PreferCodecInit()
        {
            if (PreferCodecCollection.Count == 0)
            {
                PreferCodecCollection.Add(PreferCodec.H264);
                PreferCodecCollection.Add(PreferCodec.H265);
                PreferCodecCollection.Add(PreferCodec.Av1);
            }

            PreferCodec = ReadSetting(SettingNames.PreferCodec, PreferCodec.H264);
        }

        private void DecodeInit()
        {
            if (DecodeTypeCollection.Count == 0)
            {
                DecodeTypeCollection.Add(DecodeType.Automatic);
                DecodeTypeCollection.Add(DecodeType.HardwareDecode);
                DecodeTypeCollection.Add(DecodeType.SoftwareDecode);
            }

            DecodeType = ReadSetting(SettingNames.DecodeType, DecodeType.Automatic);
        }

        private void PlayerTypeInit()
        {
            if (PlayerTypeCollection.Count == 0)
            {
                PlayerTypeCollection.Add(PlayerType.Native);
                PlayerTypeCollection.Add(PlayerType.FFmpeg);
            }

            PlayerType = ReadSetting(SettingNames.PlayerType, PlayerType.Native);
            IsFFmpegPlayer = PlayerType == PlayerType.FFmpeg;
        }

        private void PreferQualityInit()
        {
            if(PreferQualities.Count == 0)
            {
                PreferQualities.Add(PreferQuality.Auto);
                PreferQualities.Add(PreferQuality.HDFirst);
                PreferQualities.Add(PreferQuality.HighQuality);
            }

            var defaultQuality = _appViewModel.IsXbox ? PreferQuality.HDFirst : PreferQuality.Auto;
            PreferQuality = ReadSetting(SettingNames.PreferQuality, defaultQuality);
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
