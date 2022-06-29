// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放列表视图模型.
    /// </summary>
    public sealed partial class PgcPlaylistViewModel : ViewModelBase, IInitializeViewModel, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlaylistViewModel"/> class.
        /// </summary>
        /// <param name="appViewModel">应用视图模型.</param>
        /// <param name="pgcProvider">PGC服务提供工具.</param>
        /// <param name="resourceToolkit">本地资源管理工具.</param>
        public PgcPlaylistViewModel(
            AppViewModel appViewModel,
            IPgcProvider pgcProvider,
            IResourceToolkit resourceToolkit)
        {
            _appViewModel = appViewModel;
            _pgcProvider = pgcProvider;
            _resourceToolkit = resourceToolkit;

            Seasons = new ObservableCollection<SeasonItemViewModel>();

            ShowMoreCommand = ReactiveCommand.Create(ShowMore, outputScheduler: RxApp.MainThreadScheduler);
            InitializeCommand = ReactiveCommand.CreateFromTask(InitializeAsync, outputScheduler: RxApp.MainThreadScheduler);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isReloading = InitializeCommand.IsExecuting.Merge(ReloadCommand.IsExecuting)
                .ToProperty(this, x => x.IsReloading, scheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置播放列表.
        /// </summary>
        /// <param name="data">列表数据.</param>
        public void SetPlaylist(PgcPlaylist data)
        {
            Data = data;
            IsShowDetailButton = !string.IsNullOrEmpty(data.Id);
        }

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.GetMessage()
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestPlayListFailed)}\n{msg}";
            LogException(exception);
        }

        private void ShowMore() => _appViewModel.ShowPgcPlaylist(this);

        private async Task InitializeAsync()
        {
            if (Seasons.Count > 0)
            {
                return;
            }

            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (IsReloading)
            {
                return;
            }

            TryClear(Seasons);
            var list = await _pgcProvider.GetPgcPlaylistAsync(Data.Id);
            Subtitle = list.Subtitle;
            foreach (var item in list.Seasons)
            {
                var seasonVM = Splat.Locator.Current.GetService<SeasonItemViewModel>();
                seasonVM.SetInformation(item);
                Seasons.Add(seasonVM);
            }
        }
    }
}
