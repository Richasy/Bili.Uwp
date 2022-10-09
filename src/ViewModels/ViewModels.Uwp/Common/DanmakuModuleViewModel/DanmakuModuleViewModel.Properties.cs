// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Live;
using Bili.Models.Data.Local;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private bool _isShowDanmaku;

        [ObservableProperty]
        private bool _canShowDanmaku;

        [ObservableProperty]
        private double _danmakuOpacity;

        [ObservableProperty]
        private double _danmakuFontSize;

        [ObservableProperty]
        private double _danmakuArea;

        [ObservableProperty]
        private double _danmakuSpeed;

        [ObservableProperty]
        private string _danmakuFont;

        [ObservableProperty]
        private bool _isDanmakuLimit;

        [ObservableProperty]
        private bool _isDanmakuMerge;

        [ObservableProperty]
        private bool _isDanmakuBold;

        [ObservableProperty]
        private bool _useCloudShieldSettings;

        [ObservableProperty]
        private bool _isStandardSize;

        [ObservableProperty]
        private DanmakuLocation _location;

        [ObservableProperty]
        private string _color;

        [ObservableProperty]
        private bool _isReloading;

        [ObservableProperty]
        private bool _isDanmakuLoading;

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
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ResetCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<string> SendDanmakuCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<int> LoadSegmentDanmakuCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<double> SeekCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<LiveDanmakuInformation> AddLiveDanmakuCommand { get; }
    }
}
