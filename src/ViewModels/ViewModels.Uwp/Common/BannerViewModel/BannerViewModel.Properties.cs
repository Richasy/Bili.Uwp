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
        [ObservableProperty]
        public string Uri { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string Cover { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string Description { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsTooltipEnabled { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double MinHeight { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public BannerIdentifier Data { get; set; }
    }
}
