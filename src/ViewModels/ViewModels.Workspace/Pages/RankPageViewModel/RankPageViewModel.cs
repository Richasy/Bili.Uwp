// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Home;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Workspace.Pages
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
        public RankPageViewModel(
            IResourceToolkit resourceToolkit,
            IHomeProvider homeProvider)
        {
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
            _dispatcher = DispatcherQueue.GetForCurrentThread();

            Videos = new ObservableCollection<IVideoItemViewModel>();
            Partitions = new ObservableCollection<Partition>();

            InitializeCommand = new AsyncRelayCommand(InitializeAsync);
            ReloadCommand = new AsyncRelayCommand(ReloadAsync, () => !IsReloading);

            AttachExceptionHandlerToAsyncCommand(
                DisplayException,
                InitializeCommand,
                ReloadCommand);
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
            if (Videos.Count > 0)
            {
                return;
            }

            _dispatcher.TryEnqueue(async () => await ReloadAsync());
            await Task.CompletedTask;
        }

        private async Task ReloadAsync()
        {
            IsReloading = true;
            TryClear(Videos);
            var allItem = new Partition("0", string.Empty);
            await SelectPartitionAsync(allItem);
            IsReloading = false;
        }

        private async Task SelectPartitionAsync(Partition partition)
        {
            TryClear(Videos);
            var videos = await _homeProvider.GetRankDetailAsync(partition.Id);

            if (videos != null)
            {
                foreach (var item in videos)
                {
                    var videoVM = Locator.Instance.GetService<IVideoItemViewModel>();
                    item.Publisher = default;
                    videoVM.InjectData(item);
                    Videos.Add(videoVM);
                }
            }
        }
    }
}
