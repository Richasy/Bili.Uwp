// Copyright (c) Richasy. All rights reserved.

using Bili.App.Pages.Base;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 电视剧页面.
    /// </summary>
    public sealed partial class TvPage : TvPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvPage"/> class.
        /// </summary>
        public TvPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
