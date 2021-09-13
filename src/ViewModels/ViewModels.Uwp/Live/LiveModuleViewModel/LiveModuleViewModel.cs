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
    /// 直播视图模型.
    /// </summary>
    public partial class LiveModuleViewModel : WebRequestViewModelBase, IDeltaRequestViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveModuleViewModel"/> class.
        /// </summary>
        internal LiveModuleViewModel()
        {
            _currentPage = 1;
            BannerCollection = new ObservableCollection<BannerViewModel>();
            FollowLiveRoomCollection = new ObservableCollection<VideoViewModel>();
            RecommendLiveRoomCollection = new ObservableCollection<VideoViewModel>();

            Controller.LiveFeedRoomIteration += OnLiveFeedRoomIteration;
            Controller.LiveFeedAdditionalDataChanged += OnLiveFeedAdditionalDataChanged;
        }

        /// <summary>
        /// 请求直播源列表.
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

            IsRequested = _currentPage > 1;
        }

        /// <summary>
        /// 初始化直播源列表请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                _currentPage = 1;
                IsError = false;
                ErrorText = string.Empty;
                RecommendLiveRoomCollection.Clear();
                BannerCollection.Clear();
                FollowLiveRoomCollection.Clear();
                IsShowFollowList = false;

                try
                {
                    await Controller.RequestLiveFeedsAsync(_currentPage);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestLiveFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }

            IsRequested = _currentPage > 1;
        }

        internal async Task DeltaRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsDeltaLoading = true;
                await Controller.RequestLiveFeedsAsync(_currentPage);
                IsDeltaLoading = false;
            }
        }

        private void OnLiveFeedRoomIteration(object sender, LiveFeedRoomIterationEventArgs e)
        {
            var list = e.List;
            _currentPage = e.NextPageNumber;
            foreach (var item in list)
            {
                if (!RecommendLiveRoomCollection.Any(p => p.VideoId == item.RoomId.ToString()))
                {
                    RecommendLiveRoomCollection.Add(new VideoViewModel(item));
                }
            }
        }

        private void OnLiveFeedAdditionalDataChanged(object sender, LiveFeedAdditionalDataChangedEventArgs e)
        {
            if (e.BannerList != null)
            {
                BannerCollection.Clear();
                e.BannerList.ForEach(p => BannerCollection.Add(new BannerViewModel(p)));
            }

            if (e.FollowUserList != null)
            {
                FollowLiveRoomCollection.Clear();
                e.FollowUserList.ForEach(p => FollowLiveRoomCollection.Add(new VideoViewModel(p)));
                IsShowFollowList = FollowLiveRoomCollection.Count > 0;
            }
        }
    }
}
