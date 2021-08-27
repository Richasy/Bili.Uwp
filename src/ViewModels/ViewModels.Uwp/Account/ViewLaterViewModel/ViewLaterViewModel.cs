// Copyright (c) Richasy. All rights reserved.

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
            IsShowRuntimeError = false;
            var result = await Controller.ClearViewLaterAsync();
            if (!result)
            {
                IsShowRuntimeError = true;
                RuntimeErrorText = ResourceToolkit.GetLocaleString(LanguageNames.FailedToClearViewLater);
            }
            else
            {
                VideoCollection.Clear();
                IsShowEmpty = VideoCollection.Count == 0;
            }
        }

        /// <summary>
        /// 将视频移出稍后再看列表.
        /// </summary>
        /// <param name="vm">视频模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RemoveAsync(VideoViewModel vm)
        {
            IsShowRuntimeError = false;
            var result = await Controller.RemoveVideoFromViewLaterAsync(Convert.ToInt32(vm.VideoId));
            if (!result)
            {
                IsShowRuntimeError = true;
                RuntimeErrorText = ResourceToolkit.GetLocaleString(LanguageNames.FailedToRemoveVideoFromViewLater);
            }
            else
            {
                VideoCollection.Remove(vm);
                IsShowEmpty = VideoCollection.Count == 0;
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
            IsShowEmpty = VideoCollection.Count == 0;
        }
    }
}
