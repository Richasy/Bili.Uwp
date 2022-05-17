// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 分区详情页视图模型.
    /// </summary>
    public sealed partial class PartitionDetailPageViewModel : InformationFlowViewModelBase, IBackPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionDetailPageViewModel"/> class.
        /// </summary>
        /// <param name="resourceToolkit">本地资源工具.</param>
        /// <param name="partitionProvider">分区服务提供工具.</param>
        public PartitionDetailPageViewModel(
            IResourceToolkit resourceToolkit,
            IPartitionProvider partitionProvider)
            : base(resourceToolkit)
        {
            _partitionProvider = partitionProvider;
            _caches = new Dictionary<Partition, IEnumerable<VideoInformation>>();

            Banners = new ObservableCollection<BannerViewModel>();
            SubPartitions = new ObservableCollection<Partition>();
            SortTypes = new ObservableCollection<VideoSortType>()
            {
                VideoSortType.Default,
                VideoSortType.Newest,
                VideoSortType.Play,
                VideoSortType.Reply,
                VideoSortType.Danmaku,
                VideoSortType.Favorite,
            };

            var isRecommend = this.WhenAnyValue(
                x => x.CurrentSubPartition,
                partition => partition?.Id == OriginPartition?.Id);

            _isShowBanner = isRecommend.Merge(this.WhenAnyValue(x => x.Banners.Count, count => count > 0))
                .ToProperty(this, x => x.IsShowBanner, scheduler: RxApp.MainThreadScheduler);

            _isRecommendPartition = isRecommend
                .ToProperty(this, x => x.IsRecommendPartition, scheduler: RxApp.MainThreadScheduler);

            SelectPartitionCommand = ReactiveCommand.Create<Partition>(SelectSubPartition, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <inheritdoc/>
        public object GetBackParameter()
            => OriginPartition;

        /// <summary>
        /// 设置初始分区.
        /// </summary>
        /// <param name="partition">父分区信息.</param>
        public void SetPartition(Partition partition)
        {
            OriginPartition = partition;
            _caches.Clear();
            _partitionProvider.Clear();
            SubPartitions.Clear();
            partition.Children.ToList().ForEach(p => SubPartitions.Add(p));
            CurrentSubPartition = SubPartitions.First();
            Banners.Clear();
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
            => _partitionProvider.Reset();

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var partition = CurrentSubPartition;
            var isRecommend = partition.Id == OriginPartition.Id;
            var data = await _partitionProvider.GetSubPartitionDataAsync(partition.Id, isRecommend);
            if (data.Banners?.Count() > 0)
            {
                foreach (var item in data.Banners)
                {
                    if (!Banners.Any(p => p.Uri == item.Uri))
                    {
                        Banners.Add(new BannerViewModel(item));
                    }
                }
            }

            if (data.Videos?.Count() > 0)
            {
                foreach (var video in data.Videos)
                {
                    var videoVM = Splat.Locator.Current.GetService<VideoItemViewModel>();
                    videoVM.SetInformation(video);
                    VideoCollection.Add(videoVM);
                }

                var videos = VideoCollection
                        .OfType<VideoItemViewModel>()
                        .Select(p => p.Information)
                        .ToList();
                if (_caches.ContainsKey(CurrentSubPartition))
                {
                    _caches[CurrentSubPartition] = videos;
                }
                else
                {
                    _caches.Add(CurrentSubPartition, videos);
                }
            }
        }

        private void SelectSubPartition(Partition subPartition)
        {
            VideoCollection.Clear();
            CurrentSubPartition = subPartition;
            if (_caches.ContainsKey(subPartition))
            {
                var data = _caches[subPartition];
                foreach (var video in data)
                {
                    var videoVM = Splat.Locator.Current.GetService<VideoItemViewModel>();
                    videoVM.SetInformation(video);
                    VideoCollection.Add(videoVM);
                }
            }
            else
            {
                InitializeCommand.Execute().Subscribe();
            }
        }
    }
}
