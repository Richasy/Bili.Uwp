// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 热门视图模型的属性集.
    /// </summary>
    public sealed partial class PopularPageViewModel
    {
        private readonly IHomeProvider _homeProvider;
        private readonly IResourceToolkit _resourceToolkit;
    }
}
