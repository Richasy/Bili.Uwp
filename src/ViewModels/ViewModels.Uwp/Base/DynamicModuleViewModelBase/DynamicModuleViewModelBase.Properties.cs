// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 动态模块视图模型基类.
    /// </summary>
    public partial class DynamicModuleViewModelBase
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly bool _isOnlyVideo;
        private bool _isEnd;

        /// <summary>
        /// 动态是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }
    }
}
