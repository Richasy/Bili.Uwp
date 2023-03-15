// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Workspace.Core;

namespace Bili.ViewModels.Workspace.Pages
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
            IHomeProvider homeProvider)
        {
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
        }

        /// <summary>
        /// 设置初始分区.
        /// </summary>
        /// <param name="partition">父分区信息.</param>
        public void SetPartition(Partition partition)
        {
            OriginPartition = partition;
            _homeProvider.ClearPartitionState();
            CurrentSubPartition = partition.Children.First();
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

            if (data.Videos?.Count() > 0)
            {
                foreach (var video in data.Videos)
                {
                    var videoVM = Locator.Instance.GetService<IVideoItemViewModel>();
                    videoVM.InjectData(video);
                    Items.Add(videoVM);
                }
            }
        }

        partial void OnCurrentSubPartitionChanged(Partition value)
        {
            IsRecommendPartition = value?.Id == OriginPartition?.Id;
        }
    }
}
