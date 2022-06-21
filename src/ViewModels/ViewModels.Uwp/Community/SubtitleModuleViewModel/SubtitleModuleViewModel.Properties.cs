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

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 字幕模块视图模型.
    /// </summary>
    public sealed partial class SubtitleModuleViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly List<SubtitleInformation> _subtitles;
        private readonly IPlayerProvider _playerProvider;
        private readonly ISettingsToolkit _settingsToolkit;

        private string _mainId;
        private string _partId;
        private double _currentSeconds;

        /// <summary>
        /// 重新定位命令.
        /// </summary>
        public ReactiveCommand<double, Unit> SeekCommand { get; }

        /// <summary>
        /// 更换字幕的命令.
        /// </summary>
        public ReactiveCommand<SubtitleMeta, Unit> ChangeMetaCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 字幕元数据集合.
        /// </summary>
        public ObservableCollection<SubtitleMeta> Metas { get; }

        /// <summary>
        /// 字幕转换类型集合.
        /// </summary>
        public ObservableCollection<SubtitleConvertType> ConvertTypeCollection { get; }

        /// <summary>
        /// 当前字幕.
        /// </summary>
        [Reactive]
        public string CurrentSubtitle { get; set; }

        /// <summary>
        /// 当前的元数据.
        /// </summary>
        [Reactive]
        public SubtitleMeta CurrentMeta { get; set; }

        /// <summary>
        /// 字幕转换类型.
        /// </summary>
        [Reactive]
        public SubtitleConvertType ConvertType { get; set; }

        /// <summary>
        /// 是否有字幕.
        /// </summary>
        [Reactive]
        public bool HasSubtitles { get; set; }

        /// <summary>
        /// 是否需要显示字幕.
        /// </summary>
        [Reactive]
        public bool CanShowSubtitle { get; set; }

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;
    }
}
