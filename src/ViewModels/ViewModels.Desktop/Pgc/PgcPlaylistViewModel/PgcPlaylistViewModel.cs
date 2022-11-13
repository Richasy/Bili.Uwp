// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Pgc
{
    /// <summary>
    /// PGC 播放列表视图模型.
    /// </summary>
    public sealed partial class PgcPlaylistViewModel : ViewModelBase, IPgcPlaylistViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlaylistViewModel"/> class.
        /// </summary>
        public PgcPlaylistViewModel(
            ICallerViewModel callerViewModel,
            IPgcProvider pgcProvider,
            IResourceToolkit resourceToolkit)
        {
            _callerViewModel = callerViewModel;
            _pgcProvider = pgcProvider;
            _resourceToolkit = resourceToolkit;

            Seasons = new ObservableCollection<ISeasonItemViewModel>();

            ShowMoreCommand = new RelayCommand(ShowMore);
            InitializeCommand = new AsyncRelayCommand(InitializeAsync);
            ReloadCommand = new AsyncRelayCommand(ReloadAsync);

            AttachIsRunningToAsyncCommand(p => IsReloading = p, InitializeCommand, ReloadCommand);
        }

        /// <summary>
        /// 设置播放列表.
        /// </summary>
        /// <param name="data">列表数据.</param>
        public void InjectData(PgcPlaylist data)
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

        private void ShowMore() => _callerViewModel.ShowPgcPlaylist(this);

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
                var seasonVM = Locator.Instance.GetService<ISeasonItemViewModel>();
                seasonVM.InjectData(item);
                Seasons.Add(seasonVM);
            }
        }
    }
}
