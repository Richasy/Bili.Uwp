// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Pages.Base;

namespace Bili.Desktop.App.Pages.Desktop
{
    /// <summary>
    /// 电影页面.
    /// </summary>
    public sealed partial class MoviePage : MoviePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoviePage"/> class.
        /// </summary>
        public MoviePage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
