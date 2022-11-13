// Copyright (c) Richasy. All rights reserved.

using Bili.Uwp.App.Pages.Base;

namespace Bili.Uwp.App.Pages.Desktop
{
    /// <summary>
    /// 国创页面.
    /// </summary>
    public sealed partial class DomesticPage : DomesticPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticPage"/> class.
        /// </summary>
        public DomesticPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
