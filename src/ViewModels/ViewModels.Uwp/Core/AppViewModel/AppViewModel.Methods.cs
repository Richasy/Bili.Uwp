// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Windows.ApplicationModel.Background;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 应用视图模型.
    /// </summary>
    public sealed partial class AppViewModel
    {
        private void AddPlayRecord(PlayRecord record)
        {
            PlayRecords.Remove(record);
            PlayRecords.Insert(0, record);
        }

        private void RemovePlayRecord(PlayRecord record)
            => PlayRecords.Remove(record);

        private void ClearPlayRecords()
            => TryClear(PlayRecords);

        private async Task AddLastPlayItemAsync(PlaySnapshot data)
        {
            await _fileToolkit.WriteLocalDataAsync(AppConstants.LastOpenVideoFileName, data);
            _settingsToolkit.WriteLocalSetting(SettingNames.CanContinuePlay, true);
        }

        /// <summary>
        /// 清除本地的继续播放视图模型.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task DeleteLastPlayItemAsync()
        {
            await _fileToolkit.DeleteLocalDataAsync(AppConstants.LastOpenVideoFileName);
            _settingsToolkit.WriteLocalSetting(SettingNames.CanContinuePlay, false);
        }

        /// <summary>
        /// 检查是否可以继续播放.
        /// </summary>
        private void CheckContinuePlay()
        {
            var supportCheck = _settingsToolkit.ReadLocalSetting(SettingNames.SupportContinuePlay, true);
            var canPlay = _settingsToolkit.ReadLocalSetting(SettingNames.CanContinuePlay, false);
            if (supportCheck && canPlay)
            {
                RequestContinuePlay?.Invoke(this, EventArgs.Empty);
            }
        }

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
                RequestShowUpdateDialog?.Invoke(this, args);
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

        private void OnUpdateReceived(object sender, UpdateEventArgs e)
            => RequestShowUpdateDialog?.Invoke(this, e);

        private void OnNetworkChanged(object sender, EventArgs e)
            => IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;

        private void OnPlayRecordsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => IsShowPlayRecordButton = PlayRecords.Count > 0;
    }
}
