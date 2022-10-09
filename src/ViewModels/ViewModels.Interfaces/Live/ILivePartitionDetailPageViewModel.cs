// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Data.Community;
using Bili.Models.Data.Live;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Live
{
    /// <summary>
    /// 直播分区详情页面视图模型的接口定义.
    /// </summary>
    public interface ILivePartitionDetailPageViewModel : IInformationFlowViewModel<ILiveItemViewModel>
    {
        /// <summary>
        /// 分区标签集合.
        /// </summary>
        public ObservableCollection<LiveTag> Tags { get; }

        /// <summary>
        /// 选中标签命令.
        /// </summary>
        public IAsyncRelayCommand<LiveTag> SelectTagCommand { get; }

        /// <summary>
        /// 查看全部分区命令.
        /// </summary>
        public IRelayCommand SeeAllPartitionsCommand { get; }

        /// <summary>
        /// 始分区.
        /// </summary>
        public Partition OriginPartition { get; }

        /// <summary>
        /// 当前选中的标签.
        /// </summary>
        public LiveTag CurrentTag { get; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        public bool IsEmpty { get; }

        /// <summary>
        /// 设置初始分区.
        /// </summary>
        /// <param name="partition">分区信息.</param>
        void SetPartition(Partition partition);
    }
}
