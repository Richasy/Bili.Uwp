// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Desktop.Home
{
    /// <summary>
    /// 排行榜页面视图模型.
    /// </summary>
    public sealed partial class RankPageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IHomeProvider _homeProvider;
        private readonly DispatcherQueue _dispatcherQueue;
        private readonly Dictionary<Partition, IEnumerable<VideoInformation>> _caches;

        [ObservableProperty]
        private Partition _currentPartition;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> Partitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> Videos { get; }
    }
}
