﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 稍后再看视图模型.
    /// </summary>
    public partial class ViewLaterViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterViewModel"/> class.
        /// </summary>
        protected ViewLaterViewModel()
        {
            VideoCollection = new ObservableCollection<VideoViewModel>();
            Controller.ViewLaterVideoIteration += OnViewLaterVideoIteration;
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

            IsRequested = _pageNumber >= 1;
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
                IsShowEmpty = false;
                _isLoadCompleted = false;
                VideoCollection.Clear();
                _pageNumber = 0;
                IsError = false;
                ErrorText = string.Empty;
                try
                {
                    await Controller.RequestViewLaterListAsync(1);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestViewLaterFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (Exception e)
                {
                    IsError = true;
                    ErrorText = $"{e.Message}";
                }

                IsInitializeLoading = false;
            }

            IsRequested = _pageNumber != 0;
        }

        /// <summary>
        /// 清除稍后再看列表.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ClearAsync()
        {
            var result = await Controller.ClearViewLaterAsync();
            if (!result)
            {
                AppViewModel.Instance.ShowTip(ResourceToolkit.GetLocaleString(LanguageNames.FailedToClearViewLater), Models.Enums.App.InfoType.Error);
            }
            else
            {
                VideoCollection.Clear();
                CheckStatus();
            }
        }

        /// <summary>
        /// 将视频移出稍后再看列表.
        /// </summary>
        /// <param name="vm">视频模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RemoveAsync(VideoViewModel vm)
        {
            var result = await Controller.RemoveVideoFromViewLaterAsync(Convert.ToInt32(vm.VideoId));
            if (!result)
            {
                AppViewModel.Instance.ShowTip(ResourceToolkit.GetLocaleString(LanguageNames.FailedToRemoveVideoFromViewLater), Models.Enums.App.InfoType.Error);
            }
            else
            {
                VideoCollection.Remove(vm);
                CheckStatus();
            }
        }

        /// <summary>
        /// 添加到稍后再看列表.
        /// </summary>
        /// <param name="vm">视图模型.</param>
        /// <returns>添加的结果.</returns>
        public async Task<bool> AddAsync(VideoViewModel vm)
        {
            var result = await Controller.AddVideoToViewLaterAsync(Convert.ToInt32(vm.VideoId));
            if (result)
            {
                AppViewModel.Instance.ShowTip(ResourceToolkit.GetLocaleString(LanguageNames.AddViewLaterSucceseded), Models.Enums.App.InfoType.Success);
            }
            else
            {
                AppViewModel.Instance.ShowTip(ResourceToolkit.GetLocaleString(LanguageNames.AddViewLaterFailed), Models.Enums.App.InfoType.Error);
            }

            return result;
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
                await Controller.RequestViewLaterListAsync(_pageNumber);
                IsDeltaLoading = false;
            }
        }

        private void OnViewLaterVideoIteration(object sender, ViewLaterVideoIterationEventArgs e)
        {
            if (e.List?.Any() ?? false)
            {
                e.List.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
            }

            _pageNumber = e.NextPageNumber;
            _isLoadCompleted = e.TotalCount <= VideoCollection.Count;
            CheckStatus();
        }

        private void CheckStatus()
        {
            IsShowEmpty = VideoCollection.Count == 0;
            IsClearButtonEnabled = !IsShowEmpty;
        }
    }
}
