// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.ViewModels.Interfaces.Toolbox;

namespace Bili.ViewModels.Interfaces.Home
{
    /// <summary>
    /// 工具页面视图模型的接口定义.
    /// </summary>
    public interface IToolboxPageViewModel
    {
        /// <summary>
        /// 工具集合.
        /// </summary>
        ObservableCollection<IToolboxItemViewModel> ToolCollection { get; }
    }
}
