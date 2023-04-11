// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Enums.Workspace;

namespace Bili.ViewModels.Interfaces.Workspace
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public interface ISettingsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 启动位置集合.
        /// </summary>
        ObservableCollection<StartupPosition> StartupPositions { get; }

        /// <summary>
        /// 启动类型集合.
        /// </summary>
        ObservableCollection<LaunchType> LaunchTypes { get; }

        /// <summary>
        /// 应用版本.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 启动位置.
        /// </summary>
        StartupPosition StartupPosition { get; set; }

        /// <summary>
        /// 启动类型.
        /// </summary>
        LaunchType LaunchType { get; set; }

        /// <summary>
        /// 是否开机自启动.
        /// </summary>
        bool IsStartup { get; set; }
    }
}
