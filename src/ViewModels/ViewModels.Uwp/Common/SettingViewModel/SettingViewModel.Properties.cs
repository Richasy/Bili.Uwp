// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public partial class SettingViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private string _initializeTheme;

        /// <summary>
        /// 单例.
        /// </summary>
        public static SettingViewModel Instance { get; } = new Lazy<SettingViewModel>(() => new SettingViewModel()).Value;

        /// <summary>
        /// 播放器显示模式可选集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PlayerDisplayMode> PlayerDisplayModeCollection { get; set; }

        /// <summary>
        /// 媒体传输控件控制模式可选集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<MTCControlMode> MTCControlModeCollection { get; set; }

        /// <summary>
        /// 偏好的解码模式可选集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PreferCodec> PreferCodecCollection { get; set; }

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
        /// 默认播放器显示模式.
        /// </summary>
        [Reactive]
        public PlayerDisplayMode DefaultPlayerDisplayMode { get; set; }

        /// <summary>
        /// 默认媒体传输控件控制模式.
        /// </summary>
        [Reactive]
        public MTCControlMode DefaultMTCControlMode { get; set; }

        /// <summary>
        /// 优先4K.
        /// </summary>
        [Reactive]
        public bool IsPrefer4K { get; set; }

        /// <summary>
        /// 偏好的解码模式.
        /// </summary>
        [Reactive]
        public PreferCodec PreferCodec { get; set; }

        /// <summary>
        /// 单次快进/快退时长.
        /// </summary>
        [Reactive]
        public double SingleFastForwardAndRewindSpan { get; set; }

        /// <summary>
        /// 应用版本.
        /// </summary>
        [Reactive]
        public string Version { get; set; }
    }
}
