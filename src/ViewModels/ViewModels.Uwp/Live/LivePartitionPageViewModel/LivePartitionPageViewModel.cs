// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Live;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播分区页面视图模型.
    /// </summary>
    public sealed partial class LivePartitionPageViewModel : ViewModelBase, ILivePartitionPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankPageViewModel"/> class.
        /// </summary>
        public LivePartitionPageViewModel(
            IResourceToolkit resourceToolkit,
            ILiveProvider liveProvider)
        {
            _resourceToolkit = resourceToolkit;
            _liveProvider = liveProvider;
            _dispatcher = Window.Current.CoreWindow.Dispatcher;

            ParentPartitions = new ObservableCollection<Partition>();
            DisplayPartitions = new ObservableCollection<Partition>();

            InitializeCommand = new AsyncRelayCommand(InitializeAsync);
            ReloadCommand = new AsyncRelayCommand(ReloadAsync);
            SelectPartitionCommand = new AsyncRelayCommand<Partition>(SelectPartitionAsync);

            AttachExceptionHandlerToAsyncCommand(
                DisplayException,
                InitializeCommand,
                ReloadCommand,
                SelectPartitionCommand);

            AttachIsRunningToAsyncCommand(
                p => IsReloading = p,
                InitializeCommand,
                ReloadCommand,
                SelectPartitionCommand);
        }

        private async Task InitializeAsync()
        {
            if (ParentPartitions.Count > 0)
            {
                await FakeLoadingAsync();
                return;
            }

            var task = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await ReloadAsync()).AsTask();
            await RunDelayTask(task);
        }

        private async Task ReloadAsync()
        {
            TryClear(ParentPartitions);
            TryClear(DisplayPartitions);
            CurrentParentPartition = null;
            var partitions = (await _liveProvider.GetLiveAreaIndexAsync()).ToList();
            partitions.ForEach(p => ParentPartitions.Add(p));
            await SelectPartitionAsync(partitions.First());
        }

        private async Task SelectPartitionAsync(Partition partition)
        {
            await Task.Delay(100);
            CurrentParentPartition = partition;
            TryClear(DisplayPartitions);
            partition.Children.ToList().ForEach(p => DisplayPartitions.Add(p));
        }

        private void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.GetMessage()
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestLiveTagsFailed)}\n{msg}";
            LogException(exception);
        }
    }
}
