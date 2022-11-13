// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Video
{
    /// <summary>
    /// 分区页面视图模型.
    /// </summary>
    public sealed partial class VideoPartitionPageViewModel : ViewModelBase, IVideoPartitionPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartitionPageViewModel"/> class.
        /// </summary>
        /// <param name="homeProvider">分区服务提供工具.</param>
        public VideoPartitionPageViewModel(IHomeProvider homeProvider)
        {
            _homeProvider = homeProvider;
            Partitions = new ObservableCollection<Partition>();

            InitializeCommand = new AsyncRelayCommand(
                InitializeAsync,
                () => Partitions.Count == 0);

            AttachExceptionHandlerToAsyncCommand(LogException, InitializeCommand);
            AttachIsRunningToAsyncCommand(p => IsInitializing = p, InitializeCommand);
        }

        private async Task InitializeAsync()
        {
            TryClear(Partitions);
            var items = await _homeProvider.GetVideoPartitionIndexAsync();
            items.ToList().ForEach(p => Partitions.Add(p));
        }
    }
}
