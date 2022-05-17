// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendPageViewModel
    {
        private readonly IRecommendProvider _recommendProvider;
        private readonly IResourceToolkit _resourceToolkit;
    }
}
