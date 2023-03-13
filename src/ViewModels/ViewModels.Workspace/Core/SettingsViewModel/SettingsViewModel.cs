// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Workspace;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel(
            ISettingsToolkit settingsToolkit,
            IAppToolkit appToolkit)
        {
            _settingsToolkit = settingsToolkit;
            _appToolkit = appToolkit;
            StartupPositions = new ObservableCollection<StartupPosition>
            {
                StartupPosition.TopLeft,
                StartupPosition.TopCenter,
                StartupPosition.TopRight,
                StartupPosition.BottomLeft,
                StartupPosition.BottomCenter,
                StartupPosition.BottomRight,
                StartupPosition.Center,
            };
            PlayerTypes = new ObservableCollection<PlayerType>
            {
                PlayerType.Bili,
                PlayerType.Web,
            };

            InitializeSettings();
        }

        private void InitializeSettings()
        {
            Version = _appToolkit.GetPackageVersion();
            StartupPosition = _settingsToolkit.ReadLocalSetting(SettingNames.StartupPosition, StartupPosition.BottomCenter);
            PlayerType = _settingsToolkit.ReadLocalSetting(SettingNames.PlayerType, PlayerType.Web);
        }

        partial void OnStartupPositionChanged(StartupPosition value)
            => _settingsToolkit.WriteLocalSetting(SettingNames.StartupPosition, value);

        partial void OnPlayerTypeChanged(PlayerType value)
            => _settingsToolkit.WriteLocalSetting(SettingNames.PlayerType, value);
    }
}
