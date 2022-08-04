// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 的播放列表视图模型.
    /// </summary>
    public sealed partial class PgcPlaylistViewModel
    {
        private readonly ICallerViewModel _callerViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ShowMoreCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public PgcPlaylist Data { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string Subtitle { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowDetailButton { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        public ObservableCollection<ISeasonItemViewModel> Seasons { get; }

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;
    }
}
