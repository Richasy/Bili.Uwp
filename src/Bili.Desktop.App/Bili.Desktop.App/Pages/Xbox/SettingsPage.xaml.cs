// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Base;

namespace Bili.Desktop.App.Pages.Xbox
{
    /// <summary>
    /// 设置页面.
    /// </summary>
    public sealed partial class SettingsPage : SettingsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
