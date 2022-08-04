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
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly ILiveProvider _liveProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly ISettingsToolkit _settingsToolkit;

        private readonly IRecordViewModel _recordViewModel;
        private readonly ICallerViewModel _callerViewModel;
        private readonly IAccountViewModel _accountViewModel;
        private readonly CoreDispatcher _dispatcher;

        private DispatcherTimer _heartBeatTimer;
        private string _presetRoomId;

        /// <inheritdoc/>
        public event EventHandler RequestDanmakusScrollToBottom;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ShareCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> FixedCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<LiveDanmakuInformation> Danmakus { get; }

        /// <inheritdoc/>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <inheritdoc/>
        [Reactive]
        public LivePlayerView View { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSignedIn { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public IUserItemViewModel User { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string WatchingCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsLiveFixed { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsDanmakusEmpty { get; set; }

        /// <inheritdoc/>
        public bool IsDanmakusAutoScroll { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}
