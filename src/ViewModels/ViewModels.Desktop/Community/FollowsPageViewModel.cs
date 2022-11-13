// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 关注用户页面视图模型.
    /// </summary>
    public sealed class FollowsPageViewModel : RelationPageViewModelBase, IFollowsPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowsPageViewModel"/> class.
        /// </summary>
        public FollowsPageViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit)
            : base(accountProvider, resourceToolkit, Models.Enums.Bili.RelationType.Follows)
        {
        }
    }
}
