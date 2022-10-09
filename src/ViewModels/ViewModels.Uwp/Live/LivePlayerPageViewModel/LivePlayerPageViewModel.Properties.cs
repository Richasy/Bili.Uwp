// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Live;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        private LivePlayerView _view;

        [ObservableProperty]
        private bool _isSignedIn;

        [ObservableProperty]
        private IUserItemViewModel _user;

        [ObservableProperty]
        private string _watchingCountText;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isLiveFixed;

        [ObservableProperty]
        private PlayerSectionHeader _currentSection;

        [ObservableProperty]
        private bool _isDanmakusEmpty;

        [ObservableProperty]
        private bool _isDanmakusAutoScroll;

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public event EventHandler RequestDanmakusScrollToBottom;

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShareCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand FixedCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<LiveDanmakuInformation> Danmakus { get; }

        /// <inheritdoc/>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }
    }
}
