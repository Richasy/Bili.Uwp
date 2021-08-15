// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
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
        {
            Type = type;
            switch (type)
            {
                case SearchModuleType.Video:
                    VideoCollection = new ObservableCollection<VideoViewModel>();
                    Controller.VideoSearchIteration += OnVideoSearchIteration;
                    break;
                case SearchModuleType.Bangumi:
                    break;
                case SearchModuleType.Live:
                    break;
                case SearchModuleType.User:
                    break;
                case SearchModuleType.Movie:
                    break;
                case SearchModuleType.Article:
                    break;
                default:
                    break;
            }
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
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                Reset();
                try
                {
                    await Controller.RequestSearchModuleDataAsync(Type, Keyword, CurrentOrder, 1);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestPopularFailed)}\n{ex.Error?.Message ?? ex.Message}";
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
                await Controller.RequestSearchModuleDataAsync(Type, Keyword, CurrentOrder, PageNumber);
                IsDeltaLoading = false;
            }
        }

        /// <summary>
        /// 清空视图模型中已缓存的数据.
        /// </summary>
        public void Reset()
        {
            PageNumber = 0;
            IsError = false;
            ErrorText = string.Empty;
            Keyword = string.Empty;
            IsLoadCompleted = false;

            switch (Type)
            {
                case SearchModuleType.Video:
                    CurrentOrder = "default";
                    VideoCollection.Clear();
                    break;
                case SearchModuleType.Bangumi:
                    break;
                case SearchModuleType.Live:
                    break;
                case SearchModuleType.User:
                    break;
                case SearchModuleType.Movie:
                    break;
                case SearchModuleType.Article:
                    break;
                default:
                    break;
            }

            IsRequested = false;
        }

        private void OnVideoSearchIteration(object sender, VideoSearchEventArgs e)
        {
            if (e.Keyword == Keyword)
            {
                IsLoadCompleted = false;
                PageNumber = e.NextPageNumber;
                e.List.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
            }
        }
    }
}
