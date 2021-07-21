// Copyright (c) Richasy. All rights reserved.

using System;
using ReactiveUI.Fody.Helpers;
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
    }
}
