// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 粉丝页面视图模型.
    /// </summary>
    public sealed class FansPageViewModel : RelationPageViewModelBase, IFansPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansPageViewModel"/> class.
        /// </summary>
        public FansPageViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit)
            : base(accountProvider, resourceToolkit, Models.Enums.Bili.RelationType.Fans)
        {
        }
    }
}
