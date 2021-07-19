// Copyright (c) Richasy. All rights reserved.

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
        /// <returns><see cref="SeasonViewModel"/>.</returns>
        public static SeasonViewModel CreateFromAnime(PgcModuleItem item)
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

            vm.Tags = item.SeasonTags;
            vm.CoverUrl = item.Cover;
            vm.Source = item;

            if (!string.IsNullOrEmpty(item.DisplayScoreText))
            {
                vm.AdditionalText = item.DisplayScoreText;
            }

            return vm;
        }
    }
}
