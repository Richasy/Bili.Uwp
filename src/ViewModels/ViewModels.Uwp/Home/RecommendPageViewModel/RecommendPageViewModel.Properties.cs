// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendPageViewModel
    {
        private readonly IHomeProvider _homeProvider;
        private readonly IResourceToolkit _resourceToolkit;

        /// <summary>
        /// 应用视图模型引用.
        /// </summary>
        [Reactive]
        public IAppViewModel App { get; set; }
    }
}
