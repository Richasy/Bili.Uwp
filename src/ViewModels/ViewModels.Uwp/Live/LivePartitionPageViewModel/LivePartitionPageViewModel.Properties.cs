// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

        /// <inheritdoc/>
        public ObservableCollection<Partition> ParentPartitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> DisplayPartitions { get; }

        /// <inheritdoc/>
        public IRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public Partition CurrentParentPartition { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}
