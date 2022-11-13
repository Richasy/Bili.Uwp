// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Base;

namespace Bili.Desktop.App.Pages.Xbox
{
    /// <summary>
    /// 热门页面.
    /// </summary>
    public sealed partial class PopularPage : PopularPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopularPage"/> class.
        /// </summary>
        public PopularPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
