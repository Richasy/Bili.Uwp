// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.Enums;

using static Bili.Models.App.Constants.ControllerConstants.Search;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 搜索模块视图模型.
    /// </summary>
    /// <typeparam name="T">内部数据类型.</typeparam>
    public partial class SearchSubModuleViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSubModuleViewModel"/> class.
        /// </summary>
        /// <param name="type">模块类型.</param>
        public SearchSubModuleViewModel(SearchModuleType type)
            : base()
        {
            Type = type;
            switch (type)
            {
                case SearchModuleType.Video:
                    VideoCollection = new ObservableCollection<VideoViewModel>();
                    IsEnabled = true;
                    Controller.VideoSearchIteration += OnVideoSearchIteration;
                    InitializeVideoFilters();
                    break;
                case SearchModuleType.Bangumi:
                    PgcCollection = new ObservableCollection<SeasonViewModel>();
                    Controller.BangumiSearchIteration += OnPgcSearchIteration;
                    break;
                case SearchModuleType.Live:
                    VideoCollection = new ObservableCollection<VideoViewModel>();
                    Controller.LiveSearchIteration += OnLiveSearchIteration;
                    break;
                case SearchModuleType.User:
                    UserCollection = new ObservableCollection<UserViewModel>();
                    Controller.UserSearchIteration += OnUserSearchIteration;
                    InitializeUserFilters();
                    break;
                case SearchModuleType.Movie:
                    PgcCollection = new ObservableCollection<SeasonViewModel>();
                    Controller.MovieSearchIteration += OnPgcSearchIteration;
                    break;
                case SearchModuleType.Article:
                    ArticleCollection = new ObservableCollection<ArticleViewModel>();
                    Controller.ArticleSearchIteration += OnArticleSearchIteration;
                    InitializeArticleFiltersAsync();
                    break;
                default:
                    break;
            }

            this.PropertyChanged += OnPropertyChangedAsync;
        }

        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDataAsync()
        {
            if (!IsRequested)
            {
                await InitializeRequestAsync();
            }
            else
            {
                await DeltaRequestAsync();
            }

            IsRequested = PageNumber != 0;
        }

        /// <summary>
        /// 初始化请求.
        /// </summary>
        /// <param name="isClearFilter">是否清除筛选条件.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync(bool isClearFilter = true)
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                Reset(isClearFilter);
                try
                {
                    var queryParameters = GetQueryParameters();
                    await Controller.RequestSearchModuleDataAsync(Type, Keyword, 1, queryParameters);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestSearchResultFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }
        }

        /// <summary>
        /// 增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task DeltaRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading && !IsLoadCompleted)
            {
                IsDeltaLoading = true;
                var queryParameters = GetQueryParameters();
                await Controller.RequestSearchModuleDataAsync(Type, Keyword, PageNumber, queryParameters);
                IsDeltaLoading = false;
            }
        }

        /// <summary>
        /// 清空视图模型中已缓存的数据.
        /// </summary>
        /// <param name="isClearFilter">是否清除筛选条件.</param>
        public void Reset(bool isClearFilter)
        {
            PageNumber = 0;
            IsError = false;
            ErrorText = string.Empty;
            IsLoadCompleted = false;
            IsRequested = false;
            switch (Type)
            {
                case SearchModuleType.Video:
                    if (isClearFilter)
                    {
                        CurrentOrder = OrderCollection.First();
                        CurrentDuration = VideoDurationCollection.First();
                        CurrentPartitionId = PartitionCollection.FirstOrDefault();
                    }

                    VideoCollection.Clear();
                    break;
                case SearchModuleType.Bangumi:
                case SearchModuleType.Movie:
                    PgcCollection.Clear();
                    break;
                case SearchModuleType.Live:
                    VideoCollection.Clear();
                    break;
                case SearchModuleType.User:
                    if (isClearFilter)
                    {
                        CurrentOrder = OrderCollection.First();
                        CurrentUserType = UserTypeCollection.FirstOrDefault();
                    }

                    UserCollection.Clear();
                    break;
                case SearchModuleType.Article:
                    if (isClearFilter)
                    {
                        CurrentOrder = OrderCollection.First();
                        CurrentPartitionId = PartitionCollection.FirstOrDefault();
                    }

                    ArticleCollection.Clear();
                    break;
                default:
                    break;
            }
        }

        private void OnVideoSearchIteration(object sender, VideoSearchIterationEventArgs e)
        {
            if (e.Keyword == Keyword)
            {
                if (e.List != null)
                {
                    foreach (var item in e.List)
                    {
                        if (!VideoCollection.Any(p => p.VideoId == item.Parameter))
                        {
                            VideoCollection.Add(new VideoViewModel(item));
                        }
                    }
                }

                HandleSearchIterationArgs(e, VideoCollection.Count);
            }
        }

        private void OnPgcSearchIteration(object sender, PgcSearchIterationEventArgs e)
        {
            if (e.Keyword == Keyword)
            {
                if (e.List != null)
                {
                    foreach (var item in e.List)
                    {
                        if (!PgcCollection.Any(p => p.SeasonId == item.SeasonId))
                        {
                            PgcCollection.Add(new SeasonViewModel(item));
                        }
                    }
                }

                HandleSearchIterationArgs(e, PgcCollection.Count);
            }
        }

        private void OnArticleSearchIteration(object sender, ArticleSearchIterationEventArgs e)
        {
            if (e.Keyword == Keyword)
            {
                if (e.List != null)
                {
                    foreach (var item in e.List)
                    {
                        if (!ArticleCollection.Any(p => p.Id == item.Id.ToString()))
                        {
                            ArticleCollection.Add(new ArticleViewModel(item));
                        }
                    }
                }

                HandleSearchIterationArgs(e, ArticleCollection.Count);
            }
        }

        private void OnUserSearchIteration(object sender, UserSearchIterationEventArgs e)
        {
            if (e.Keyword == Keyword)
            {
                if (e.List != null)
                {
                    foreach (var item in e.List)
                    {
                        if (!UserCollection.Any(p => p.Id == item.UserId))
                        {
                            UserCollection.Add(new UserViewModel(item));
                        }
                    }
                }

                HandleSearchIterationArgs(e, UserCollection.Count);
            }
        }

        private void OnLiveSearchIteration(object sender, LiveSearchIterationEventArgs e)
        {
            if (e.Keyword == Keyword)
            {
                if (e.List != null)
                {
                    foreach (var item in e.List)
                    {
                        if (!VideoCollection.Any(p => p.VideoId == item.RoomId.ToString()))
                        {
                            VideoCollection.Add(new VideoViewModel(item));
                        }
                    }
                }

                HandleSearchIterationArgs(e, VideoCollection.Count);
            }
        }

        private void HandleSearchIterationArgs(SearchIterationEventArgs args, int currentCount)
        {
            IsLoadCompleted = !args.HasMore && currentCount >= Total;
            if (!IsLoadCompleted)
            {
                PageNumber = args.NextPageNumber;
            }
            else if (PageNumber == 0)
            {
                PageNumber = 1;
            }

            IsShowEmpty = currentCount == 0;
            IsRequested = true;
        }

        private Dictionary<string, string> GetQueryParameters()
        {
            var result = new Dictionary<string, string>();
            switch (Type)
            {
                case SearchModuleType.Video:
                    result.Add(OrderType, CurrentOrder.Key);
                    result.Add(Duration, CurrentDuration.Key);
                    result.Add(PartitionId, CurrentPartitionId.Key);
                    break;
                case SearchModuleType.User:
                    var userOrderSp = CurrentOrder.Key.Split("_");
                    result.Add(OrderType, userOrderSp[0]);
                    result.Add(OrderSort, userOrderSp[1]);
                    result.Add(UserType, CurrentUserType.Key);
                    break;
                case SearchModuleType.Article:
                    result.Add(OrderType, CurrentOrder.Key);
                    result.Add(PartitionId, CurrentPartitionId.Key);
                    break;
                default:
                    break;
            }

            return result;
        }

        private void InitializeVideoFilters()
        {
            OrderCollection = new ObservableCollection<KeyValue<string>>();
            VideoDurationCollection = new ObservableCollection<KeyValue<string>>();
            PartitionCollection = new ObservableCollection<KeyValue<string>>();

            OrderCollection.Add(new KeyValue<string>("default", ResourceToolkit.GetLocaleString(LanguageNames.SortByDefault)));
            OrderCollection.Add(new KeyValue<string>("view", ResourceToolkit.GetLocaleString(LanguageNames.SortByPlay)));
            OrderCollection.Add(new KeyValue<string>("pubdate", ResourceToolkit.GetLocaleString(LanguageNames.SortByNewest)));
            OrderCollection.Add(new KeyValue<string>("danmaku", ResourceToolkit.GetLocaleString(LanguageNames.SortByDanmaku)));

            VideoDurationCollection.Add(new KeyValue<string>("0", ResourceToolkit.GetLocaleString(LanguageNames.FilterByTotalDuration)));
            VideoDurationCollection.Add(new KeyValue<string>("1", ResourceToolkit.GetLocaleString(LanguageNames.FilterByLessThan10Min)));
            VideoDurationCollection.Add(new KeyValue<string>("2", ResourceToolkit.GetLocaleString(LanguageNames.FilterByLessThan30Min)));
            VideoDurationCollection.Add(new KeyValue<string>("3", ResourceToolkit.GetLocaleString(LanguageNames.FilterByLessThan60Min)));
            VideoDurationCollection.Add(new KeyValue<string>("4", ResourceToolkit.GetLocaleString(LanguageNames.FilterByGreaterThan60Min)));

            // var totalPartition = PartitionModuleViewModel.Instance.PartitionCollection;
            // if (totalPartition.Count == 0)
            // {
            //    await PartitionModuleViewModel.Instance.InitializeAllPartitionAsync();
            //    totalPartition = PartitionModuleViewModel.Instance.PartitionCollection;
            // }

            // totalPartition.ToList().ForEach(p => PartitionCollection.Add(new KeyValue<string>(p.PartitionId.ToString(), p.Title)));
            // PartitionCollection.Insert(0, new KeyValue<string>("0", ResourceToolkit.GetLocaleString(LanguageNames.Total)));
        }

        private async void InitializeArticleFiltersAsync()
        {
            OrderCollection = new ObservableCollection<KeyValue<string>>();
            PartitionCollection = new ObservableCollection<KeyValue<string>>();

            OrderCollection.Add(new KeyValue<string>(string.Empty, ResourceToolkit.GetLocaleString(LanguageNames.SortByDefault)));
            OrderCollection.Add(new KeyValue<string>("pubdate", ResourceToolkit.GetLocaleString(LanguageNames.SortByNewest)));
            OrderCollection.Add(new KeyValue<string>("click", ResourceToolkit.GetLocaleString(LanguageNames.SortByRead)));
            OrderCollection.Add(new KeyValue<string>("scores", ResourceToolkit.GetLocaleString(LanguageNames.SortByReply)));
            OrderCollection.Add(new KeyValue<string>("attention", ResourceToolkit.GetLocaleString(LanguageNames.SortByLike)));

            var totalPartition = SpecialColumnModuleViewModel.Instance.CategoryCollection;
            if (totalPartition.Count == 0)
            {
                await SpecialColumnModuleViewModel.Instance.RequestCategoriesAsync();
                totalPartition = SpecialColumnModuleViewModel.Instance.CategoryCollection;
            }

            totalPartition.ToList().ForEach(p => PartitionCollection.Add(new KeyValue<string>(p.Id.ToString(), p.Title)));
        }

        private void InitializeUserFilters()
        {
            OrderCollection = new ObservableCollection<KeyValue<string>>();
            UserTypeCollection = new ObservableCollection<KeyValue<string>>();

            OrderCollection.Add(new KeyValue<string>("totalrank_0", ResourceToolkit.GetLocaleString(LanguageNames.SortByDefault)));
            OrderCollection.Add(new KeyValue<string>("fan_0", ResourceToolkit.GetLocaleString(LanguageNames.SortByFansHTL)));
            OrderCollection.Add(new KeyValue<string>("fan_1", ResourceToolkit.GetLocaleString(LanguageNames.SortByFansLTH)));
            OrderCollection.Add(new KeyValue<string>("level_0", ResourceToolkit.GetLocaleString(LanguageNames.SortByLevelHTL)));
            OrderCollection.Add(new KeyValue<string>("level_1", ResourceToolkit.GetLocaleString(LanguageNames.SortByLevelLTH)));

            UserTypeCollection.Add(new KeyValue<string>("0", ResourceToolkit.GetLocaleString(LanguageNames.TotalUser)));
            UserTypeCollection.Add(new KeyValue<string>("1", ResourceToolkit.GetLocaleString(LanguageNames.UpMaster)));
            UserTypeCollection.Add(new KeyValue<string>("2", ResourceToolkit.GetLocaleString(LanguageNames.NormalUser)));
            UserTypeCollection.Add(new KeyValue<string>("3", ResourceToolkit.GetLocaleString(LanguageNames.OfficialUser)));
        }

        private async void OnPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Current"))
            {
                if (IsRequested)
                {
                    await InitializeRequestAsync(false);
                }
            }
        }
    }
}
