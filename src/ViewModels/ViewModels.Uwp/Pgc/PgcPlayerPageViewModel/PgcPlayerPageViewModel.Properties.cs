// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IAppToolkit _appToolkit;
        private readonly IPgcProvider _pgcProvider;
        private readonly ICallerViewModel _callerViewModel;
        private readonly IRecordViewModel _recordViewModel;
        private readonly IAccountViewModel _accountViewModel;
        private readonly ICommentPageViewModel _commentPageViewModel;

        private string _presetEpisodeId;
        private string _presetSeasonId;
        private string _presetTitle;
        private bool _needBiliPlus;
        private Action _playNextEpisodeAction;

        [ObservableProperty]
        private bool _isReloading;

        [ObservableProperty]
        private bool _isFavoriteFolderRequesting;

        [ObservableProperty]
        private PgcPlayerView _view;

        [ObservableProperty]
        private bool _isSignedIn;

        [ObservableProperty]
        private string _playCountText;

        [ObservableProperty]
        private string _danmakuCountText;

        [ObservableProperty]
        private string _commentCountText;

        [ObservableProperty]
        private string _likeCountText;

        [ObservableProperty]
        private string _coinCountText;

        [ObservableProperty]
        private string _favoriteCountText;

        [ObservableProperty]
        private string _ratingCountText;

        [ObservableProperty]
        private bool _isLiked;

        [ObservableProperty]
        private bool _isCoined;

        [ObservableProperty]
        private bool _isFavorited;

        [ObservableProperty]
        private bool _isTracking;

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
        private bool _isShowCelebrities;

        [ObservableProperty]
        private PlayerSectionHeader _currentSection;

        [ObservableProperty]
        private EpisodeInformation _currentEpisode;

        [ObservableProperty]
        private bool _isShowSeasons;

        [ObservableProperty]
        private bool _isShowEpisodes;

        [ObservableProperty]
        private bool _isShowComments;

        [ObservableProperty]
        private bool _isShowExtras;

        [ObservableProperty]
        private bool _isSectionsEmpty;

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand RequestFavoriteFoldersCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<EpisodeInformation> ChangeEpisodeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<SeasonInformation> ChangeSeasonCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand FavoriteEpisodeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand TrackSeasonCommand { get; }

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
        public IRelayCommand ShowSeasonDetailCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoFavoriteFolderSelectableViewModel> FavoriteFolders { get; }

        /// <inheritdoc/>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <inheritdoc/>
        public ObservableCollection<IEpisodeItemViewModel> Episodes { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoIdentifierSelectableViewModel> Seasons { get; }

        /// <inheritdoc/>
        public ObservableCollection<IPgcExtraItemViewModel> Extras { get; }

        /// <inheritdoc/>
        public ObservableCollection<IUserItemViewModel> Celebrities { get; }

        /// <inheritdoc/>
        public IDownloadModuleViewModel DownloadViewModel { get; }
    }
}
