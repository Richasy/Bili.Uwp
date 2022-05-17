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

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 分区页面视图模型.
    /// </summary>
    public sealed partial class PartitionPageViewModel : ViewModelBase, IInitializeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionPageViewModel"/> class.
        /// </summary>
        /// <param name="partitionProvider">分区服务提供工具.</param>
        public PartitionPageViewModel(IPartitionProvider partitionProvider)
        {
            _partitionProvider = partitionProvider;
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
            Partitions.Clear();
            var items = await _partitionProvider.GetPartitionIndexAsync();
            items.ToList().ForEach(p => Partitions.Add(p));
        }
    }
}
