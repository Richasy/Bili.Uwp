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
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly ObservableAsPropertyHelper<bool> _isDanmakuLoading;
        private readonly ICallerViewModel _callerViewModel;

        private string _mainId;
        private string _partId;
        private int _segmentIndex;
        private double _currentSeconds;
        private VideoType _videoType;

        /// <summary>
        /// 弹幕列表已添加.
        /// </summary>
        public event EventHandler<IEnumerable<DanmakuInformation>> DanmakuListAdded;

        /// <summary>
        /// 已成功发送弹幕.
        /// </summary>
        public event EventHandler<string> SendDanmakuSucceeded;

        /// <summary>
        /// 请求清除弹幕列表.
        /// </summary>
        public event EventHandler RequestClearDanmaku;

        /// <summary>
        /// 直播弹幕已添加.
        /// </summary>
        public event EventHandler<LiveDanmakuInformation> LiveDanmakuAdded;

        /// <summary>
        /// 弹幕位置可选集合.
        /// </summary>
        public ObservableCollection<DanmakuLocation> LocationCollection { get; }

        /// <summary>
        /// 弹幕颜色集合.
        /// </summary>
        public ObservableCollection<KeyValue<string>> ColorCollection { get; }

        /// <summary>
        /// 系统字体集合.
        /// </summary>
        public ObservableCollection<string> FontCollection { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 重置命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }

        /// <summary>
        /// 发送弹幕命令.
        /// </summary>
        public ReactiveCommand<string, bool> SendDanmakuCommand { get; }

        /// <summary>
        /// 获取分片弹幕命令.
        /// </summary>
        public ReactiveCommand<int, Unit> LoadSegmentDanmakuCommand { get; }

        /// <summary>
        /// 重新定位命令.
        /// </summary>
        public ReactiveCommand<double, Unit> SeekCommand { get; }

        /// <summary>
        /// 添加新的直播弹幕命令.
        /// </summary>
        public ReactiveCommand<LiveDanmakuInformation, Unit> AddLiveDanmakuCommand { get; }

        /// <summary>
        /// 是否显示弹幕.
        /// </summary>
        [Reactive]
        public bool IsShowDanmaku { get; set; }

        /// <summary>
        /// 是否可以显示弹幕.
        /// </summary>
        [Reactive]
        public bool CanShowDanmaku { get; set; }

        /// <summary>
        /// 弹幕透明度.
        /// </summary>
        [Reactive]
        public double DanmakuOpacity { get; set; }

        /// <summary>
        /// 弹幕文本大小.
        /// </summary>
        [Reactive]
        public double DanmakuFontSize { get; set; }

        /// <summary>
        /// 弹幕显示区域.
        /// </summary>
        [Reactive]
        public double DanmakuArea { get; set; }

        /// <summary>
        /// 弹幕速度.
        /// </summary>
        [Reactive]
        public double DanmakuSpeed { get; set; }

        /// <summary>
        /// 弹幕字体.
        /// </summary>
        [Reactive]
        public string DanmakuFont { get; set; }

        /// <summary>
        /// 是否启用同屏弹幕限制.
        /// </summary>
        [Reactive]
        public bool IsDanmakuLimit { get; set; }

        /// <summary>
        /// 是否启用弹幕合并.
        /// </summary>
        [Reactive]
        public bool IsDanmakuMerge { get; set; }

        /// <summary>
        /// 是否加粗弹幕.
        /// </summary>
        [Reactive]
        public bool IsDanmakuBold { get; set; }

        /// <summary>
        /// 是否启用云屏蔽设置.
        /// </summary>
        [Reactive]
        public bool UseCloudShieldSettings { get; set; }

        /// <summary>
        /// 是否为标准字号.
        /// </summary>
        [Reactive]
        public bool IsStandardSize { get; set; }

        /// <summary>
        /// 弹幕位置.
        /// </summary>
        [Reactive]
        public DanmakuLocation Location { get; set; }

        /// <summary>
        /// 弹幕颜色.
        /// </summary>
        [Reactive]
        public string Color { get; set; }

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <summary>
        /// 是否正在加载分片弹幕.
        /// </summary>
        public bool IsDanmakuLoading => _isDanmakuLoading.Value;
    }
}
