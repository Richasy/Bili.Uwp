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
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShareCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand FixedCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<LiveDanmakuInformation> Danmakus { get; }

        /// <inheritdoc/>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public LivePlayerView View { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSignedIn { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public IUserItemViewModel User { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string WatchingCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsLiveFixed { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsDanmakusEmpty { get; set; }

        /// <inheritdoc/>
        public bool IsDanmakusAutoScroll { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}
