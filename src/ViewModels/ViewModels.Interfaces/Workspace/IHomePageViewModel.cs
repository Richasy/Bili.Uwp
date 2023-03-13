// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Community;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Workspace
{
    /// <summary>
    /// 首页视图模型的接口定义.
    /// </summary>
    public interface IHomePageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 固定的视频分区集合.
        /// </summary>
        ObservableCollection<Partition> FixedVideoPartitions { get; }

        /// <summary>
        /// 视频分区集合.
        /// </summary>
        ObservableCollection<IVideoPartitionViewModel> VideoPartitions { get; }

        /// <summary>
        /// 分区是否正在加载.
        /// </summary>
        bool IsVideoPartitionLoading { get; }

        /// <summary>
        /// 初始化分区的命令.
        /// </summary>
        IAsyncRelayCommand InitializeVideoPartitionsCommand { get; }
    }
}
