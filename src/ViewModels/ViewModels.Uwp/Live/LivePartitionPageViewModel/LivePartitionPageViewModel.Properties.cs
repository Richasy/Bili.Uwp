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
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public Partition CurrentParentPartition { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}
