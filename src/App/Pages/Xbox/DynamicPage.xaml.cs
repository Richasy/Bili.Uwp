// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Pages.Base;
using Windows.UI.Xaml;

namespace Bili.App.Pages.Xbox
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
            ViewModel.VideoModule.InitializeCommand.Execute().Subscribe();
        }

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}
