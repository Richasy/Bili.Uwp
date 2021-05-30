// Copyright (c) Richasy. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Richasy.Bili.Locator.Uwp;
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
