// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 视频动态模块视图模型.
    /// </summary>
    public sealed class DynamicVideoModuleViewModel : DynamicModuleViewModelBase, IDynamicVideoModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVideoModuleViewModel"/> class.
        /// </summary>
        public DynamicVideoModuleViewModel(
            ICommunityProvider communityProvider,
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            IAuthorizeProvider authorizeProvider)
            : base(communityProvider, resourceToolkit, settingsToolkit, authorizeProvider, true)
        {
        }
    }
}
