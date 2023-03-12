// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Models.Workspace;

namespace Bili.ViewModels.Interfaces.Core
{
    /// <summary>
    /// 工坊应用视图模型的接口定义.
    /// </summary>
    public interface IWorkspaceViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 导航条目集合.
        /// </summary>
        ObservableCollection<NavigateItem> Items { get; }

        /// <summary>
        /// 当前条目.
        /// </summary>
        NavigateItem CurrentItem { get; set; }
    }
}
