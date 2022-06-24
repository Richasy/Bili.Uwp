// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 横幅视图模型.
    /// </summary>
    public partial class BannerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerViewModel"/> class.
        /// </summary>
        /// <param name="identifier">横幅标识.</param>
        public BannerViewModel(BannerIdentifier identifier)
        {
            Uri = identifier.Uri;
            Description = identifier.Title;
            IsTooltipEnabled = !string.IsNullOrEmpty(Description);
            Cover = identifier.Image.Uri;
            MinHeight = 100d;
        }
    }
}
