// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Home
{
    /// <summary>
    /// 排行榜页面视图模型的接口定义.
    /// </summary>
    public interface IRankPageViewModel : INotifyPropertyChanged, IInitializeViewModel, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 选择分区命令.
        /// </summary>
        IRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前的分区.
        /// </summary>
        Partition CurrentPartition { get; }

        /// <summary>
        /// 全部分区.
        /// </summary>
        ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 当前展示的视频集合.
        /// </summary>
        ObservableCollection<IVideoItemViewModel> Videos { get; }
    }
}
