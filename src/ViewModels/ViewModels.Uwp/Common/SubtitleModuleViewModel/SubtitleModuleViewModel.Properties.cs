// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 字幕模块视图模型.
    /// </summary>
    public sealed partial class SubtitleModuleViewModel
    {
        private readonly List<SubtitleInformation> _subtitles;
        private readonly IPlayerProvider _playerProvider;
        private readonly ISettingsToolkit _settingsToolkit;

        private string _mainId;
        private string _partId;
        private double _currentSeconds;

        /// <inheritdoc/>
        public ReactiveCommand<double, Unit> SeekCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<SubtitleMeta, Unit> ChangeMetaCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<SubtitleMeta> Metas { get; }

        /// <inheritdoc/>
        public ObservableCollection<SubtitleConvertType> ConvertTypeCollection { get; }

        /// <inheritdoc/>
        [Reactive]
        public string CurrentSubtitle { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public SubtitleMeta CurrentMeta { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public SubtitleConvertType ConvertType { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool HasSubtitles { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool CanShowSubtitle { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}
