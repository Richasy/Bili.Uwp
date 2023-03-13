// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.Models.Data.Community;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Workspace
{
    /// <summary>
    /// 需要被固定的视图模型的接口定义.
    /// </summary>
    /// <typeparam name="T">分区数据类型.</typeparam>
    public interface IFixedItemViewModel<T> : INotifyPropertyChanged
    {
        /// <summary>
        /// 数据条目.
        /// </summary>
        T Data { get; set; }

        /// <summary>
        /// 是否已经被固定.
        /// </summary>
        bool IsFixed { get; set; }

        /// <summary>
        /// 切换固定状态的命令.
        /// </summary>
        IRelayCommand ToggleCommand { get; }

        /// <summary>
        /// 注入动作.
        /// </summary>
        /// <param name="action">动作.</param>
        void InjectAction(Action<bool, IFixedItemViewModel<T>> action);
    }

    /// <summary>
    /// 视频分区固定视图模型的接口定义.
    /// </summary>
    public interface IVideoPartitionViewModel : IFixedItemViewModel<Partition>
    {
    }
}
