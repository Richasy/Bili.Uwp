// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 热门视图模型.
    /// </summary>
    public partial class PopularViewModel : WebRequestViewModelBase, IDeltaRequestViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopularViewModel"/> class.
        /// </summary>
        internal PopularViewModel()
        {
            VideoCollection = new ObservableCollection<VideoViewModel>();
            _offsetIndex = 0;
            Controller.PopularVideoIteration += OnPopularVideoIteration;
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

            IsRequested = _offsetIndex > 0;
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
                    await Controller.RequestPopularCardsAsync(_offsetIndex);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestPopularFailed)}\n{ex.Error?.Message ?? ex.Message}";
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
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsDeltaLoading = true;
                await Controller.RequestPopularCardsAsync(_offsetIndex);
                IsDeltaLoading = false;
            }
        }

        /// <summary>
        /// 清空视图模型中已缓存的数据.
        /// </summary>
        public void Reset()
        {
            _offsetIndex = 0;
            IsError = false;
            ErrorText = string.Empty;
            VideoCollection.Clear();
            IsRequested = false;
        }

        private void OnPopularVideoIteration(object sender, PopularVideoIterationEventArgs e)
        {
            _offsetIndex = e.OffsetIndex;

            if (e.Cards?.Any() ?? false)
            {
                e.Cards.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
            }
        }
    }
}
