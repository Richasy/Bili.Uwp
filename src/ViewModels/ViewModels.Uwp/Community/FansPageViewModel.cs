// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 粉丝页面视图模型.
    /// </summary>
    public sealed class FansPageViewModel : RelationPageViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansPageViewModel"/> class.
        /// </summary>
        public FansPageViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher)
            : base(accountProvider, resourceToolkit, Models.Enums.Bili.RelationType.Fans, dispatcher)
        {
        }
    }
}
