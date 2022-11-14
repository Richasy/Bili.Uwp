// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Uwp.Base;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 综合动态模块视图模型.
    /// </summary>
    public sealed class DynamicAllModuleViewModel : DynamicModuleViewModelBase, IDynamicAllModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicAllModuleViewModel"/> class.
        /// </summary>
        public DynamicAllModuleViewModel(
            ICommunityProvider communityProvider,
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            IAuthorizeProvider authorizeProvider)
            : base(communityProvider, resourceToolkit, settingsToolkit, authorizeProvider, false)
        {
        }
    }
}
