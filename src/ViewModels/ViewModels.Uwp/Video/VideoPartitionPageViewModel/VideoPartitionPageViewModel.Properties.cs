// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 分区页面的视图模型.
    /// </summary>
    public sealed partial class VideoPartitionPageViewModel
    {
        private readonly IHomeProvider _homeProvider;

        /// <inheritdoc/>
        public ObservableCollection<Partition> Partitions { get; }

        /// <inheritdoc/>
        public IRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsInitializing { get; set; }
    }
}
