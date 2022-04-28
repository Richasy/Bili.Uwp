// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Models.App.Args;
using Windows.ApplicationModel;

namespace Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的应用更新部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 获取当前的应用版本.
        /// </summary>
        /// <returns>版本号字符串.</returns>
        public string GetCurrentAppVersion()
        {
            var appVersion = Package.Current.Id.Version;
            return $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}.{appVersion.Revision}";
        }

        /// <summary>
        /// 检查有没有更新.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task CheckUpdateAsync()
        {
            try
            {
                var data = await _updateProvider.GetGithubLatestReleaseAsync();
                var currentVersion = GetCurrentAppVersion();
                var ignoreVersion = _settingToolkit.ReadLocalSetting(Models.Enums.SettingNames.IgnoreVersion, string.Empty);
                var args = new UpdateEventArgs(data);
                if (args.Version != currentVersion && args.Version != ignoreVersion)
                {
                    UpdateReceived?.Invoke(this, args);
                }
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex, true);
            }
        }
    }
}
