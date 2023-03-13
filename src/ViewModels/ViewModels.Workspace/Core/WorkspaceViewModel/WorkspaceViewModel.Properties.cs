// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Workspace;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 工坊视图模型.
    /// </summary>
    public sealed partial class WorkspaceViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;

        [ObservableProperty]
        private NavigateItem _currentItem;

        /// <inheritdoc />
        public ObservableCollection<NavigateItem> Items { get; }
    }
}
