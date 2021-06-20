// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 排行榜视图模型.
    /// </summary>
    public partial class RankViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankViewModel"/> class.
        /// </summary>
        public RankViewModel()
        {
            _controller = Controller.Uwp.BiliController.Instance;
            _cachedRankData = new Dictionary<RankPartition, List<VideoViewModel>>();
            DisplayVideoCollection = new ObservableCollection<VideoViewModel>();
            PartitionCollection = new ObservableCollection<RankPartition>();
            ServiceLocator.Instance.LoadService(out _resourceToolkit);
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeAsync()
        {
            if (CurrentPartition == null)
            {
                IsInitializeLoading = true;
                try
                {
                    var originalPartitions = await _controller.RequestPartitionIndexAsync();
                    var rankPartitions = originalPartitions.Select(p => new RankPartition(p)).ToList();
                    rankPartitions.Insert(
                        0,
                        new RankPartition(
                            _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.WholePartitions),
                            "ms-appx:///Assets/Bili_rgba_80.png"));

                    rankPartitions.ForEach(p => PartitionCollection.Add(p));

                    IsInitializeLoading = false;
                    await SetSelectedRankPartitionAsync(PartitionCollection.First());
                }
                catch (System.Exception)
                {
                    // ignore.
                }
            }
        }

        /// <summary>
        /// 设置选中的排行榜分区.
        /// </summary>
        /// <param name="partition">排行榜分区.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SetSelectedRankPartitionAsync(RankPartition partition)
        {
            if (partition != null && partition != CurrentPartition)
            {
                IsRankLoading = true;
                CurrentPartition = partition;
                DisplayVideoCollection.Clear();
                List<VideoViewModel> videoList = null;
                if (_cachedRankData.ContainsKey(partition))
                {
                    videoList = _cachedRankData[partition];
                }
                else
                {
                    var rankList = await _controller.GetRankAsync(partition.PartitionId);
                    if (rankList?.Any() ?? false)
                    {
                        videoList = rankList.Select(p => new VideoViewModel(p)).ToList();
                        _cachedRankData.Add(partition, videoList);
                    }
                }

                if (videoList != null)
                {
                    videoList.ForEach(p => DisplayVideoCollection.Add(p));
                }

                IsRankLoading = false;
            }
        }
    }
}
