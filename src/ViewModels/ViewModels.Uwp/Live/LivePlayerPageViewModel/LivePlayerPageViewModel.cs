// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Live;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Live;
using Bili.ViewModels.Uwp.Base;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播播放页面视图模型.
    /// </summary>
    public sealed partial class LivePlayerPageViewModel : PlayerPageViewModelBase, ILivePlayerPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlayerPageViewModel"/> class.
        /// </summary>
        public LivePlayerPageViewModel(
            IAuthorizeProvider authorizeProvider,
            ILiveProvider liveProvider,
            IResourceToolkit resourceToolkit,
            INumberToolkit numberToolkit,
            ISettingsToolkit settingsToolkit,
            ICallerViewModel callerViewModel,
            IRecordViewModel recordViewModel,
            IAccountViewModel accountViewModel,
            IMediaPlayerViewModel mediaPlayerViewModel)
        {
            _authorizeProvider = authorizeProvider;
            _liveProvider = liveProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _settingsToolkit = settingsToolkit;
            _callerViewModel = callerViewModel;
            _recordViewModel = recordViewModel;
            _accountViewModel = accountViewModel;
            _dispatcher = Window.Current.CoreWindow.Dispatcher;

            Danmakus = new ObservableCollection<LiveDanmakuInformation>();
            Sections = new ObservableCollection<PlayerSectionHeader>
            {
                new PlayerSectionHeader(PlayerSectionType.Chat, _resourceToolkit.GetLocaleString(LanguageNames.Chat)),
            };
            CurrentSection = Sections.First();

            MediaPlayerViewModel = mediaPlayerViewModel;
            IsSignedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;

            ReloadCommand = new AsyncRelayCommand(GetDataAsync);
            ShareCommand = new RelayCommand(Share);
            FixedCommand = new RelayCommand(Fix);
            ClearCommand = new RelayCommand(Reset);
            OpenInBroswerCommand = new AsyncRelayCommand(OpenInBroswerAsync);

            AttachIsRunningToAsyncCommand(p => IsReloading = p, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayException, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(LogException, OpenInBroswerCommand);

            Danmakus.CollectionChanged += OnDanmakusCollectionChanged;
        }

        /// <inheritdoc/>
        public void SetSnapshot(PlaySnapshot snapshot)
        {
            _presetRoomId = snapshot.VideoId;
            var defaultPlayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            ReloadCommand.ExecuteAsync(null)
                .ContinueWith(async _ =>
                {
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        MediaPlayerViewModel.DisplayMode = snapshot.DisplayMode ?? defaultPlayMode;
                    });
                });

            _liveProvider.MessageReceived += OnMessageReceivedAsync;
        }

        private void Reset()
        {
            View = null;
            MediaPlayerViewModel.ClearCommand.Execute(null);
            ResetTimers();
            ResetPublisher();
            ResetOverview();
            ResetInterop();
        }

        private async Task GetDataAsync()
        {
            Reset();
            MediaPlayerViewModel = Locator.Instance.GetService<IMediaPlayerViewModel>();
            View = await _liveProvider.GetLiveRoomDetailAsync(_presetRoomId);
            var snapshot = new PlaySnapshot(View.Information.Identifier.Id, default, VideoType.Live)
            {
                Title = View.Information.Identifier.Title,
            };
            _recordViewModel.AddPlayRecordCommand.Execute(new PlayRecord(View.Information.Identifier, snapshot));

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
