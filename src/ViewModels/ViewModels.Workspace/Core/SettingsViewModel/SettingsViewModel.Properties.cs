// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IAppToolkit _appToolkit;

        [ObservableProperty]
        private StartupPosition _startupPosition;

        [ObservableProperty]
        private LaunchType _launchType;

        [ObservableProperty]
        private string _version;

        /// <inheritdoc/>
        public ObservableCollection<StartupPosition> StartupPositions { get; }

        /// <inheritdoc/>
        public ObservableCollection<LaunchType> LaunchTypes { get; }
    }
}
