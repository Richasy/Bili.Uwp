// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsPageViewModel
    {
        private readonly IAppToolkit _appToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly AppViewModel _appViewModel;
        private string _initializeTheme;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

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

        /// <summary>
        /// 应用主题.
        /// </summary>
        [Reactive]
        public string AppTheme { get; set; }

        /// <summary>
        /// 是否显示主题重启提示文本.
        /// </summary>
        [Reactive]
        public bool IsShowThemeRestartTip { get; set; }

        /// <summary>
        /// 是否预启动.
        /// </summary>
        [Reactive]
        public bool IsPrelaunch { get; set; }

        /// <summary>
        /// 是否开机自启动.
        /// </summary>
        [Reactive]
        public bool IsStartup { get; set; }

        /// <summary>
        /// 启动项警告文本.
        /// </summary>
        [Reactive]
        public string StartupWarningText { get; set; }

        /// <summary>
        /// 加载完成后自动播放.
        /// </summary>
        [Reactive]
        public bool IsAutoPlayWhenLoaded { get; set; }

        /// <summary>
        /// 自动播放下一个关联视频.
        /// </summary>
        [Reactive]
        public bool IsAutoPlayNextRelatedVideo { get; set; }

        /// <summary>
        /// 默认播放器显示模式.
        /// </summary>
        [Reactive]
        public PlayerDisplayMode DefaultPlayerDisplayMode { get; set; }

        /// <summary>
        /// 禁用 P2P CDN.
        /// </summary>
        [Reactive]
        public bool DisableP2PCdn { get; set; }

        /// <summary>
        /// 连续播放.
        /// </summary>
        [Reactive]
        public bool IsContinusPlay { get; set; }

        /// <summary>
        /// 偏好的解码模式.
        /// </summary>
        [Reactive]
        public PreferCodec PreferCodec { get; set; }

        /// <summary>
        /// 解码类型.
        /// </summary>
        [Reactive]
        public DecodeType DecodeType { get; set; }

        /// <summary>
        /// 播放器类型.
        /// </summary>
        [Reactive]
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// 偏好的画质选择.
        /// </summary>
        [Reactive]
        public PreferQuality PreferQuality { get; set; }

        /// <summary>
        /// 单次快进/快退时长.
        /// </summary>
        [Reactive]
        public double SingleFastForwardAndRewindSpan { get; set; }

        /// <summary>
        /// 是否开启播放速率增强.
        /// </summary>
        [Reactive]
        public bool PlaybackRateEnhancement { get; set; }

        /// <summary>
        /// 是否开启全局播放速率.
        /// </summary>
        [Reactive]
        public bool GlobalPlaybackRate { get; set; }

        /// <summary>
        /// 应用版本.
        /// </summary>
        [Reactive]
        public string Version { get; set; }

        /// <summary>
        /// 是否支持初始检查继续播放.
        /// </summary>
        [Reactive]
        public bool IsSupportContinuePlay { get; set; }

        /// <summary>
        /// 是否复制截图.
        /// </summary>
        [Reactive]
        public bool IsCopyScreenshot { get; set; }

        /// <summary>
        /// 是否打开截图文件.
        /// </summary>
        [Reactive]
        public bool IsOpenScreenshotFile { get; set; }

        /// <summary>
        /// 是否打开番剧代理.
        /// </summary>
        [Reactive]
        public bool IsOpenRoaming { get; set; }

        /// <summary>
        /// 是否打开全局代理.
        /// </summary>
        [Reactive]
        public bool IsGlobeProxy { get; set; }

        /// <summary>
        /// 播放代理地址.
        /// </summary>
        [Reactive]
        public string RoamingVideoAddress { get; set; }

        /// <summary>
        /// 详情代理地址.
        /// </summary>
        [Reactive]
        public string RoamingViewAddress { get; set; }

        /// <summary>
        /// 搜索代理地址.
        /// </summary>
        [Reactive]
        public string RoamingSearchAddress { get; set; }

        /// <summary>
        /// 是否开启动态通知.
        /// </summary>
        [Reactive]
        public bool IsOpenDynamicNotification { get; set; }

        /// <summary>
        /// 是否允许后台任务.
        /// </summary>
        [Reactive]
        public bool IsEnableBackgroundTask { get; set; }

        /// <summary>
        /// 是否使用完全的繁体中文模式.
        /// </summary>
        [Reactive]
        public bool IsFullTraditionalChinese { get; set; }

        /// <summary>
        /// 是否为FFmpeg播放器.
        /// </summary>
        [Reactive]
        public bool IsFFmpegPlayer { get; set; }
    }
}
