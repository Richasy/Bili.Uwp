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
        /// 播放器类型集合.
        /// </summary>
        ObservableCollection<PlayerType> PlayerTypes { get; }

        /// <summary>
        /// 应用版本.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 启动位置.
        /// </summary>
        StartupPosition StartupPosition { get; set; }

        /// <summary>
        /// 播放器类型.
        /// </summary>
        PlayerType PlayerType { get; set; }
    }
}
