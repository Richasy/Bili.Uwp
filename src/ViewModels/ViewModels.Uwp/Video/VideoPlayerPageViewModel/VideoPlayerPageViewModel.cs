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
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Common;
using Bili.ViewModels.Uwp.Community;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel : PlayerPageViewModelBase, IReloadViewModel, IErrorViewModel
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
            CommentPageViewModel commentPageViewModel,
            MediaPlayerViewModel playerViewModel,
            DownloadModuleViewModel downloadViewModel,
            CoreDispatcher dispatcher)
            : base(playerViewModel)
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
            FavoriteFolders = new ObservableCollection<VideoFavoriteFolderSelectableViewModel>();
            Sections = new ObservableCollection<Models.App.Other.PlayerSectionHeader>();
            RelatedVideos = new ObservableCollection<IVideoItemViewModel>();
            VideoParts = new ObservableCollection<VideoIdentifierSelectableViewModel>();
            Seasons = new ObservableCollection<VideoSeason>();
            CurrentSeasonVideos = new ObservableCollection<IVideoItemViewModel>();
            VideoPlaylist = new ObservableCollection<IVideoItemViewModel>();

            DownloadViewModel = downloadViewModel;

            IsSignedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            MediaPlayerViewModel.MediaEnded += OnMediaEnded;
            MediaPlayerViewModel.InternalPartChanged += OnInternalPartChanged;

            ReloadCommand = ReactiveCommand.CreateFromTask(GetDataAsync);
            RequestFavoriteFoldersCommand = ReactiveCommand.CreateFromTask(GetFavoriteFoldersAsync);
            SearchTagCommand = ReactiveCommand.Create<Tag>(SearchTag);
            SelectSeasonCommand = ReactiveCommand.Create<VideoSeason>(SelectSeason);
            ReloadCommunityInformationCommand = ReactiveCommand.CreateFromTask(ReloadCommunityInformationAsync);
            RequestOnlineCountCommand = ReactiveCommand.CreateFromTask(GetOnlineCountAsync);
            FavoriteVideoCommand = ReactiveCommand.CreateFromTask(FavoriteVideoAsync);
            CoinCommand = ReactiveCommand.CreateFromTask<int>(CoinAsync);
            LikeCommand = ReactiveCommand.CreateFromTask(LikeAsync);
            TripleCommand = ReactiveCommand.CreateFromTask(TripleAsync);
            ShareCommand = ReactiveCommand.Create(Share);
            FixedCommand = ReactiveCommand.Create(Fix);
            ClearCommand = ReactiveCommand.Create(Reset);
            ChangeVideoPartCommand = ReactiveCommand.Create<VideoIdentifier>(ChangeVideoPart);
            ClearPlaylistCommand = ReactiveCommand.Create(ClearPlaylist);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading);
            _isFavoriteFolderRequesting = RequestFavoriteFoldersCommand.IsExecuting.ToProperty(this, x => x.IsFavoriteFolderRequesting);

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

        /// <summary>
        /// 设置视频.
        /// </summary>
        /// <param name="snapshot">视频信息.</param>
        public void SetSnapshot(PlaySnapshot snapshot)
        {
            _presetVideoId = snapshot.VideoId;
            var defaultPlayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            MediaPlayerViewModel.DisplayMode = snapshot.DisplayMode ?? defaultPlayMode;
            ReloadCommand.Execute().Subscribe();
        }

        /// <summary>
        /// 设置播放列表.
        /// </summary>
        /// <param name="videos">视频列表.</param>
        /// <param name="playIndex">需要播放的视频索引.</param>
        public void SetPlaylist(IEnumerable<VideoInformation> videos, int playIndex = 0)
        {
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
            MediaPlayerViewModel.ClearCommand.Execute().Subscribe();
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
    }
}
