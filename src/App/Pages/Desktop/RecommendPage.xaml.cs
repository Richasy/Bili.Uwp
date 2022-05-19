// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 推荐视频页面.
    /// </summary>
    public sealed partial class RecommendPage : RecommendPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendPage"/> class.
        /// </summary>
        public RecommendPage() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="RecommendPage"/> 的基类.
    /// </summary>
    public class RecommendPageBase : AppPage<RecommendPageViewModel>
    {
    }
}
