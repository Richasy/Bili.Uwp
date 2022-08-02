// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Data.Community;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 视频分区页视图模型的接口定义.
    /// </summary>
    public interface IVideoPartitionPageViewModel : IReactiveObject, IInitializeViewModel
    {
        /// <summary>
        /// 分区集合.
        /// </summary>
        ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 是否正在初始化.
        /// </summary>
        bool IsInitializing { get; }
    }
}
