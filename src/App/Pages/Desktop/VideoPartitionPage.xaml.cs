// Copyright (c) Richasy. All rights reserved.

using Bili.App.Pages.Base;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class VideoPartitionPage : VideoPartitionPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartitionPage"/> class.
        /// </summary>
        public VideoPartitionPage()
            => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
