﻿// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Live;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using Splat;
using static Bili.Models.App.Constants.ServiceConstants.Search;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索页面视图模型.
    /// </summary>
    public sealed partial class SearchPageViewModel
    {
        private async Task RequestDataAsync()
        {
            switch (CurrentModule.Type)
            {
                case SearchModuleType.Video:
                    await RequestVideoDataAsync();
                    break;
                case SearchModuleType.Anime:
                    await RequestAnimeDataAsync();
                    break;
                case SearchModuleType.Live:
                    await RequestLiveDataAsync();
                    break;
                case SearchModuleType.User:
                    await RequestUserDataAsync();
                    break;
                case SearchModuleType.Movie:
                    await RequestMovieDataAsync();
                    break;
                case SearchModuleType.Article:
                    await RequestArticleDataAsync();
                    break;
                default:
                    break;
            }

            CheckModuleContentEmpty();
        }

        private async Task RequestVideoDataAsync()
        {
            var orderType = GetCondition(OrderType);
            var partitionId = GetCondition(PartitionId);
            var duration = GetCondition(Duration);
            var data = await _searchProvider.GetComprehensiveSearchResultAsync(Keyword, orderType, partitionId, duration);

            var isProxySearch = _settingsToolkit.ReadLocalSetting(SettingNames.IsOpenRoaming, false)
                    && !string.IsNullOrEmpty(_settingsToolkit.ReadLocalSetting(SettingNames.RoamingSearchAddress, string.Empty));

            // 处理元数据.
            if (data.Metadata != null && data.Metadata.Count > 0 && !isProxySearch)
            {
                foreach (var item in data.Metadata)
                {
                    var module = Items.FirstOrDefault(p => p.Type == item.Key);
                    if (module != null)
                    {
                        module.IsEnabled = item.Value > 0;
                    }
                }
            }

            // 处理视频.
            if (data.VideoSet != null)
            {
                ResetModuleEndIdentifier(SearchModuleType.Video, data.VideoSet.IsEnd);
                foreach (var item in data.VideoSet.Items)
                {
                    if (!Videos.Any(p => p.Data.Equals(item)))
                    {
                        var videoVM = Locator.Current.GetService<IVideoItemViewModel>();
                        videoVM.InjectData(item);
                        Videos.Add(videoVM);
                    }
                }
            }
        }

        private async Task RequestAnimeDataAsync()
        {
            var data = await _searchProvider.GetAnimeSearchResultAsync(Keyword, TotalRank);
            ResetModuleEndIdentifier(SearchModuleType.Anime, data.IsEnd);
            foreach (var item in data.Items)
            {
                if (!Animes.Any(p => p.Data.Equals(item)))
                {
                    var seasonVM = Splat.Locator.Current.GetService<ISeasonItemViewModel>();
                    seasonVM.InjectData(item);
                    Animes.Add(seasonVM);
                }
            }
        }

        private async Task RequestMovieDataAsync()
        {
            var data = await _searchProvider.GetMovieSearchResultAsync(Keyword, TotalRank);
            ResetModuleEndIdentifier(SearchModuleType.Movie, data.IsEnd);
            foreach (var item in data.Items)
            {
                if (!Movies.Any(p => p.Data.Equals(item)))
                {
                    var seasonVM = Splat.Locator.Current.GetService<ISeasonItemViewModel>();
                    seasonVM.InjectData(item);
                    Movies.Add(seasonVM);
                }
            }
        }

        private async Task RequestLiveDataAsync()
        {
            var data = await _searchProvider.GetLiveSearchResultAsync(Keyword);
            ResetModuleEndIdentifier(SearchModuleType.Live, data.IsEnd);
            foreach (var item in data.Items)
            {
                if (!Lives.Any(p => p.Data.Equals(item)))
                {
                    var liveVM = Locator.Current.GetService<ILiveItemViewModel>();
                    liveVM.InjectData(item);
                    Lives.Add(liveVM);
                }
            }
        }

        private async Task RequestUserDataAsync()
        {
            var orderType = GetCondition(OrderType);
            var sp = orderType.Split("_");
            orderType = sp[0];
            var orderSort = sp[1];
            var userType = GetCondition(UserType);
            var data = await _searchProvider.GetUserSearchResultAsync(Keyword, orderType, orderSort, userType);
            ResetModuleEndIdentifier(SearchModuleType.User, data.IsEnd);
            foreach (var item in data.Items)
            {
                if (!Users.Any(p => p.User.Equals(item.User)))
                {
                    var userVM = Splat.Locator.Current.GetService<IUserItemViewModel>();
                    userVM.SetInformation(item);
                    Users.Add(userVM);
                }
            }
        }

        private async Task RequestArticleDataAsync()
        {
            var orderType = GetCondition(OrderType);
            var partitionId = GetCondition(PartitionId);
            var data = await _searchProvider.GetArticleSearchResultAsync(Keyword, orderType, partitionId);
            ResetModuleEndIdentifier(SearchModuleType.Article, data.IsEnd);
            foreach (var item in data.Items)
            {
                if (!Articles.Any(p => p.Data.Equals(item)))
                {
                    var articleVM = Splat.Locator.Current.GetService<IArticleItemViewModel>();
                    articleVM.InjectData(item);
                    Articles.Add(articleVM);
                }
            }
        }

        private string GetCondition(string id)
            => CurrentFilters.FirstOrDefault(p => p.Filter.Id == id)?.CurrentCondition?.Id ?? string.Empty;

        private void ResetModuleEndIdentifier(SearchModuleType type, bool isEnd)
        {
            _requestStatusCache.Remove(type);
            _requestStatusCache.Add(type, isEnd);
        }
    }
}
