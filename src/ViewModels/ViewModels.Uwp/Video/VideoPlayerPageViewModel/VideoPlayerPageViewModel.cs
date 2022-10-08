// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel : PlayerPageViewModelBase, IVideoPlayerPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerPageViewModel"/> class.
        /// </summary>
        public VideoPlayerPageViewModel(
            IPlayerProvider playerProvider,
            IAuthorizeProvider authorizeProvider,
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit,
            INumberToolkit numberToolkit,
            ISettingsToolkit settingsToolkit,
            ICallerViewModel callerViewModel,
            IRecordViewModel recordViewModel,
            INavigationViewModel navigationViewModel,
            IAccountViewModel accountViewModel,
            IDownloadModuleViewModel downloadViewModel,
            ICommentPageViewModel commentPageViewModel,
            CoreDispatcher dispatcher)
        {
            _playerProvider = playerProvider;
            _authorizeProvider = authorizeProvider;
            _favoriteProvider = favoriteProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _settingsToolkit = settingsToolkit;
            _callerViewModel = callerViewModel;
            _navigationViewModel = navigationViewModel;
            _recordViewModel = recordViewModel;
            _commentPageViewModel = commentPageViewModel;
            _accountViewModel = accountViewModel;
            _dispatcher = dispatcher;

            Collaborators = new ObservableCollection<IUserItemViewModel>();
            Tags = new ObservableCollection<Tag>();
            FavoriteFolders = new ObservableCollection<IVideoFavoriteFolderSelectableViewModel>();
            Sections = new ObservableCollection<Models.App.Other.PlayerSectionHeader>();
            RelatedVideos = new ObservableCollection<IVideoItemViewModel>();
            VideoParts = new ObservableCollection<IVideoIdentifierSelectableViewModel>();
            Seasons = new ObservableCollection<VideoSeason>();
            CurrentSeasonVideos = new ObservableCollection<IVideoItemViewModel>();
            VideoPlaylist = new ObservableCollection<IVideoItemViewModel>();

            DownloadViewModel = downloadViewModel;

            IsSignedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;

            ReloadCommand = new AsyncRelayCommand(GetDataAsync);
            RequestFavoriteFoldersCommand = new AsyncRelayCommand(GetFavoriteFoldersAsync);
            SearchTagCommand = new RelayCommand<Tag>(SearchTag);
            SelectSeasonCommand = new RelayCommand<VideoSeason>(SelectSeason);
            ReloadCommunityInformationCommand = new AsyncRelayCommand(ReloadCommunityInformationAsync);
            RequestOnlineCountCommand = new AsyncRelayCommand(GetOnlineCountAsync);
            FavoriteVideoCommand = new AsyncRelayCommand(FavoriteVideoAsync);
            CoinCommand = new AsyncRelayCommand<int>(CoinAsync);
            LikeCommand = new AsyncRelayCommand(LikeAsync);
            TripleCommand = new AsyncRelayCommand(TripleAsync);
            ShareCommand = new RelayCommand(Share);
            FixedCommand = new RelayCommand(Fix);
            ChangeVideoPartCommand = new RelayCommand<VideoIdentifier>(ChangeVideoPart);
            ClearPlaylistCommand = new RelayCommand(ClearPlaylist);
            ClearCommand = new RelayCommand(Clear);

            ReloadCommand.IsExecuting.ToPropertyEx(this, x => x.IsReloading);
            RequestFavoriteFoldersCommand.IsExecuting.ToPropertyEx(this, x => x.IsFavoriteFolderRequesting);

            ReloadCommand.ThrownExceptions.Subscribe(DisplayException);
            RequestFavoriteFoldersCommand.ThrownExceptions.Subscribe(DisplayFavoriteFoldersException);

            this.WhenAnyValue(p => p.CurrentSection)
                .WhereNotNull()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => CheckSectionVisibility());

            this.WhenAnyValue(p => p.IsOnlyShowIndex)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(isShow => _settingsToolkit.WriteLocalSetting(SettingNames.IsOnlyShowIndex, isShow));
        }

        /// <inheritdoc/>
        public void SetSnapshot(PlaySnapshot snapshot)
        {
            ReloadMediaPlayer();
            _presetVideoId = snapshot.VideoId;
            var defaultPlayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            MediaPlayerViewModel.DisplayMode = snapshot.DisplayMode ?? defaultPlayMode;
            ReloadCommand.Execute().Subscribe();
        }

        /// <inheritdoc/>
        public void SetPlaylist(IEnumerable<VideoInformation> videos, int playIndex = 0)
        {
            ReloadMediaPlayer();
            TryClear(VideoPlaylist);
            foreach (var item in videos)
            {
                VideoPlaylist.Add(GetItemViewModel(item));
            }

            var current = VideoPlaylist[playIndex].Data.Identifier;
            var snapshot = new PlaySnapshot(current.Id, default, Models.Enums.VideoType.Video);
            SetSnapshot(snapshot);
        }

        private void Reset()
        {
            View = null;
            ResetPublisher();
            ResetOverview();
            ResetOperation();
            ResetCommunityInformation();
            ResetInterop();
            ResetSections();
        }

        private async Task GetDataAsync()
        {
            Reset();
            View = await _playerProvider.GetVideoDetailAsync(_presetVideoId);
            var snapshot = new PlaySnapshot(View.Information.Identifier.Id, default, VideoType.Video)
            {
                Title = View.Information.Identifier.Title,
            };

            _recordViewModel.AddLastPlayItemCommand.Execute(snapshot).Subscribe();
            _recordViewModel.AddPlayRecordCommand.Execute(new PlayRecord(View.Information.Identifier, snapshot)).Subscribe();
            InitializePublisher();
            InitializeOverview();
            InitializeOperation();
            InitializeCommunityInformation();
            InitializeSections();
            InitializeInterop();

            MediaPlayerViewModel.SetVideoData(View);
        }

        private void Clear()
        {
            Reset();
            if (MediaPlayerViewModel != null)
            {
                MediaPlayerViewModel.MediaEnded -= OnMediaEnded;
                MediaPlayerViewModel.InternalPartChanged -= OnInternalPartChanged;
                MediaPlayerViewModel.ClearCommand.Execute().Subscribe();
                MediaPlayerViewModel = null;
            }
        }

        private void ReloadMediaPlayer()
        {
            if (MediaPlayerViewModel != null)
            {
                return;
            }

            MediaPlayerViewModel = Locator.Current.GetService<IMediaPlayerViewModel>();
            MediaPlayerViewModel.MediaEnded += OnMediaEnded;
            MediaPlayerViewModel.InternalPartChanged += OnInternalPartChanged;
        }
    }
}
