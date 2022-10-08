// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
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
using Bili.ViewModels.Uwp.Community;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

        /// <inheritdoc/>
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand RequestFavoriteFoldersCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand RequestOnlineCountCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<VideoIdentifier> ChangeVideoPartCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<Tag> SearchTagCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<VideoSeason> SelectSeasonCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand FavoriteVideoCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<int> CoinCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand LikeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand TripleCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ReloadCommunityInformationCommand { get; }

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

        /// <inheritdoc/>
        [ObservableProperty]
        public VideoPlayerView View { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSignedIn { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public IUserItemViewModel Author { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsCooperationVideo { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string PublishTime { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string PlayCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string DanmakuCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string CommentCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string WatchingCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowTags { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string CoinCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string FavoriteCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsLiked { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsCoined { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsFavorited { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsCoinWithLiked { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsFavoriteFoldersError { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string FavoriteFoldersErrorText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsVideoFixed { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsOnlyShowIndex { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public VideoSeason CurrentSeason { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public VideoIdentifier CurrentVideoPart { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowUgcSeason { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowRelatedVideos { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowVideoPlaylist { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowComments { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowParts { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsFavoriteFolderRequesting { get; set; }
    }
}
