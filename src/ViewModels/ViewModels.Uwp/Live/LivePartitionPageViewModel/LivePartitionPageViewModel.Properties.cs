// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播分区页面视图模型.
    /// </summary>
    public sealed partial class LivePartitionPageViewModel
    {
        private readonly ILiveProvider _liveProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly CoreDispatcher _dispatcher;

        [ObservableProperty]
        private Partition _currentParentPartition;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public ObservableCollection<Partition> ParentPartitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> DisplayPartitions { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<Partition> SelectPartitionCommand { get; }
    }
}
