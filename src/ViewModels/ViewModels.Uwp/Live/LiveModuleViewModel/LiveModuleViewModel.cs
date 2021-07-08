// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 直播视图模型.
    /// </summary>
    public partial class LiveModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveModuleViewModel"/> class.
        /// </summary>
        internal LiveModuleViewModel()
        {
            _controller = BiliController.Instance;
            _currentPage = 1;
            BannerCollection = new ObservableCollection<BannerViewModel>();

            ServiceLocator.Instance.LoadService(out _resourceToolkit);

            _controller.LiveFeedRoomIteration += OnLiveFeedRoomIteration;
            _controller.LiveFeedAdditionalDataChanged += OnLiveFeedAdditionalDataChanged;
        }

        /// <summary>
        /// 请求直播源列表.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestFeedsAsync()
        {
            if (_currentPage == 1)
            {
                await InitializeRequestFeedsAsync();
            }
            else
            {
                await DeltaRequestFeedsAsync();
            }
        }

        /// <summary>
        /// 初始化直播源列表请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestFeedsAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                _currentPage = 1;
                IsError = false;
                ErrorText = string.Empty;

                try
                {
                    await _controller.RequestLiveFeedsAsync(_currentPage);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestSubPartitionFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
            }
        }

        internal async Task DeltaRequestFeedsAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsDeltaLoading = true;
                await _controller.RequestLiveFeedsAsync(_currentPage);
                IsDeltaLoading = false;
            }
        }

        private void OnLiveFeedRoomIteration(object sender, LiveFeedRoomIterationEventArgs e)
        {
            var list = e.List;
        }

        private void OnLiveFeedAdditionalDataChanged(object sender, LiveFeedAdditionalDataChangedEventArgs e)
        {
            if (e.BannerList != null)
            {
                BannerCollection.Clear();
                e.BannerList.ForEach(p => BannerCollection.Add(new BannerViewModel(p)));
            }
        }
    }
}
