// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频收藏夹视图模型.
    /// </summary>
    public partial class FavoriteVideoViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteVideoViewModel"/> class.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        public FavoriteVideoViewModel(int favoriteId)
        {
            Id = favoriteId;
            _pageNumber = 1;
            VideoCollection = new ObservableCollection<VideoViewModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteVideoViewModel"/> class.
        /// </summary>
        /// <param name="response">收藏夹详情.</param>
        public FavoriteVideoViewModel(VideoFavoriteListResponse response)
            : this(response.Detail.Id)
        {
            HandleResponse(response, true);
            IsRequested = true;
        }

        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDataAsync()
        {
            if (IsRequested)
            {
                await DeltaRequestAsync();
            }
            else
            {
                await InitializeRequestAsync();
            }
        }

        /// <summary>
        /// 执行初始请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                VideoCollection.Clear();
                _isLoadCompleted = false;
                _pageNumber = 1;
                IsError = false;
                ErrorText = string.Empty;
                try
                {
                    var response = await Controller.GetVideoFavoriteListAsync(Id, _pageNumber);
                    HandleResponse(response, !IsRequested);
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestVideoFavoriteFailed)}\n{ex.Error?.Message ?? ex.Message}";
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
        /// 执行增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        internal async Task DeltaRequestAsync()
        {
            if (!IsDeltaLoading && !_isLoadCompleted)
            {
                IsDeltaLoading = true;
                var response = await Controller.GetVideoFavoriteListAsync(Id, _pageNumber);
                HandleResponse(response);
                IsDeltaLoading = false;
            }
        }

        private void HandleResponse(VideoFavoriteListResponse response, bool isInit = false)
        {
            if (response == null)
            {
                return;
            }

            if (isInit)
            {
                var detail = response.Detail ?? response.Information;
                User = new UserViewModel(detail.Publisher);
                Name = detail.Title;
                Description = detail.Description;
                TotalCount = detail.MediaCount;
            }

            if (response.Medias != null && response.Medias.Count > 0)
            {
                response.Medias.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
            }

            IsShowEmpty = VideoCollection.Count == 0;
            _isLoadCompleted = !response.HasMore;
            if (!_isLoadCompleted)
            {
                _pageNumber += 1;
            }
        }
    }
}
