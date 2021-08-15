// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 剧集视图模型.
    /// </summary>
    public partial class SeasonViewModel
    {
        /// <summary>
        /// 从动漫条目中创建.
        /// </summary>
        /// <param name="item">动漫模块条目.</param>
        /// <param name="isVerticalCover">是否为纵向封面.</param>
        /// <returns><see cref="SeasonViewModel"/>.</returns>
        public static SeasonViewModel CreateFromModuleItem(PgcModuleItem item, bool isVerticalCover = true)
        {
            var vm = new SeasonViewModel();
            vm.Title = item.Title;
            vm.Subtitle = item.Description;
            vm.SeasonId = item.OriginId;
            if (item.Aid > 0)
            {
                vm.EpisodeId = item.Aid;
            }

            var resString = isVerticalCover ? "@240w_320h_1c_100q.jpg" : "@400w_250h_1c_100q.jpg";
            vm.Tags = item.SeasonTags;
            vm.CoverUrl = item.Cover + resString;
            vm.Source = item;
            vm.BadgeText = item.Badge;
            vm.SourceCoverUrl = item.Cover;

            if (item.Stat != null && !string.IsNullOrEmpty(item.Stat.FollowDisplayText))
            {
                vm.AdditionalText = item.Stat.FollowDisplayText;
            }
            else if (!string.IsNullOrEmpty(item.DisplayScoreText))
            {
                vm.AdditionalText = item.DisplayScoreText;
            }

            vm.IsShowBadge = !string.IsNullOrEmpty(item.Badge);
            vm.IsShowTags = !string.IsNullOrEmpty(item.SeasonTags);
            vm.IsShowAdditionalText = !string.IsNullOrEmpty(vm.AdditionalText);

            return vm;
        }

        /// <summary>
        /// 从搜索结果创建条目.
        /// </summary>
        /// <param name="item">条目.</param>
        /// <returns>剧集视图模型.</returns>
        public static SeasonViewModel CreateFromSearchItem(PgcSearchItem item)
        {
            var numberToolkit = ServiceLocator.Instance.GetService<INumberToolkit>();
            var vm = new SeasonViewModel();
            vm.Title = item.Title;
            vm.Subtitle = item.Label;
            vm.SeasonId = item.SeasonId;
            vm.Tags = item.SubTitle;
            vm.CoverUrl = item.Cover + "@240w_320h_1c_100q.jpg";
            vm.BadgeText = item.BadgeText;
            vm.SourceCoverUrl = item.Cover;
            vm.Rating = item.Rating;
            vm.AdditionalText = numberToolkit.GetCountText(item.VoteNumber);
            vm.Source = item;

            vm.IsShowBadge = !string.IsNullOrEmpty(item.BadgeText);
            vm.IsShowTags = !string.IsNullOrEmpty(item.Label);
            vm.IsShowAdditionalText = !string.IsNullOrEmpty(vm.AdditionalText);
            vm.IsShowRating = item.Rating > 0;

            return vm;
        }
    }
}
