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

            IsRequested = _cursor != null && _cursor.Max != 0;
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
                _cursor = new Bilibili.App.Interfaces.V1.Cursor { Max = 0 };
                IsError = false;
                ErrorText = string.Empty;
                try
                {
                    // request.
                    await Controller.RequestHistorySetAsync(_cursor);
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

            IsRequested = _cursor != null && _cursor.Max != 0;
        }

        /// <summary>
        /// 执行增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        internal async Task DeltaRequestAsync()
        {
            if (!IsDeltaLoading)
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
                    VideoCollection.Add(new VideoViewModel(item));
                }
            }
        }
    }
}
