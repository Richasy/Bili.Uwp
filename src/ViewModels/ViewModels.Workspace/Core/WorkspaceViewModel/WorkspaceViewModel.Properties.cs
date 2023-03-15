// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
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

        [ObservableProperty]
        private bool _isSettingsOpen;

        private Partition _selectedPartition;

        /// <inheritdoc/>
        public event EventHandler RequestNavigating;

        /// <inheritdoc/>
        public object XamlRoot { get; set; }

        /// <inheritdoc />
        public ObservableCollection<NavigateItem> Items { get; }
    }
}
