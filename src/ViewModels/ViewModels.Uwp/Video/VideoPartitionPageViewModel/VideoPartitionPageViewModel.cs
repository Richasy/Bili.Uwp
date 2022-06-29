// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 分区页面视图模型.
    /// </summary>
    public sealed partial class VideoPartitionPageViewModel : ViewModelBase, IInitializeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartitionPageViewModel"/> class.
        /// </summary>
        /// <param name="homeProvider">分区服务提供工具.</param>
        public VideoPartitionPageViewModel(IHomeProvider homeProvider)
        {
            _homeProvider = homeProvider;
            Partitions = new ObservableCollection<Partition>();

            var canInitialize = this.WhenAnyValue(
                x => x.Partitions.Count,
                count => count == 0);

            InitializeCommand = ReactiveCommand.CreateFromTask(
                InitializeAsync,
                canInitialize,
                outputScheduler: RxApp.MainThreadScheduler);

            InitializeCommand.ThrownExceptions.Subscribe(ex =>
            {
                this.Log().Debug(ex);
            });

            _isInitializing = InitializeCommand.IsExecuting.ToProperty(
                this,
                x => x.IsInitializing,
                scheduler: RxApp.MainThreadScheduler);
        }

        private async Task InitializeAsync()
        {
            TryClear(Partitions);
            var items = await _homeProvider.GetVideoPartitionIndexAsync();
            items.ToList().ForEach(p => Partitions.Add(p));
        }
    }
}
