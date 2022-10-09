// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly ICallerViewModel _callerViewModel;
        private readonly IRecordViewModel _recordViewModel;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly IAccountViewModel _accountViewModel;
        private readonly ICommentPageViewModel _commentPageViewModel;
        private readonly CoreDispatcher _dispatcher;

        private string _presetVideoId;
        private Action _playNextVideoAction;

        [ObservableProperty]
        private VideoPlayerView _view;

        [ObservableProperty]
        private bool _isSignedIn;

        [ObservableProperty]
        private IUserItemViewModel _author;

        [ObservableProperty]
        private bool _isCooperationVideo;

        [ObservableProperty]
        private string _publishTime;

        [ObservableProperty]
        private string _playCountText;

        [ObservableProperty]
        private string _danmakuCountText;

        [ObservableProperty]
        private string _commentCountText;

        [ObservableProperty]
        private string _watchingCountText;

        [ObservableProperty]
        private bool _isShowTags;

        [ObservableProperty]
        private string _likeCountText;

        [ObservableProperty]
        private string _coinCountText;

        [ObservableProperty]
        private string _favoriteCountText;

        [ObservableProperty]
        private bool _isLiked;

        [ObservableProperty]
        private bool _isCoined;

        [ObservableProperty]
        private bool _isFavorited;

        [ObservableProperty]
        private bool _isCoinWithLiked;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isFavoriteFoldersError;

        [ObservableProperty]
        private string _favoriteFoldersErrorText;

        [ObservableProperty]
        private bool _isVideoFixed;

        [ObservableProperty]
        private bool _isOnlyShowIndex;

        [ObservableProperty]
        private PlayerSectionHeader _currentSection;

        [ObservableProperty]
        private VideoSeason _currentSeason;

        [ObservableProperty]
        private VideoIdentifier _currentVideoPart;

        [ObservableProperty]
        private bool _isShowUgcSeason;

        [ObservableProperty]
        private bool _isShowRelatedVideos;

        [ObservableProperty]
        private bool _isShowVideoPlaylist;

        [ObservableProperty]
        private bool _isShowComments;

        [ObservableProperty]
        private bool _isShowParts;

        [ObservableProperty]
        private bool _isReloading;

        [ObservableProperty]
        private bool _isFavoriteFolderRequesting;

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand RequestFavoriteFoldersCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand RequestOnlineCountCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<VideoIdentifier> ChangeVideoPartCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<Tag> SearchTagCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<VideoSeason> SelectSeasonCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand FavoriteVideoCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<int> CoinCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand LikeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand TripleCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommunityInformationCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShareCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand FixedCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearPlaylistCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<IUserItemViewModel> Collaborators { get; }

        /// <inheritdoc/>
        public ObservableCollection<Tag> Tags { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoFavoriteFolderSelectableViewModel> FavoriteFolders { get; }

        /// <inheritdoc/>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> RelatedVideos { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> VideoPlaylist { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoIdentifierSelectableViewModel> VideoParts { get; }

        /// <inheritdoc/>
        public ObservableCollection<VideoSeason> Seasons { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> CurrentSeasonVideos { get; set; }

        /// <inheritdoc/>
        public IDownloadModuleViewModel DownloadViewModel { get; }
    }
}
