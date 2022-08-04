// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.ViewModels.Interfaces.Toolbox;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 工具箱页面视图模型.
    /// </summary>
    public sealed partial class ToolboxPageViewModel
    {
        /// <summary>
        /// 工具集合.
        /// </summary>
        public ObservableCollection<IToolboxItemViewModel> ToolCollection { get; }
    }
}
