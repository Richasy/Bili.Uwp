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
        private readonly CommentPageViewModel _commentPageViewModel;
        private readonly CoreDispatcher _dispatcher;

        private string _presetVideoId;
        private Action _playNextVideoAction;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> RequestFavoriteFoldersCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> RequestOnlineCountCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<VideoIdentifier, Unit> ChangeVideoPartCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Tag, Unit> SearchTagCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<VideoSeason, Unit> SelectSeasonCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> FavoriteVideoCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<int, Unit> CoinCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> LikeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> TripleCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommunityInformationCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ShareCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> FixedCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearPlaylistCommand { get; }

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
        [Reactive]
        public VideoPlayerView View { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSignedIn { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public IUserItemViewModel Author { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsCooperationVideo { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string PublishTime { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string PlayCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string DanmakuCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string CommentCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string WatchingCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowTags { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string CoinCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string FavoriteCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsLiked { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsCoined { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsFavorited { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsCoinWithLiked { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsFavoriteFoldersError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string FavoriteFoldersErrorText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsVideoFixed { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsOnlyShowIndex { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public VideoSeason CurrentSeason { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public VideoIdentifier CurrentVideoPart { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowUgcSeason { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowRelatedVideos { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowVideoPlaylist { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowComments { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowParts { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsFavoriteFolderRequesting { get; set; }
    }
}
