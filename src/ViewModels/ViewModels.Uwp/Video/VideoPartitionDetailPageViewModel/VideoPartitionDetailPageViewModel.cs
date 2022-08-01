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
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 分区详情页视图模型.
    /// </summary>
    public sealed partial class VideoPartitionDetailPageViewModel : InformationFlowViewModelBase<IVideoItemViewModel>, IVideoPartitionDetailPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartitionDetailPageViewModel"/> class.
        /// </summary>
        public VideoPartitionDetailPageViewModel(
            IResourceToolkit resourceToolkit,
            IHomeProvider homeProvider,
            CoreDispatcher coreDispatcher)
            : base(coreDispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
            _caches = new Dictionary<Partition, IEnumerable<VideoInformation>>();

            Banners = new ObservableCollection<IBannerViewModel>();
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

            isRecommend.Merge(this.WhenAnyValue(x => x.Banners.Count, count => count > 0))
                .ToPropertyEx(this, x => x.IsShowBanner);

            isRecommend.ToPropertyEx(this, x => x.IsRecommendPartition);

            SelectPartitionCommand = ReactiveCommand.Create<Partition>(SelectSubPartition);
        }

        /// <summary>
        /// 设置初始分区.
        /// </summary>
        /// <param name="partition">父分区信息.</param>
        public void SetPartition(Partition partition)
        {
            OriginPartition = partition;
            _caches.Clear();
            _homeProvider.ClearPartitionState();
            TryClear(SubPartitions);
            partition.Children.ToList().ForEach(p => SubPartitions.Add(p));
            CurrentSubPartition = SubPartitions.First();
            TryClear(Banners);
            TryClear(Items);
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
            => _homeProvider.ResetSubPartitionState();

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestSubPartitionFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var partition = CurrentSubPartition;
            var isRecommend = partition.Id == OriginPartition.Id;
            var data = await _homeProvider.GetVideoSubPartitionDataAsync(partition.Id, isRecommend, SortType);
            if (data.Banners?.Count() > 0)
            {
                foreach (var item in data.Banners)
                {
                    if (!Banners.Any(p => p.Uri == item.Uri))
                    {
                        var vm = Locator.Current.GetService<IBannerViewModel>();
                        vm.InjectData(item);
                        Banners.Add(vm);
                    }
                }
            }

            if (data.Videos?.Count() > 0)
            {
                foreach (var video in data.Videos)
                {
                    var videoVM = Locator.Current.GetService<IVideoItemViewModel>();
                    videoVM.InjectData(video);
                    Items.Add(videoVM);
                }

                var videos = Items
                        .OfType<IVideoItemViewModel>()
                        .Select(p => p.Data)
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
            TryClear(Items);
            CurrentSubPartition = subPartition;
            if (_caches.ContainsKey(subPartition))
            {
                var data = _caches[subPartition];
                foreach (var video in data)
                {
                    var videoVM = Splat.Locator.Current.GetService<IVideoItemViewModel>();
                    videoVM.InjectData(video);
                    Items.Add(videoVM);
                }
            }
            else
            {
                InitializeCommand.Execute().Subscribe();
            }
        }
    }
}
