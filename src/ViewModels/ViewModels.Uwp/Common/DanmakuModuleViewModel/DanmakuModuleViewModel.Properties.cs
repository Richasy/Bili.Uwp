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
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ResetCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<string, bool> SendDanmakuCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<int> LoadSegmentDanmakuCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<double> SeekCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<LiveDanmakuInformation> AddLiveDanmakuCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowDanmaku { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool CanShowDanmaku { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double DanmakuOpacity { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double DanmakuFontSize { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double DanmakuArea { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public double DanmakuSpeed { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string DanmakuFont { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsDanmakuLimit { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsDanmakuMerge { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsDanmakuBold { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool UseCloudShieldSettings { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsStandardSize { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public DanmakuLocation Location { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string Color { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsDanmakuLoading { get; set; }
    }
}
