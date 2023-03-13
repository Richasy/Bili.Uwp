// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Workspace.Pages
{
    /// <summary>
    /// 首页视图模型.
    /// </summary>
    public sealed partial class HomePageViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IHomeProvider _homeProvider;

        [ObservableProperty]
        private bool _isVideoPartitionLoading;

        /// <inheritdoc/>
        public ObservableCollection<Partition> FixedVideoPartitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoPartitionViewModel> VideoPartitions { get; }
    }
}
