// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 横幅视图模型.
    /// </summary>
    public partial class BannerViewModel
    {
        /// <inheritdoc/>
        [Reactive]
        public string Uri { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string Cover { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string Description { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsTooltipEnabled { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double MinHeight { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public BannerIdentifier Data { get; set; }
    }
}
