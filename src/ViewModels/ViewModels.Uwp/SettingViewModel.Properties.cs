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

        /// <summary>
        /// 单例.
        /// </summary>
        public static SettingViewModel Instance { get; } = new Lazy<SettingViewModel>(() => new SettingViewModel()).Value;

        /// <summary>
        /// 应用主题.
        /// </summary>
        [Reactive]
        public string AppTheme { get; set; }
    }
}
