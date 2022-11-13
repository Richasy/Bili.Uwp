// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Base;

namespace Bili.Desktop.App.Pages.Desktop
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

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
