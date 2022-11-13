// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Windows.ApplicationModel.Background;

namespace Bili.ViewModels.Desktop.Core
{
    /// <summary>
    /// 应用视图模型.
    /// </summary>
    public sealed partial class AppViewModel
    {
        /// <summary>
        /// 检查更新.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task CheckUpdateAsync()
        {
            var data = await _updateProvider.GetGithubLatestReleaseAsync();
            var currentVersion = _appToolkit.GetPackageVersion();
            var ignoreVersion = _settingsToolkit.ReadLocalSetting(SettingNames.IgnoreVersion, string.Empty);
            var args = new UpdateEventArgs(data);
            if (args.Version != currentVersion && args.Version != ignoreVersion)
            {
                _callerViewModel.ShowUpdateDialog(args);
            }
        }

        /// <summary>
        /// 检查新动态通知是否启用.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task CheckNewDynamicRegistrationAsync()
        {
            var openDynamicNotify = _settingsToolkit.ReadLocalSetting(SettingNames.IsOpenNewDynamicNotify, true);
            if (openDynamicNotify)
            {
                await RegisterNewDynamicBackgroundTaskAsync();
            }
            else
            {
                UnregisterNewDynamicBackgroundTask();
            }
        }

        /// <summary>
        /// 注册新动态通知的后台通知任务.
        /// </summary>
        /// <returns>注册结果.</returns>
        private async Task<bool> RegisterNewDynamicBackgroundTaskAsync()
        {
            var taskName = AppConstants.NewDynamicTaskName;
            var hasRegistered = BackgroundTaskRegistration.AllTasks.Any(p => p.Value.Name.Equals(taskName));
            if (!hasRegistered)
            {
                var status = await BackgroundExecutionManager.RequestAccessAsync();
                if (!status.ToString().Contains("Allowed"))
                {
                    return false;
                }

                var builder = new BackgroundTaskBuilder
                {
                    Name = taskName,
                    TaskEntryPoint = taskName,
                };
                builder.SetTrigger(new TimeTrigger(15, false));
                builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                _ = builder.Register();
            }

            return true;
        }

        /// <summary>
        /// 注销新动态通知任务.
        /// </summary>
        private void UnregisterNewDynamicBackgroundTask()
        {
            var taskName = AppConstants.NewDynamicTaskName;
            var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(p => p.Value.Name.Equals(taskName)).Value;
            if (task != null)
            {
                task.Unregister(true);
            }
        }

        private void OnNetworkChanged(object sender, EventArgs e)
            => IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;
    }
}
