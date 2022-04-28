// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;

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
        /// <param name="partitionBanner">分区横幅对象.</param>
        public BannerViewModel(PartitionBanner partitionBanner)
        {
            this.Uri = partitionBanner.NavigateUri;
            this.Description = partitionBanner.Title;
            this.Source = partitionBanner;
            this.IsTooltipEnabled = !string.IsNullOrEmpty(this.Description);
            LimitCover(partitionBanner.Image);
            MinHeight = 100d;
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
            MinHeight = 100d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerViewModel"/> class.
        /// </summary>
        /// <param name="pgcItem">PGC条目.</param>
        public BannerViewModel(PgcModuleItem pgcItem)
        {
            this.Uri = pgcItem.WebLink;
            this.Description = pgcItem.Title;
            this.Source = pgcItem;
            this.IsTooltipEnabled = !string.IsNullOrEmpty(this.Description);
            this.Cover = pgcItem.Cover + "@600w_320h_1c_100q.jpg";
            MinHeight = 180d;
        }

        private void LimitCover(string coverUrl)
        {
            this.Cover = coverUrl + "@600w_180h_1c_100q.jpg";
        }
    }
}
