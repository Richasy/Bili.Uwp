// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.App.Constants;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Workspace;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Workspace.Core;
using Windows.Storage;

namespace DI.Workspace
{
    /// <summary>
    /// 依赖注入工厂.
    /// </summary>
    public static class DIFactory
    {
        /// <summary>
        /// 注册应用所需的依赖服务.
        /// </summary>
        public static void RegisterAppRequiredServices()
        {
            var rootFolder = ApplicationData.Current.LocalFolder;
            var logFolderName = AppConstants.Location.LoggerFolder;
            var fullPath = $"{rootFolder.Path}\\{logFolderName}\\";
            NLog.GlobalDiagnosticsContext.Set("LogPath", fullPath);
            Locator.Instance
                .RegisterSingleton<IResourceToolkit, ResourceToolkit>()
                .RegisterSingleton<INumberToolkit, NumberToolkit>()
                .RegisterSingleton<ISettingsToolkit, SettingsToolkit>()
                .RegisterSingleton<IAppToolkit, AppToolkit>()
                .RegisterSingleton<IMD5Toolkit, MD5Toolkit>()
                .RegisterSingleton<IVideoToolkit, VideoToolkit>()
                .RegisterSingleton<ITextToolkit, TextToolkit>()

                .RegisterSingleton<IWorkspaceViewModel, WorkspaceViewModel>()
                .Build();
        }
    }
}
