// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bilibili.App.Interfaces.V1;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 历史记录视图模型.
    /// </summary>
    public partial class HistoryViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryViewModel"/> class.
        /// </summary>
        protected HistoryViewModel()
        {
            VideoCollection = new ObservableCollection<VideoViewModel>();
            _cursor = null;
            Controller.HistoryVideoIteration += OnHistoryVideoIteration;
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
                _cursor = new Cursor { Max = 0 };
                IsError = false;
                ErrorText = string.Empty;
                IsShowEmpty = false;
                _isLoadCompleted = false;
                try
                {
                    // request.
                    await Controller.RequestHistorySetAsync(_cursor);
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestHistoryFailed)}\n{ex.Error?.Message ?? ex.Message}";
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
        /// 删除历史记录条目.
        /// </summary>
        /// <param name="vm">视图模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task DeleteItemAsync(VideoViewModel vm)
        {
            var source = vm.Source as CursorItem;
            var result = await Controller.RemoveHistoryItemAsync(source.Kid);
            if (result)
            {
                VideoCollection.Remove(vm);
                CheckStatus();
            }
            else
            {
                AppViewModel.Instance.ShowTip(ResourceToolkit.GetLocaleString(LanguageNames.FailedToRemoveVideoFromHistory), Models.Enums.App.InfoType.Error);
            }
        }

        /// <summary>
        /// 清除历史记录.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ClearAsync()
        {
            var result = await Controller.ClearHistoryAsync();
            if (!result)
            {
                AppViewModel.Instance.ShowTip(ResourceToolkit.GetLocaleString(LanguageNames.FailedToClearHisotry), Models.Enums.App.InfoType.Error);
            }
            else
            {
                VideoCollection.Clear();
                CheckStatus();
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
                await Controller.RequestHistorySetAsync(_cursor);
                IsDeltaLoading = false;
            }
        }

        private void OnHistoryVideoIteration(object sender, HistoryVideoIterationEventArgs e)
        {
            _cursor = e.Cursor;
            if (e.List != null && e.List.Count > 0)
            {
                foreach (var item in e.List)
                {
                    if (item.CardItemCase == CursorItem.CardItemOneofCase.CardUgc || item.CardItemCase == CursorItem.CardItemOneofCase.CardOgv)
                    {
                        VideoCollection.Add(new VideoViewModel(item));
                    }
                }
            }
            else
            {
                _isLoadCompleted = true;
            }

            CheckStatus();
        }

        private void CheckStatus()
        {
            IsShowEmpty = VideoCollection.Count == 0;
            IsClearButtonEnabled = !IsShowEmpty;
        }
    }
}
