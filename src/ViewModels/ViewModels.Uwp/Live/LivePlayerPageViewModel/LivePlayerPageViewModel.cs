﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Live;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播播放页面视图模型.
    /// </summary>
    public sealed partial class LivePlayerPageViewModel : PlayerPageViewModelBase, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlayerPageViewModel"/> class.
        /// </summary>
        public LivePlayerPageViewModel(
            IPlayerProvider playerProvider,
            IAuthorizeProvider authorizeProvider,
            ILiveProvider liveProvider,
            IResourceToolkit resourceToolkit,
            INumberToolkit numberToolkit,
            ISettingsToolkit settingsToolkit,
            ICallerViewModel callerViewModel,
            IRecordViewModel recordViewModel,
            INavigationViewModel navigationViewModel,
            AccountViewModel accountViewModel,
            MediaPlayerViewModel playerViewModel,
            CoreDispatcher dispatcher)
            : base(playerViewModel)
        {
            _playerProvider = playerProvider;
            _authorizeProvider = authorizeProvider;
            _liveProvider = liveProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _settingsToolkit = settingsToolkit;
            _callerViewModel = callerViewModel;
            _recordViewModel = recordViewModel;
            _navigationViewModel = navigationViewModel;
            _accountViewModel = accountViewModel;
            _dispatcher = dispatcher;

            Danmakus = new ObservableCollection<LiveDanmakuInformation>();
            Sections = new ObservableCollection<PlayerSectionHeader>
            {
                new PlayerSectionHeader(PlayerSectionType.Chat, _resourceToolkit.GetLocaleString(LanguageNames.Chat)),
            };
            CurrentSection = Sections.First();

            IsSignedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;

            ReloadCommand = ReactiveCommand.CreateFromTask(GetDataAsync);
            ShareCommand = ReactiveCommand.Create(Share);
            FixedCommand = ReactiveCommand.Create(Fix);
            ClearCommand = ReactiveCommand.Create(Reset);
            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading);

            ReloadCommand.ThrownExceptions
                .Subscribe(DisplayException);
            ClearCommand.ThrownExceptions
                .Merge(ShareCommand.ThrownExceptions)
                .Merge(FixedCommand.ThrownExceptions)
                .Merge(OpenInBroswerCommand.ThrownExceptions)
                .Subscribe(LogException);

            Danmakus.CollectionChanged += OnDanmakusCollectionChanged;
        }

        /// <summary>
        /// 设置直播间.
        /// </summary>
        /// <param name="snapshot">直播间信息.</param>
        public void SetSnapshot(PlaySnapshot snapshot)
        {
            _presetRoomId = snapshot.VideoId;
            var defaultPlayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            MediaPlayerViewModel.DisplayMode = snapshot.DisplayMode ?? defaultPlayMode;
            ReloadCommand.Execute().Subscribe();
            _liveProvider.MessageReceived += OnMessageReceivedAsync;
        }

        private void Reset()
        {
            View = null;
            MediaPlayerViewModel.ClearCommand.Execute().Subscribe();
            ResetTimers();
            ResetPublisher();
            ResetOverview();
            ResetInterop();
        }

        private async Task GetDataAsync()
        {
            Reset();
            View = await _liveProvider.GetLiveRoomDetailAsync(_presetRoomId);
            var snapshot = new PlaySnapshot(View.Information.Identifier.Id, default, VideoType.Live)
            {
                Title = View.Information.Identifier.Title,
            };
            _recordViewModel.AddPlayRecordCommand.Execute(new PlayRecord(View.Information.Identifier, snapshot)).Subscribe();

            var isEnterSuccess = await _liveProvider.EnterLiveRoomAsync(_presetRoomId);

            if (isEnterSuccess)
            {
                InitializeTimers();
                InitializePublisher();
                InitializeOverview();
                InitializeInterop();

                MediaPlayerViewModel.SetLiveData(View);
            }
            else
            {
                DisplayException(new Exception("进入直播间失败"));
            }
        }
    }
}
