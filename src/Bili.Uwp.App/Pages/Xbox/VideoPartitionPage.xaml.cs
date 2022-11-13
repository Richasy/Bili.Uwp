// Copyright (c) Richasy. All rights reserved.

using Bili.App.Pages.Base;

namespace Bili.App.Pages.Xbox
{
    /// <summary>
    /// 视频分区页面.
    /// </summary>
    public sealed partial class VideoPartitionPage : VideoPartitionPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartitionPage"/> class.
        /// </summary>
        public VideoPartitionPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
