// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendViewModel : WebRequestViewModelBase, IDeltaRequestViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendViewModel"/> class.
        /// </summary>
        internal RecommendViewModel()
        {
            VideoCollection = new ObservableCollection<VideoViewModel>();
            _offsetIndex = 0;
            Controller.RecommendVideoIteration += OnRecommendVideoIteration;
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
                    await Controller.RequestRecommendCardsAsync(_offsetIndex);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestRecommendFailed)}\n{ex.Error?.Message ?? ex.Message}";
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
                await Controller.RequestRecommendCardsAsync(_offsetIndex);
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
        }

        private void OnRecommendVideoIteration(object sender, RecommendVideoIterationEventArgs e)
        {
            _offsetIndex = e.OffsetIndex;

            if (e.Cards?.Any() ?? false)
            {
                e.Cards.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
            }
        }
    }
}
