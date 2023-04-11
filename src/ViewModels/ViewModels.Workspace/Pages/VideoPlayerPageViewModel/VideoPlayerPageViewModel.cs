// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Workspace.Core;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Workspace.Pages
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
            IAccountViewModel accountViewModel)
        {
            _playerProvider = playerProvider;
            _authorizeProvider = authorizeProvider;
            _favoriteProvider = favoriteProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _settingsToolkit = settingsToolkit;
            _callerViewModel = callerViewModel;
            _accountViewModel = accountViewModel;
            _dispatcher = DispatcherQueue.GetForCurrentThread();

            Collaborators = new ObservableCollection<IUserItemViewModel>();
            Tags = new ObservableCollection<Tag>();
            FavoriteFolders = new ObservableCollection<IVideoFavoriteFolderSelectableViewModel>();
            Sections = new ObservableCollection<Models.App.Other.PlayerSectionHeader>();
            RelatedVideos = new ObservableCollection<IVideoItemViewModel>();
            VideoParts = new ObservableCollection<IVideoIdentifierSelectableViewModel>();
            Seasons = new ObservableCollection<VideoSeason>();
            CurrentSeasonVideos = new ObservableCollection<IVideoItemViewModel>();
            VideoPlaylist = new ObservableCollection<IVideoItemViewModel>();

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

            AttachIsRunningToAsyncCommand(p => IsReloading = p, ReloadCommand);
            AttachIsRunningToAsyncCommand(p => IsFavoriteFolderRequesting = p, RequestFavoriteFoldersCommand);

            AttachExceptionHandlerToAsyncCommand(DisplayException, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayFavoriteFoldersException, RequestFavoriteFoldersCommand);
        }

        /// <inheritdoc/>
        public void SetSnapshot(PlaySnapshot snapshot)
        {
            ReloadMediaPlayer();
            _presetVideoId = snapshot.VideoId;
            _isInPrivate = snapshot.IsInPrivate;
            var defaultPlayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            MediaPlayerViewModel.DisplayMode = snapshot.DisplayMode ?? defaultPlayMode;
            ReloadCommand.ExecuteAsync(null);
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

            // InitializePublisher();
            // InitializeOverview();
            // InitializeOperation();
            // InitializeCommunityInformation();
            // InitializeSections();
            // InitializeInterop();
            MediaPlayerViewModel.SetVideoData(View, _isInPrivate);
        }

        private void Clear()
        {
            Reset();
            if (MediaPlayerViewModel != null)
            {
                MediaPlayerViewModel.MediaEnded -= OnMediaEnded;
                MediaPlayerViewModel.InternalPartChanged -= OnInternalPartChanged;
                MediaPlayerViewModel.ClearCommand.Execute(null);
                MediaPlayerViewModel = null;
            }
        }

        private void ReloadMediaPlayer()
        {
            if (MediaPlayerViewModel != null)
            {
                return;
            }

            MediaPlayerViewModel = Locator.Instance.GetService<IMediaPlayerViewModel>();
            MediaPlayerViewModel.MediaEnded += OnMediaEnded;
            MediaPlayerViewModel.InternalPartChanged += OnInternalPartChanged;
        }

        partial void OnCurrentSectionChanged(PlayerSectionHeader value)
        {
            if (value != null)
            {
                CheckSectionVisibility();
            }
        }

        partial void OnIsOnlyShowIndexChanged(bool value)
            => _settingsToolkit.WriteLocalSetting(SettingNames.IsOnlyShowIndex, value);
    }
}
