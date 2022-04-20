// Copyright (c) Richasy. All rights reserved.

using System;
using System.Text.RegularExpressions;
using Bilibili.App.Dynamic.V2;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 剧集视图模型.
    /// </summary>
    public partial class SeasonViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonViewModel"/> class.
        /// </summary>
        /// <param name="item">动漫模块条目.</param>
        /// <param name="isVerticalCover">是否为纵向封面.</param>
        public SeasonViewModel(PgcModuleItem item, bool isVerticalCover = true)
        {
            Title = item.Title;
            Subtitle = item.Description;
            SeasonId = item.OriginId;
            if (item.Aid > 0)
            {
                EpisodeId = item.Aid;
            }

            var resString = isVerticalCover ? "@240w_320h_1c_100q.jpg" : "@400w_250h_1c_100q.jpg";
            Tags = item.SeasonTags;
            CoverUrl = item.Cover + resString;
            Source = item;
            BadgeText = item.Badge;
            SourceCoverUrl = item.Cover;

            if (item.Stat != null && !string.IsNullOrEmpty(item.Stat.FollowDisplayText))
            {
                AdditionalText = item.Stat.FollowDisplayText;
            }
            else if (!string.IsNullOrEmpty(item.DisplayScoreText))
            {
                AdditionalText = item.DisplayScoreText;
            }

            IsShowBadge = !string.IsNullOrEmpty(item.Badge);
            IsShowTags = !string.IsNullOrEmpty(item.SeasonTags);
            IsShowAdditionalText = !string.IsNullOrEmpty(AdditionalText);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonViewModel"/> class.
        /// </summary>
        /// <param name="item">条目.</param>
        public SeasonViewModel(PgcSearchItem item)
        {
            Title = Regex.Replace(item.Title, "<[^>]+>", string.Empty);
            Subtitle = item.Label;
            SeasonId = item.SeasonId;
            Tags = item.SubTitle;
            CoverUrl = item.Cover + "@240w_320h_1c_100q.jpg";
            BadgeText = item.BadgeText;
            SourceCoverUrl = item.Cover;
            Rating = item.Rating;
            AdditionalText = item.Area;
            Source = item;

            IsShowBadge = !string.IsNullOrEmpty(item.BadgeText);
            IsShowTags = !string.IsNullOrEmpty(item.Label);
            IsShowAdditionalText = !string.IsNullOrEmpty(AdditionalText);
            IsShowRating = item.Rating > 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonViewModel"/> class.
        /// </summary>
        /// <param name="item">PGC索引条目.</param>
        public SeasonViewModel(PgcIndexItem item)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            Title = item.Title;
            Subtitle = item.IsFinish == 1 ?
                resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.PublishFinished) :
                resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.PublishInInstalments);
            Tags = item.OrderText;
            SeasonId = item.SeasonId;
            CoverUrl = item.Cover + "@240w_320h_1c_100q.jpg";
            BadgeText = item.BadgeText;
            SourceCoverUrl = item.Cover;
            AdditionalText = item.AdditionalText;
            Source = item;

            IsShowBadge = !string.IsNullOrEmpty(item.BadgeText);
            IsShowTags = !string.IsNullOrEmpty(item.OrderText);
            IsShowAdditionalText = !string.IsNullOrEmpty(AdditionalText);
            IsShowRating = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonViewModel"/> class.
        /// </summary>
        /// <param name="item">PGC时间线条目.</param>
        public SeasonViewModel(TimeLineEpisode item)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            Title = item.Title;
            Subtitle = item.PublishTime;
            Tags = item.PublishIndex;
            SeasonId = item.SeasonId;
            EpisodeId = item.EpisodeId;
            CoverUrl = item.Cover + "@240w_320h_1c_100q.jpg";
            SourceCoverUrl = item.Cover;
            AdditionalText = item.IsPublished == 1 ?
                resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Updated) :
                resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NotUpdated);
            Source = item;

            IsShowBadge = false;
            IsShowTags = !string.IsNullOrEmpty(item.PublishIndex);
            IsShowAdditionalText = !string.IsNullOrEmpty(AdditionalText);
            IsShowRating = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonViewModel"/> class.
        /// </summary>
        /// <param name="item">播放列表条目.</param>
        public SeasonViewModel(PgcPlayListSeason item)
        {
            Title = item.Title;
            Subtitle = item.Description;
            SeasonId = item.SeasonId;
            Tags = item.Styles;
            CoverUrl = item.Cover + "@240w_320h_1c_100q.jpg";
            Source = item;
            BadgeText = item.BadgeText;
            SourceCoverUrl = item.Cover;
            AdditionalText = item.Subtitle;

            IsShowBadge = !string.IsNullOrEmpty(item.BadgeText);
            IsShowTags = !string.IsNullOrEmpty(item.Styles);
            IsShowAdditionalText = !string.IsNullOrEmpty(AdditionalText);

            if (item.Rating != null)
            {
                Rating = item.Rating.Score;
            }

            IsShowRating = Rating > 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonViewModel"/> class.
        /// </summary>
        /// <param name="item">收藏夹条目.</param>
        public SeasonViewModel(FavoritePgcItem item)
        {
            Title = item.Title;
            Subtitle = item.NewEpisode?.DisplayText ?? "--";
            SeasonId = item.SeasonId;
            Tags = item.SeasonTypeName;
            CoverUrl = item.Cover + "@240w_320h_1c_100q.jpg";
            Source = item;
            BadgeText = item.BadgeText;
            SourceCoverUrl = item.Cover;

            IsShowBadge = !string.IsNullOrEmpty(item.BadgeText);
            IsShowTags = !string.IsNullOrEmpty(item.SeasonTypeName);
            IsShowAdditionalText = !string.IsNullOrEmpty(AdditionalText);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonViewModel"/> class.
        /// </summary>
        /// <param name="item">PGC动态条目.</param>
        public SeasonViewModel(MdlDynPGC item)
        {
            var numberToolkit = ServiceLocator.Instance.GetService<INumberToolkit>();
            Title = item.Title;
            SeasonId = Convert.ToInt32(item.SeasonId);
            EpisodeId = Convert.ToInt32(item.Epid);
            CoverUrl = item.Cover + "@400w_250h_1c_100q.jpg";
            AdditionalText = numberToolkit.GetDurationText(TimeSpan.FromSeconds(item.Duration));
            SourceCoverUrl = item.Cover;
            Source = item;

            IsShowBadge = false;
            IsShowAdditionalText = !string.IsNullOrEmpty(AdditionalText);
            IsShowRating = false;
        }
    }
}
