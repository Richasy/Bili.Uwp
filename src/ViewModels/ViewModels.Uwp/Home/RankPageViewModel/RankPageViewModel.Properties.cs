// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 排行榜页面视图模型.
    /// </summary>
    public sealed partial class RankPageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IHomeProvider _homeProvider;
        private readonly CoreDispatcher _dispatcher;
        private readonly Dictionary<Partition, IEnumerable<VideoInformation>> _caches;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public Partition CurrentPartition { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> Partitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> Videos { get; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}
