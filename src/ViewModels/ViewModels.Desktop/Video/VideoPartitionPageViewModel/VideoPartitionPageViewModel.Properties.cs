// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Video
{
    /// <summary>
    /// 分区页面的视图模型.
    /// </summary>
    public sealed partial class VideoPartitionPageViewModel
    {
        private readonly IHomeProvider _homeProvider;

        [ObservableProperty]
        private bool _isInitializing;

        /// <inheritdoc/>
        public ObservableCollection<Partition> Partitions { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }
    }
}
