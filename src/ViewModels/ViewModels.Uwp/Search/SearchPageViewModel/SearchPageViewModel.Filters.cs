// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.Data.Appearance;
using Bili.Models.Enums;

using static Bili.Models.App.Constants.ServiceConstants.Search;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索页面视图模型.
    /// </summary>
    public sealed partial class SearchPageViewModel
    {
        private async Task InitializeFiltersAsync(SearchModuleType type)
        {
            _filters.Remove(type);
            if (type == SearchModuleType.Video)
            {
                await InitializeVideoFiltersAsync();
            }
            else if (type == SearchModuleType.Article)
            {
                await InitializeArticleFiltersAsync();
            }
            else if (type == SearchModuleType.User)
            {
                InitializeUserFilters();
            }
            else
            {
                _filters.Add(type, new List<SearchFilterViewModel>());
            }
        }

        private async Task InitializeVideoFiltersAsync()
        {
            var orderFilter = new Filter("排序", OrderType, new List<Condition>
            {
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByDefault), "default"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByPlay), "view"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByNewest), "pubdate"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByDanmaku), "danmaku"),
            });

            var durationFilter = new Filter("时长", Duration, new List<Condition>
            {
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.FilterByTotalDuration), "0"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.FilterByLessThan10Min), "1"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.FilterByLessThan30Min), "2"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.FilterByLessThan60Min), "3"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.FilterByGreaterThan60Min), "4"),
            });

            var totalPartitions = await _homeProvider.GetVideoPartitionIndexAsync();
            var partitionConditions = totalPartitions
                .Select(p => new Condition(p.Name, p.Id))
                .ToList();
            partitionConditions.Insert(0, new Condition(_resourceToolkit.GetLocaleString(LanguageNames.Total), "0"));
            var partitionFilter = new Filter("分区", PartitionId, partitionConditions);

            var orderVM = new SearchFilterViewModel(orderFilter);
            var durationVM = new SearchFilterViewModel(durationFilter);
            var partitionVM = new SearchFilterViewModel(partitionFilter);
            _filters.Add(SearchModuleType.Video, new List<SearchFilterViewModel> { orderVM, durationVM, partitionVM });
        }

        private async Task InitializeArticleFiltersAsync()
        {
            var orderFilter = new Filter("排序", OrderType, new List<Condition>
            {
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByDefault), string.Empty),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByNewest), "pubdate"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByRead), "click"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByReply), "scores"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByLike), "attention"),
            });

            var totalPartitions = await _articleProvider.GetPartitionsAsync();
            var partitionConditions = totalPartitions
                .Select(p => new Condition(p.Name, p.Id))
                .ToList();
            var partitionFilter = new Filter("分区", PartitionId, partitionConditions);

            var orderVM = new SearchFilterViewModel(orderFilter);
            var partitionVM = new SearchFilterViewModel(partitionFilter);
            _filters.Add(SearchModuleType.Article, new List<SearchFilterViewModel> { orderVM, partitionVM });
        }

        private void InitializeUserFilters()
        {
            var orderFilter = new Filter("排序", OrderType, new List<Condition>
            {
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByDefault), "totalrank_0"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByFansHTL), "fan_0"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByFansLTH), "fan_1"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByLevelHTL), "level_0"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.SortByLevelLTH), "level_1"),
            });

            var typeFilter = new Filter("用户类型", UserType, new List<Condition>
            {
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.TotalUser), "0"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.UpMaster), "1"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.NormalUser), "2"),
                new Condition(_resourceToolkit.GetLocaleString(LanguageNames.OfficialUser), "3"),
            });

            var orderVM = new SearchFilterViewModel(orderFilter);
            var typeVM = new SearchFilterViewModel(typeFilter);
            _filters.Add(SearchModuleType.User, new List<SearchFilterViewModel> { orderVM, typeVM });
        }
    }
}
