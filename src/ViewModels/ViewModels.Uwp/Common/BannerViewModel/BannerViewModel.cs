// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 横幅视图模型.
    /// </summary>
    public partial class BannerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerViewModel"/> class.
        /// </summary>
        /// <param name="partitionBanner">分区横幅对象.</param>
        public BannerViewModel(PartitionBanner partitionBanner)
        {
            this.Uri = partitionBanner.NavigateUri;
            this.Description = partitionBanner.Title;
            this.Source = partitionBanner;
            this.IsTooltipEnabled = !string.IsNullOrEmpty(this.Description);
            LimitCover(partitionBanner.Image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerViewModel"/> class.
        /// </summary>
        /// <param name="liveBanner">直播横幅对象.</param>
        public BannerViewModel(LiveFeedBanner liveBanner)
        {
            this.Uri = liveBanner.Link;
            this.Description = liveBanner.Title;
            this.Source = liveBanner;
            this.IsTooltipEnabled = !string.IsNullOrEmpty(this.Description);
            LimitCover(liveBanner.Cover);
        }

        private void LimitCover(string coverUrl)
        {
            this.Cover = coverUrl + "@600w_180h_1c_100q.jpg";
        }
    }
}
