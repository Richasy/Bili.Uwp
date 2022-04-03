// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
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
            HotTagCollection = new ObservableCollection<LiveFeedHotArea>();
            LiveAreaGroupCollection = new ObservableCollection<LiveAreaGroup>();
            DisplayAreaCollection = new ObservableCollection<LiveArea>();

            Controller.LiveFeedRoomIteration += OnLiveFeedRoomIteration;
            Controller.LiveFeedAdditionalDataChanged += OnLiveFeedAdditionalDataChanged;

            this.WhenAnyValue(x => x.SelectedAreaGroup)
                .WhereNotNull()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => ChangeSelectedAreaGroup());
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

        /// <summary>
        /// 初始化分区索引.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeAreaIndexAsync()
        {
            IsLiveAreaRequesting = true;
            IsError = false;

            try
            {
                SelectedAreaGroup = null;
                LiveAreaGroupCollection.Clear();
                DisplayAreaCollection.Clear();
                var response = await Controller.GetLiveAreaIndexAsync();
                response.List.ForEach(p => LiveAreaGroupCollection.Add(p));
                SelectedAreaGroup = LiveAreaGroupCollection.First();
                SelectedAreaGroup.AreaList.ForEach(p => DisplayAreaCollection.Add(p));
            }
            catch (Exception)
            {
                IsAreaError = true;
            }

            IsLiveAreaRequesting = false;
        }

        /// <summary>
        /// 改变选中的分区组.
        /// </summary>
        /// <param name="group">分区组.</param>
        public void ChangeSelectedAreaGroup()
        {
            if (SelectedAreaGroup != null)
            {
                DisplayAreaCollection.Clear();
                SelectedAreaGroup.AreaList.ForEach((p) => DisplayAreaCollection.Add(p));
            }
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

            if (e.HotAreaList != null)
            {
                HotTagCollection.Clear();
                e.HotAreaList.Where(p => p.Id != 0).ToList().ForEach(p => HotTagCollection.Add(p));
            }
        }
    }
}
