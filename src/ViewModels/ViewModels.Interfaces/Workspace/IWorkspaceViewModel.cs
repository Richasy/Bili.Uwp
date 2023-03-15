// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Community;
using CommunityToolkit.Mvvm.Input;
using Models.Workspace;

namespace Bili.ViewModels.Interfaces.Workspace
{
    /// <summary>
    /// 工坊应用视图模型的接口定义.
    /// </summary>
    public interface IWorkspaceViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 请求导航.
        /// </summary>
        event EventHandler RequestNavigating;

        /// <summary>
        /// 导航条目集合.
        /// </summary>
        ObservableCollection<NavigateItem> Items { get; }

        /// <summary>
        /// 当前条目.
        /// </summary>
        NavigateItem CurrentItem { get; set; }

        /// <summary>
        /// 设置页面是否打开.
        /// </summary>
        bool IsSettingsOpen { get; set; }

        /// <summary>
        /// Xaml根元素.
        /// </summary>
        object XamlRoot { get; set; }

        /// <summary>
        /// 设置按钮命令.
        /// </summary>
        IRelayCommand ShowSettingsCommand { get; }

        /// <summary>
        /// 导航到指定的分区.
        /// </summary>
        /// <param name="partition">分区信息.</param>
        void NavigateToPartition(Partition partition);

        /// <summary>
        /// 获取选中的分区.
        /// </summary>
        /// <returns>分区信息.</returns>
        Partition GetSelectedPartition();
    }
}
