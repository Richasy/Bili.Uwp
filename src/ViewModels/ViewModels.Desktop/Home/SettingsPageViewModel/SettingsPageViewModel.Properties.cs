// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Home
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsPageViewModel
    {
        private readonly IAppToolkit _appToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAppViewModel _appViewModel;
        private string _initializeTheme;

        [ObservableProperty]
        private string _appTheme;

        [ObservableProperty]
        private bool _isShowThemeRestartTip;

        [ObservableProperty]
        private bool _isPrelaunch;

        [ObservableProperty]
        private bool _isStartup;

        [ObservableProperty]
        private string _startupWarningText;

        [ObservableProperty]
        private bool _isAutoPlayWhenLoaded;

        [ObservableProperty]
        private bool _isAutoPlayNextRelatedVideo;

        [ObservableProperty]
        private PlayerDisplayMode _defaultPlayerDisplayMode;

        [ObservableProperty]
        private bool _disableP2PCdn;

        [ObservableProperty]
        private bool _isContinusPlay;

        [ObservableProperty]
        private PreferCodec _preferCodec;

        [ObservableProperty]
        private DecodeType _decodeType;

        [ObservableProperty]
        private PlayerType _playerType;

        [ObservableProperty]
        private PreferQuality _preferQuality;

        [ObservableProperty]
        private double _singleFastForwardAndRewindSpan;

        [ObservableProperty]
        private bool _playbackRateEnhancement;

        [ObservableProperty]
        private bool _globalPlaybackRate;

        [ObservableProperty]
        private string _version;

        [ObservableProperty]
        private bool _isSupportContinuePlay;

        [ObservableProperty]
        private bool _isCopyScreenshot;

        [ObservableProperty]
        private bool _isOpenScreenshotFile;

        [ObservableProperty]
        private bool _isOpenRoaming;

        [ObservableProperty]
        private bool _isGlobeProxy;

        [ObservableProperty]
        private string _roamingVideoAddress;

        [ObservableProperty]
        private string _roamingViewAddress;

        [ObservableProperty]
        private string _roamingSearchAddress;

        [ObservableProperty]
        private bool _isOpenDynamicNotification;

        [ObservableProperty]
        private bool _isEnableBackgroundTask;

        [ObservableProperty]
        private bool _isFullTraditionalChinese;

        [ObservableProperty]
        private bool _isFFmpegPlayer;

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }

        /// <summary>
        /// 播放器显示模式可选集合.
        /// </summary>
        public ObservableCollection<PlayerDisplayMode> PlayerDisplayModeCollection { get; }

        /// <summary>
        /// 偏好的解码模式可选集合.
        /// </summary>
        public ObservableCollection<PreferCodec> PreferCodecCollection { get; }

        /// <summary>
        /// 解码类型可选集合.
        /// </summary>
        public ObservableCollection<DecodeType> DecodeTypeCollection { get; }

        /// <summary>
        /// 播放器类型可选集合.
        /// </summary>
        public ObservableCollection<PlayerType> PlayerTypeCollection { get; }

        /// <summary>
        /// 偏好的画质可选集合.
        /// </summary>
        public ObservableCollection<PreferQuality> PreferQualities { get; }
    }
}
