// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Home;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 排行榜页面视图模型.
    /// </summary>
    public sealed partial class RankPageViewModel : ViewModelBase, IRankPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankPageViewModel"/> class.
        /// </summary>
        /// <param name="resourceToolkit">本地资源工具.</param>
        /// <param name="homeProvider">分区服务提供工具.</param>
        /// <param name="dispatcher">UI 调度器.</param>
        public RankPageViewModel(
            IResourceToolkit resourceToolkit,
            IHomeProvider homeProvider,
            CoreDispatcher dispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
            _dispatcher = dispatcher;
            _caches = new Dictionary<Partition, IEnumerable<VideoInformation>>();

            Videos = new ObservableCollection<IVideoItemViewModel>();
            Partitions = new ObservableCollection<Partition>();

            var canReload = this.WhenAnyValue(x => x.IsReloading).Select(p => !p);

            InitializeCommand = ReactiveCommand.CreateFromTask(InitializeAsync);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync, canReload, RxApp.MainThreadScheduler);
            SelectPartitionCommand = ReactiveCommand.CreateFromTask<Partition>(SelectPartitionAsync);

            InitializeCommand.ThrownExceptions
                .Merge(ReloadCommand.ThrownExceptions)
                .Merge(SelectPartitionCommand.ThrownExceptions)
                .Subscribe(DisplayException);

            InitializeCommand.IsExecuting
                .Merge(ReloadCommand.IsExecuting)
                .Merge(SelectPartitionCommand.IsExecuting)
                .ToPropertyEx(this, x => x.IsReloading);
        }

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.GetMessage()
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RankRequestFailed)}\n{msg}";
            LogException(exception);
        }

        private async Task InitializeAsync()
        {
            if (Partitions.Count > 0)
            {
                await FakeLoadingAsync();
                return;
            }

            var task = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await ReloadAsync()).AsTask();
            await RunDelayTask(task);
        }

        private async Task ReloadAsync()
        {
            TryClear(Partitions);
            TryClear(Videos);
            _caches.Clear();
            var partitions = (await _homeProvider.GetVideoPartitionIndexAsync()).ToList();
            var allItem = new Partition("0", _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.WholePartitions), new Image("ms-appx:///Assets/Bili_rgba_80.png"));
            partitions.Insert(0, allItem);
            partitions.ForEach(p => Partitions.Add(p));
            await SelectPartitionAsync(allItem);
        }

        private async Task SelectPartitionAsync(Partition partition)
        {
            await Task.Delay(100);
            CurrentPartition = partition;
            TryClear(Videos);
            var videos = _caches.ContainsKey(partition)
                ? _caches[partition]
                : await _homeProvider.GetRankDetailAsync(partition.Id);

            if (videos != null)
            {
                if (!_caches.ContainsKey(partition))
                {
                    _caches.Add(partition, videos);
                }

                foreach (var item in videos)
                {
                    var videoVM = Splat.Locator.Current.GetService<IVideoItemViewModel>();
                    videoVM.InjectData(item);
                    Videos.Add(videoVM);
                }
            }
        }
    }
}
