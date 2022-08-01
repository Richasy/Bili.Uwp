// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Live;
using Bili.Models.Data.Local;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public sealed partial class DanmakuModuleViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IFontToolkit _fontToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IPlayerProvider _playerProvider;
        private readonly ILiveProvider _liveProvider;
        private readonly ICallerViewModel _callerViewModel;

        private string _mainId;
        private string _partId;
        private int _segmentIndex;
        private double _currentSeconds;
        private VideoType _videoType;

        /// <inheritdoc/>
        public event EventHandler<IEnumerable<DanmakuInformation>> DanmakuListAdded;

        /// <inheritdoc/>
        public event EventHandler<string> SendDanmakuSucceeded;

        /// <inheritdoc/>
        public event EventHandler RequestClearDanmaku;

        /// <inheritdoc/>
        public event EventHandler<LiveDanmakuInformation> LiveDanmakuAdded;

        /// <inheritdoc/>
        public ObservableCollection<DanmakuLocation> LocationCollection { get; }

        /// <inheritdoc/>
        public ObservableCollection<KeyValue<string>> ColorCollection { get; }

        /// <inheritdoc/>
        public ObservableCollection<string> FontCollection { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<string, bool> SendDanmakuCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<int, Unit> LoadSegmentDanmakuCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<double, Unit> SeekCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<LiveDanmakuInformation, Unit> AddLiveDanmakuCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowDanmaku { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool CanShowDanmaku { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double DanmakuOpacity { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double DanmakuFontSize { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double DanmakuArea { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public double DanmakuSpeed { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string DanmakuFont { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsDanmakuLimit { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsDanmakuMerge { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsDanmakuBold { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool UseCloudShieldSettings { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsStandardSize { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public DanmakuLocation Location { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string Color { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsDanmakuLoading { get; set; }
    }
}
