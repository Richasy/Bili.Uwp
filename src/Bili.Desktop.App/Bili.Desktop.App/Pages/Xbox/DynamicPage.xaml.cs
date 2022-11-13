// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Base;

namespace Bili.Desktop.App.Pages.Xbox
{
    /// <summary>
    /// 动态页面.
    /// </summary>
    public sealed partial class DynamicPage : DynamicPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPage"/> class.
        /// </summary>
        public DynamicPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
        {
            Bindings.Update();
            ViewModel.VideoModule.InitializeCommand.ExecuteAsync(null);
        }

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
