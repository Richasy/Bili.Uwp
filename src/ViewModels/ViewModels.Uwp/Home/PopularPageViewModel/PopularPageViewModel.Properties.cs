// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 热门视图模型的属性集.
    /// </summary>
    public sealed partial class PopularPageViewModel
    {
        private readonly IPopularProvider _popularProvider;
        private readonly IResourceToolkit _resourceToolkit;
    }
}
