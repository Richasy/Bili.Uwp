// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 设置视图模型.
    /// </summary>
    public sealed partial class SettingsPageViewModel
    {
        /// <summary>
        /// 设置预启动.
        /// </summary>
        public void SetPrelaunch()
        {
            CoreApplication.EnablePrelaunch(IsPrelaunch);
        }

        /// <summary>
        /// 尝试设置应用自启动.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task TrySetStartupAsync()
        {
            var task = await StartupTask.GetAsync(AppConstants.StartupTaskId);
            if (IsStartup)
            {
                StartupWarningText = string.Empty;
                if (!task.State.ToString().Contains("enable", StringComparison.OrdinalIgnoreCase))
                {
                    var result = await task.RequestEnableAsync();
                    if (result != StartupTaskState.Enabled)
                    {
                        switch (result)
                        {
                            case StartupTaskState.DisabledByUser:
                                StartupWarningText = _resourceToolkit.GetLocaleString(LanguageNames.StartupDisabledByUser);
                                break;
                            case StartupTaskState.DisabledByPolicy:
                                StartupWarningText = _resourceToolkit.GetLocaleString(LanguageNames.StartupDisabledByPolicy);
                                break;
                            default:
                                break;
                        }

                        IsStartup = false;
                    }
                }
            }
            else
            {
                task.Disable();
            }
        }
    }
}
