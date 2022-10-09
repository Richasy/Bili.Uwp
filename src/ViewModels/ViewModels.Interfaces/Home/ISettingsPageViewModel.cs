// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Player;

namespace Bili.ViewModels.Interfaces.Home
{
    /// <summary>
    /// 设置页面视图模型的接口定义.
    /// </summary>
    public interface ISettingsPageViewModel : INotifyPropertyChanged, IInitializeViewModel
    {
        /// <summary>
        /// 播放器显示模式可选集合.
        /// </summary>
        ObservableCollection<PlayerDisplayMode> PlayerDisplayModeCollection { get; }

        /// <summary>
        /// 偏好的解码模式可选集合.
        /// </summary>
        ObservableCollection<PreferCodec> PreferCodecCollection { get; }

        /// <summary>
        /// 解码类型可选集合.
        /// </summary>
        ObservableCollection<DecodeType> DecodeTypeCollection { get; }

        /// <summary>
        /// 播放器类型可选集合.
        /// </summary>
        ObservableCollection<PlayerType> PlayerTypeCollection { get; }

        /// <summary>
        /// 偏好的画质可选集合.
        /// </summary>
        ObservableCollection<PreferQuality> PreferQualities { get; }

        /// <summary>
        /// 应用主题.
        /// </summary>
        string AppTheme { get; set; }

        /// <summary>
        /// 是否显示主题重启提示文本.
        /// </summary>
        bool IsShowThemeRestartTip { get; set; }

        /// <summary>
        /// 是否预启动.
        /// </summary>
        bool IsPrelaunch { get; set; }

        /// <summary>
        /// 是否开机自启动.
        /// </summary>
        bool IsStartup { get; set; }

        /// <summary>
        /// 启动项警告文本.
        /// </summary>
        string StartupWarningText { get; set; }

        /// <summary>
        /// 加载完成后自动播放.
        /// </summary>
        bool IsAutoPlayWhenLoaded { get; set; }

        /// <summary>
        /// 自动播放下一个关联视频.
        /// </summary>
        bool IsAutoPlayNextRelatedVideo { get; set; }

        /// <summary>
        /// 默认播放器显示模式.
        /// </summary>
        PlayerDisplayMode DefaultPlayerDisplayMode { get; set; }

        /// <summary>
        /// 禁用 P2P CDN.
        /// </summary>
        bool DisableP2PCdn { get; set; }

        /// <summary>
        /// 连续播放.
        /// </summary>
        bool IsContinusPlay { get; set; }

        /// <summary>
        /// 偏好的解码模式.
        /// </summary>
        PreferCodec PreferCodec { get; set; }

        /// <summary>
        /// 解码类型.
        /// </summary>
        DecodeType DecodeType { get; set; }

        /// <summary>
        /// 播放器类型.
        /// </summary>
        PlayerType PlayerType { get; set; }

        /// <summary>
        /// 偏好的画质选择.
        /// </summary>
        PreferQuality PreferQuality { get; set; }

        /// <summary>
        /// 单次快进/快退时长.
        /// </summary>
        double SingleFastForwardAndRewindSpan { get; set; }

        /// <summary>
        /// 是否开启播放速率增强.
        /// </summary>
        bool PlaybackRateEnhancement { get; set; }

        /// <summary>
        /// 是否开启全局播放速率.
        /// </summary>
        bool GlobalPlaybackRate { get; set; }

        /// <summary>
        /// 应用版本.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 是否支持初始检查继续播放.
        /// </summary>
        bool IsSupportContinuePlay { get; set; }

        /// <summary>
        /// 是否复制截图.
        /// </summary>
        bool IsCopyScreenshot { get; set; }

        /// <summary>
        /// 是否打开截图文件.
        /// </summary>
        bool IsOpenScreenshotFile { get; set; }

        /// <summary>
        /// 是否打开番剧代理.
        /// </summary>
        bool IsOpenRoaming { get; set; }

        /// <summary>
        /// 是否打开全局代理.
        /// </summary>
        bool IsGlobeProxy { get; set; }

        /// <summary>
        /// 播放代理地址.
        /// </summary>
        string RoamingVideoAddress { get; set; }

        /// <summary>
        /// 详情代理地址.
        /// </summary>
        string RoamingViewAddress { get; set; }

        /// <summary>
        /// 搜索代理地址.
        /// </summary>
        string RoamingSearchAddress { get; set; }

        /// <summary>
        /// 是否开启动态通知.
        /// </summary>
        bool IsOpenDynamicNotification { get; set; }

        /// <summary>
        /// 是否允许后台任务.
        /// </summary>
        bool IsEnableBackgroundTask { get; set; }

        /// <summary>
        /// 是否使用完全的繁体中文模式.
        /// </summary>
        bool IsFullTraditionalChinese { get; set; }

        /// <summary>
        /// 是否为FFmpeg播放器.
        /// </summary>
        bool IsFFmpegPlayer { get; set; }
    }
}
