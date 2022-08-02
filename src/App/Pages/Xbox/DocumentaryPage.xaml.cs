// Copyright (c) Richasy. All rights reserved.

using Bili.App.Pages.Base;

namespace Bili.App.Pages.Xbox
{
    /// <summary>
    /// 纪录片页面.
    /// </summary>
    public sealed partial class DocumentaryPage : DocumentaryPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentaryPage"/> class.
        /// </summary>
        public DocumentaryPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
