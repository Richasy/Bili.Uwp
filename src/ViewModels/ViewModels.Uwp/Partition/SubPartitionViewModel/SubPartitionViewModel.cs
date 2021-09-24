// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 子分区视图模型.
    /// </summary>
    public partial class SubPartitionViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubPartitionViewModel"/> class.
        /// </summary>
        /// <param name="partition">子分区数据.</param>
        public SubPartitionViewModel(Partition partition)
        {
            BannerCollection = new ObservableCollection<BannerViewModel>();
            VideoCollection = new ObservableCollection<VideoViewModel>();
            TagCollection = new ObservableCollection<Tag>();
            this._partition = partition;
            SubPartitionId = partition?.Tid ?? -1;
            if (this._partition != null)
            {
                this.Title = this._partition.Name;
                this._isRecommendPartition = false;
                GenerateSortType();
                CurrentSortType = VideoSortType.Default;
            }
            else
            {
                this.Title = this.ResourceToolkit.GetLocaleString(LanguageNames.Recommend);
                this._isRecommendPartition = true;
            }

            this.PropertyChanged += OnPropertyChangedAsync;
        }

        /// <summary>
        /// 激活该分区.
        /// </summary>
        public void Activate()
        {
            IsRequested = _offsetId != 0 || _pageNumber > 1;
            Controller.SubPartitionVideoIteration += OnSubPartitionVideoIteration;
            Controller.SubPartitionAdditionalDataChanged += OnSubPartitionAdditionalDataChanged;
            if (_isRecommendPartition)
            {
                SubPartitionId = PartitionModuleViewModel.Instance.CurrentPartition.PartitionId;
            }
        }

        /// <summary>
        /// 停用该分区.
        /// </summary>
        public void Deactive()
        {
            Controller.SubPartitionVideoIteration -= OnSubPartitionVideoIteration;
            Controller.SubPartitionAdditionalDataChanged -= OnSubPartitionAdditionalDataChanged;
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

            IsRequested = _offsetId != 0 || _pageNumber > 1;
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
                _offsetId = 0;
                _pageNumber = 1;
                IsError = false;
                ErrorText = string.Empty;
                _lastRequestTime = DateTimeOffset.MinValue;
                try
                {
                    await Controller.RequestSubPartitionDataAsync(SubPartitionId, _isRecommendPartition, 0, CurrentSortType, _pageNumber);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestSubPartitionFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }

            IsRequested = _offsetId != 0 || _pageNumber > 1;
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
                await Controller.RequestSubPartitionDataAsync(SubPartitionId, _isRecommendPartition, _offsetId, CurrentSortType, _pageNumber);
                IsDeltaLoading = false;
            }
        }

        private void GenerateSortType()
        {
            SortTypeCollection = new ObservableCollection<VideoSortType>()
            {
                VideoSortType.Default,
                VideoSortType.Newest,
                VideoSortType.Play,
                VideoSortType.Reply,
                VideoSortType.Danmaku,
                VideoSortType.Favorite,
            };
        }

        private async void OnPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentSortType))
            {
                await InitializeRequestAsync();
            }
        }

        private void OnSubPartitionAdditionalDataChanged(object sender, PartitionAdditionalDataChangedEventArgs e)
        {
            if (e.SubPartitionId == SubPartitionId)
            {
                IsShowBanner = e.BannerList?.Any() ?? false;
                IsShowTags = e.TagList?.Any() ?? false;
                if (IsShowBanner)
                {
                    BannerCollection.Clear();
                    e.BannerList.ToList().ForEach(p => BannerCollection.Add(new BannerViewModel(p)));
                }

                if (IsShowTags)
                {
                    TagCollection.Clear();
                    e.TagList.ToList().ForEach(p => TagCollection.Add(p));
                }
            }
        }

        private void OnSubPartitionVideoIteration(object sender, PartitionVideoIterationEventArgs e)
        {
            if (e.SubPartitionId == SubPartitionId)
            {
                _pageNumber = e.NextPageNumber;

                if (e.RequestDateTime > _lastRequestTime)
                {
                    _lastRequestTime = e.RequestDateTime;
                    _offsetId = e.BottomOffsetId;
                }

                if (e.VideoList?.Any() ?? false)
                {
                    e.VideoList.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
                }
            }
        }
    }
}
