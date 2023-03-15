// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Windows.System;

namespace Bili.ViewModels.Workspace
{
    internal static class Utilities
    {
        internal static async Task LaunchWithUrlAsync(string biliUrl, string webUrl)
        {
            var settingsToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            var launchType = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.LaunchType, LaunchType.Web);
            var url = launchType == LaunchType.Web || string.IsNullOrEmpty(biliUrl) ? webUrl : biliUrl;
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        internal static Task PlayVideoWithIdAsync(string videoId)
            => LaunchWithUrlAsync($"richasy-bili://play?video={videoId}", $"https://www.bilibili.com/video/av{videoId}");

        internal static Task PlayEpisodeWithIdAsync(string epId)
            => LaunchWithUrlAsync($"richasy-bili://play?episode={epId}", $"https://www.bilibili.com/bangumi/play/ep{epId}");
    }
}
