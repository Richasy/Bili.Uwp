// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Pgc;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 番剧页面.
    /// </summary>
    public sealed partial class BangumiPage : BangumiPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BangumiPage"/> class.
        /// </summary>
        public BangumiPage() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="BangumiPage"/> 的基类.
    /// </summary>
    public class BangumiPageBase : AppPage<BangumiPageViewModel>
    {
    }
}
