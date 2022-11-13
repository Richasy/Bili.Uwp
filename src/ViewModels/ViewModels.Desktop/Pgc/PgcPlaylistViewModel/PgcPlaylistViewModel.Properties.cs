// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Pgc
{
    /// <summary>
    /// PGC 的播放列表视图模型.
    /// </summary>
    public sealed partial class PgcPlaylistViewModel
    {
        private readonly ICallerViewModel _callerViewModel;
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;

        [ObservableProperty]
        private PgcPlaylist _data;

        [ObservableProperty]
        private string _subtitle;

        [ObservableProperty]
        private bool _isShowDetailButton;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public IRelayCommand ShowMoreCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<ISeasonItemViewModel> Seasons { get; }
    }
}
