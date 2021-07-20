﻿// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.BiliBili;

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
        public static SeasonViewModel CreateFromAnime(PgcModuleItem item, bool isVerticalCover = true)
        {
            var vm = new SeasonViewModel();
            vm.Title = item.Title;
            vm.Subtitle = item.Description;
            vm.SeasonId = item.OriginId;
            if (item.NewEpisode != null)
            {
                vm.VideoId = item.NewEpisode.Id;
            }
            else if (item.Aid > 0)
            {
                vm.VideoId = item.Aid;
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
    }
}