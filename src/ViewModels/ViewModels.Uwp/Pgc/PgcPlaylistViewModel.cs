// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
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
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放列表视图模型.
    /// </summary>
    public sealed class PgcPlaylistViewModel : ViewModelBase, IInitializeViewModel, IReloadViewModel
    {
        private readonly AppViewModel _appViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;

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
        /// 初始数据.
        /// </summary>
        [Reactive]
        public PgcPlaylist Data { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [Reactive]
        public string Subtitle { get; set; }

        /// <summary>
        /// 是否显示详情按钮.
        /// </summary>
        [Reactive]
        public bool IsShowDetailButton { get; set; }

        /// <summary>
        /// 是否错误.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        public ObservableCollection<SeasonItemViewModel> Seasons { get; }

        /// <summary>
        /// 显示更多的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowMoreCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <summary>
        /// 设置播放列表.
        /// </summary>
        /// <param name="data">列表数据.</param>
        public void SetPlaylist(PgcPlaylist data)
        {
            Data = data;
            IsShowDetailButton = !string.IsNullOrEmpty(data.Id);
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

            Seasons.Clear();
            var list = await _pgcProvider.GetPgcPlaylistAsync(Data.Id);
            Subtitle = list.Subtitle;
            foreach (var item in list.Seasons)
            {
                var seasonVM = Splat.Locator.Current.GetService<SeasonItemViewModel>();
                seasonVM.SetInformation(item);
                Seasons.Add(seasonVM);
            }
        }

        private void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.Error?.Message ?? se.Message
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestPlayListFailed)}\n{msg}";
            LogException(exception);
        }
    }
}
