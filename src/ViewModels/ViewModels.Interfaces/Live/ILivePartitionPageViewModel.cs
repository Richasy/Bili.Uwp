// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Community;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Live
{
    /// <summary>
    /// 直播分区页面视图模型的接口定义.
    /// </summary>
    public interface ILivePartitionPageViewModel : INotifyPropertyChanged, IInitializeViewModel, IReloadViewModel
    {
        /// <summary>
        /// 父分区集合.
        /// </summary>
        public ObservableCollection<Partition> ParentPartitions { get; }

        /// <summary>
        /// 显示的分区集合.
        /// </summary>
        public ObservableCollection<Partition> DisplayPartitions { get; }

        /// <summary>
        /// 选择分区命令.
        /// </summary>
        public IRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前选中的父分区.
        /// </summary>
        public Partition CurrentParentPartition { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        public string ErrorText { get; set; }

        /// <summary>
        /// 请求过程中出现了问题.
        /// </summary>
        public bool IsError { get; set; }
    }
}
