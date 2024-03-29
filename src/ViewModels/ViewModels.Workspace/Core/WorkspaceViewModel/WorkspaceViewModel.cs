﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Workspace;
using CommunityToolkit.Mvvm.Input;
using Models.Workspace;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 工坊视图模型.
    /// </summary>
    public sealed partial class WorkspaceViewModel : ViewModelBase, IWorkspaceViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
        /// </summary>
        public WorkspaceViewModel(IResourceToolkit resourceToolkit)
        {
            _resourceToolkit = resourceToolkit;
            Items = new ObservableCollection<NavigateItem>();
            InitializeItems();
            CurrentItem = Items.First();
        }

        /// <inheritdoc/>
        public void NavigateToPartition(Partition partition)
        {
            _selectedPartition = partition;
            var newNavItem = new NavigateItem(NavigateTarget.Partition, partition.Name, FluentSymbol.Apps);
            Items.Add(newNavItem);
            CurrentItem = newNavItem;
        }

        /// <inheritdoc/>
        public Partition GetSelectedPartition()
            => _selectedPartition;

        private void InitializeItems()
        {
            var homeItem = new NavigateItem(NavigateTarget.Home, _resourceToolkit.GetLocaleString(LanguageNames.Home), FluentSymbol.Home);
            var hotItem = new NavigateItem(NavigateTarget.Hot, _resourceToolkit.GetLocaleString(LanguageNames.Popular), FluentSymbol.Rocket);
            var dynamicItem = new NavigateItem(NavigateTarget.Dynamic, _resourceToolkit.GetLocaleString(LanguageNames.DynamicFeed), FluentSymbol.DesignIdeas);
            var rankItem = new NavigateItem(NavigateTarget.Rank, _resourceToolkit.GetLocaleString(LanguageNames.Rank), FluentSymbol.Trophy);
            var historyItem = new NavigateItem(NavigateTarget.History, _resourceToolkit.GetLocaleString(LanguageNames.ViewHistory), FluentSymbol.History);
            var rcmdItem = new NavigateItem(NavigateTarget.Recommend, _resourceToolkit.GetLocaleString(LanguageNames.Recommend), FluentSymbol.DataSunburst);
            Items.Add(homeItem);
            Items.Add(dynamicItem);
            Items.Add(rcmdItem);
            Items.Add(rankItem);
            Items.Add(hotItem);
            Items.Add(historyItem);
        }

        [RelayCommand]
        private void ShowSettings()
        {
            if (IsSettingsOpen)
            {
                return;
            }

            IsSettingsOpen = true;
            CurrentItem = null;
            RequestNavigating?.Invoke(this, EventArgs.Empty);
        }

        partial void OnCurrentItemChanged(NavigateItem value)
        {
            if (value == null)
            {
                return;
            }

            IsSettingsOpen = false;
            if (value.Target != NavigateTarget.Partition && Items.Last().Target == NavigateTarget.Partition)
            {
                Items.RemoveAt(Items.Count - 1);
                _selectedPartition = null;
            }

            RequestNavigating?.Invoke(this, EventArgs.Empty);
        }
    }
}
