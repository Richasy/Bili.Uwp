// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.App.Args;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendViewModel"/> class.
        /// </summary>
        internal RecommendViewModel()
        {
            _controller = BiliController.Instance;
            VideoCollection = new ObservableCollection<VideoViewModel>();
            _offsetIndex = 0;
            _controller.HomeVideoIteration += OnHomeVideoIteration;
        }

        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDataAsync()
        {
            if (_offsetIndex == 0)
            {
                await InitializeRequestAsync();
            }
            else
            {
                await DeltaRequestAsync();
            }
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
                    await _controller.RequestRecommendCardsAsync(_offsetIndex);
                }
                catch (Exception)
                {
                    IsError = true;
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
                await _controller.RequestRecommendCardsAsync(_offsetIndex);
                IsDeltaLoading = false;
            }
        }

        /// <summary>
        /// 清空视图模型中已缓存的数据.
        /// </summary>
        public void Reset()
        {
            _offsetIndex = 0;
            VideoCollection.Clear();
        }

        private void OnHomeVideoIteration(object sender, HomeVideoIterationEventArgs e)
        {
            _offsetIndex = e.OffsetIndex;

            if (e.Cards?.Any() ?? false)
            {
                e.Cards.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
            }
        }
    }
}
