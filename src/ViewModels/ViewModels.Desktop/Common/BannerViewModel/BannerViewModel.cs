// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Common;

namespace Bili.ViewModels.Desktop.Common
{
    /// <summary>
    /// 横幅视图模型.
    /// </summary>
    public partial class BannerViewModel : ViewModelBase, IBannerViewModel
    {
        /// <inheritdoc/>
        public void InjectData(BannerIdentifier data)
        {
            Uri = data.Uri;
            Description = data.Title;
            IsTooltipEnabled = !string.IsNullOrEmpty(Description);
            Cover = data.Image.Uri;
            MinHeight = 100d;
        }
    }
}
