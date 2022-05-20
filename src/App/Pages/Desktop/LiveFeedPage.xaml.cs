// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Live;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 直播推荐页面.
    /// </summary>
    public sealed partial class LiveFeedPage : LiveFeedPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveFeedPage"/> class.
        /// </summary>
        public LiveFeedPage() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="LiveFeedPage"/> 的基类.
    /// </summary>
    public class LiveFeedPageBase : AppPage<LiveFeedPageViewModel>
    {
    }
}
