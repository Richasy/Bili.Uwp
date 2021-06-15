// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 子分区视图模型.
    /// </summary>
    public partial class SubPartitionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubPartitionViewModel"/> class.
        /// </summary>
        /// <param name="partition">子分区数据.</param>
        public SubPartitionViewModel(Partition partition)
        {
            _controller = Controller.Uwp.BiliController.Instance;
            ServiceLocator.Instance.LoadService(out _resourceToolkit);
            BannerCollection = new ObservableCollection<Banner>();
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
                this.Title = this._resourceToolkit.GetLocaleString(LanguageNames.Recommend);
                this._isRecommendPartition = true;
            }

            this.PropertyChanged += OnPropertyChangedAsync;
        }

        /// <summary>
        /// 激活该分区.
        /// </summary>
        public void Activate()
        {
            _controller.SubPartitionVideoIteration += OnSubPartitionVideoIteration;
            _controller.SubPartitionAdditionalDataChanged += OnSubPartitionAdditionalDataChanged;
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
            _controller.SubPartitionVideoIteration -= OnSubPartitionVideoIteration;
            _controller.SubPartitionAdditionalDataChanged -= OnSubPartitionAdditionalDataChanged;
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
                _offsetId = 0;
                _pageNumber = 1;
                _lastRequestTime = DateTimeOffset.MinValue;
                await _controller.RequestSubPartitionDataAsync(SubPartitionId, _isRecommendPartition, 0, CurrentSortType, _pageNumber);
                IsInitializeLoading = false;
            }
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
                await _controller.RequestSubPartitionDataAsync(SubPartitionId, _isRecommendPartition, _offsetId, CurrentSortType, _pageNumber);
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
                    e.BannerList.ToList().ForEach(p => BannerCollection.Add(p));
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
