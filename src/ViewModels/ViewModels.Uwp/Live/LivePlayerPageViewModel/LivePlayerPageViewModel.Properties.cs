// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Live;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播播放页面视图模型.
    /// </summary>
    public sealed partial class LivePlayerPageViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly ILiveProvider _liveProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly ISettingsToolkit _settingsToolkit;

        private readonly AppViewModel _appViewModel;
        private readonly ICallerViewModel _callerViewModel;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly AccountViewModel _accountViewModel;
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;

        private DispatcherTimer _heartBeatTimer;
        private string _presetRoomId;

        /// <summary>
        /// 当有新的弹幕传入，预期让弹幕池滚动到底部的事件.
        /// </summary>
        public event EventHandler RequestDanmakusScrollToBottom;

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 分享命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShareCommand { get; }

        /// <summary>
        /// 固定条目命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> FixedCommand { get; }

        /// <summary>
        /// 清除数据命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <summary>
        /// 弹幕池.
        /// </summary>
        public ObservableCollection<LiveDanmakuInformation> Danmakus { get; }

        /// <summary>
        /// 播放时的关联区块集合.
        /// </summary>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <summary>
        /// 视图信息.
        /// </summary>
        [Reactive]
        public LivePlayerView View { get; set; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        [Reactive]
        public bool IsSignedIn { get; set; }

        /// <summary>
        /// 直播 UP 主.
        /// </summary>
        [Reactive]
        public IUserItemViewModel User { get; set; }

        /// <summary>
        /// 正在观看人数的可读文本.
        /// </summary>
        [Reactive]
        public string WatchingCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 该直播是否已经被固定在首页.
        /// </summary>
        [Reactive]
        public bool IsLiveFixed { get; set; }

        /// <summary>
        /// 当前区块.
        /// </summary>
        [Reactive]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <summary>
        /// 弹幕池是否为空.
        /// </summary>
        [Reactive]
        public bool IsDanmakusEmpty { get; set; }

        /// <summary>
        /// 是否允许弹幕池自动滚动.
        /// </summary>
        public bool IsDanmakusAutoScroll { get; set; }
    }
}
