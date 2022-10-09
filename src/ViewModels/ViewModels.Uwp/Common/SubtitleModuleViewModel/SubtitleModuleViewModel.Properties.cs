// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private string _currentSubtitle;

        [ObservableProperty]
        private SubtitleMeta _currentMeta;

        [ObservableProperty]
        private SubtitleConvertType _convertType;

        [ObservableProperty]
        private bool _hasSubtitles;

        [ObservableProperty]
        private bool _canShowSubtitle;

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public IRelayCommand<double> SeekCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<SubtitleMeta> ChangeMetaCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<SubtitleMeta> Metas { get; }

        /// <inheritdoc/>
        public ObservableCollection<SubtitleConvertType> ConvertTypeCollection { get; }
    }
}
