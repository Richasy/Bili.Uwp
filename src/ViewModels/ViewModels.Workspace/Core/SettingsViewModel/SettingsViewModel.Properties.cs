// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAppToolkit _appToolkit;
        private readonly ICallerViewModel _callerViewModel;

        [ObservableProperty]
        private StartupPosition _startupPosition;

        [ObservableProperty]
        private LaunchType _launchType;

        [ObservableProperty]
        private string _version;

        [ObservableProperty]
        private bool _isStartup;

        /// <inheritdoc/>
        public ObservableCollection<StartupPosition> StartupPositions { get; }

        /// <inheritdoc/>
        public ObservableCollection<LaunchType> LaunchTypes { get; }
    }
}
