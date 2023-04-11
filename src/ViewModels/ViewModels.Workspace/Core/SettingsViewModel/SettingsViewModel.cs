// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Workspace;
using Windows.ApplicationModel;

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
            IResourceToolkit resourceToolkit,
            IAppToolkit appToolkit,
            ICallerViewModel callerViewModel)
        {
            _settingsToolkit = settingsToolkit;
            _resourceToolkit = resourceToolkit;
            _appToolkit = appToolkit;
            _callerViewModel = callerViewModel;
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
            LaunchTypes = new ObservableCollection<LaunchType>
            {
                LaunchType.Bili,
                LaunchType.Web,
            };

            InitializeSettings();
        }

        private void InitializeSettings()
        {
            Version = _appToolkit.GetPackageVersion();
            StartupPosition = _settingsToolkit.ReadLocalSetting(SettingNames.StartupPosition, StartupPosition.BottomCenter);
            LaunchType = _settingsToolkit.ReadLocalSetting(SettingNames.LaunchType, LaunchType.Web);
            StartupInitAsync();
        }

        private async void StartupInitAsync()
        {
            var task = await StartupTask.GetAsync(AppConstants.WorkspaceStartupTaskId);
            IsStartup = task.State.ToString().Contains("enable", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 尝试设置应用自启动.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task TrySetStartupAsync()
        {
            var task = await StartupTask.GetAsync(AppConstants.WorkspaceStartupTaskId);
            if (IsStartup)
            {
                var startupWarningText = string.Empty;
                if (!task.State.ToString().Contains("enable", StringComparison.OrdinalIgnoreCase))
                {
                    var result = await task.RequestEnableAsync();
                    if (result != StartupTaskState.Enabled)
                    {
                        switch (result)
                        {
                            case StartupTaskState.DisabledByUser:
                                startupWarningText = _resourceToolkit.GetLocaleString(LanguageNames.StartupDisabledByUser);
                                break;
                            case StartupTaskState.DisabledByPolicy:
                                startupWarningText = _resourceToolkit.GetLocaleString(LanguageNames.StartupDisabledByPolicy);
                                break;
                            default:
                                break;
                        }

                        IsStartup = false;
                    }
                }

                if (!string.IsNullOrEmpty(startupWarningText))
                {
                    _callerViewModel.ShowTip(startupWarningText);
                }
            }
            else
            {
                task.Disable();
            }
        }

        partial void OnStartupPositionChanged(StartupPosition value)
            => _settingsToolkit.WriteLocalSetting(SettingNames.StartupPosition, value);

        partial void OnLaunchTypeChanged(LaunchType value)
            => _settingsToolkit.WriteLocalSetting(SettingNames.LaunchType, value);

        async partial void OnIsStartupChanged(bool value)
            => await TrySetStartupAsync();
    }
}
