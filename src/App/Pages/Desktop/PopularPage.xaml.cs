// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 热门视频页面.
    /// </summary>
    public sealed partial class PopularPage : PopularPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopularPage"/> class.
        /// </summary>
        public PopularPage()
            : base() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="PopularPage"/> 的基类.
    /// </summary>
    public class PopularPageBase : AppPage<PopularPageViewModel>
    {
    }
}
