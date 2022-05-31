// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 关注用户页面视图模型.
    /// </summary>
    public sealed class FollowsPageViewModel : RelationPageViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowsPageViewModel"/> class.
        /// </summary>
        internal FollowsPageViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher)
            : base(accountProvider, resourceToolkit, Models.Enums.Bili.RelationType.Follows, dispatcher)
        {
        }
    }
}
