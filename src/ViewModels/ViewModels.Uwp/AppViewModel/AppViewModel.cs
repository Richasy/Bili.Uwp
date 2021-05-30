// Copyright (c) Richasy. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.Toolkit.Uwp;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 应用ViewModel.
    /// </summary>
    public partial class AppViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppViewModel"/> class.
        /// </summary>
        internal AppViewModel()
        {
            RegisterToolkitServices();
            IsNavigatePaneOpen = true;
            CurrentMainContentId = PageIds.Home;
        }

        /// <summary>
        /// 修改当前主内容标识.
        /// </summary>
        /// <param name="pageId">主内容标识.</param>
        public void SetMainContentId(PageIds pageId)
        {
            CurrentMainContentId = pageId;
            IsShowOverlay = false;
        }

        /// <summary>
        /// 修改当前覆盖内容标识.
        /// </summary>
        /// <param name="pageId">覆盖内容标识.</param>
        public void SetOverlayContentId(PageIds pageId)
        {
            CurrentOverlayContentId = pageId;
            IsShowOverlay = true;
        }

        private void RegisterToolkitServices()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IAppToolkit, AppToolkit>()
                .AddSingleton<IFileToolkit, FileToolkit>()
                .AddSingleton<IResourceToolkit, ResourceToolkit>()
                .AddSingleton<ISettingsToolkit, SettingsToolkit>();
            _ = new ServiceLocator(serviceCollection);
        }
    }
}
