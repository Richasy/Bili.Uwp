// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Common;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 视频分区详情页面视图模型的接口定义.
    /// </summary>
    public interface IVideoPartitionDetailPageViewModel : IInformationFlowViewModel<IVideoItemViewModel>
    {
        /// <summary>
        /// 横幅集合.
        /// </summary>
        ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 子分区集合.
        /// </summary>
        ObservableCollection<Partition> SubPartitions { get; }

        /// <summary>
        /// 视频排序方式集合.
        /// </summary>
        ObservableCollection<VideoSortType> SortTypes { get; }

        /// <summary>
        /// 选中子分区命令.
        /// </summary>
        ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <summary>
        /// 父分区.
        /// </summary>
        Partition OriginPartition { get; }

        /// <summary>
        /// 当前选中的子分区.
        /// </summary>
        Partition CurrentSubPartition { get; set; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        VideoSortType SortType { get; set; }

        /// <summary>
        /// 是否为推荐子分区.
        /// </summary>
        bool IsRecommendPartition { get; }

        /// <summary>
        /// 是否显示横幅内容.
        /// </summary>
        bool IsShowBanner { get; }

        /// <summary>
        /// 设置初始分区.
        /// </summary>
        /// <param name="partition">父分区信息.</param>
        void SetPartition(Partition partition);
    }
}
