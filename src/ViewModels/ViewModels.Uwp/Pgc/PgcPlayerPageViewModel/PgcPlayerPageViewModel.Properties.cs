// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
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
using Bili.ViewModels.Uwp.Community;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

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

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsFavoriteFolderRequesting { get; set; }

        /// <inheritdoc/>
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand RequestFavoriteFoldersCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<EpisodeInformation> ChangeEpisodeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<SeasonInformation> ChangeSeasonCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand FavoriteEpisodeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand TrackSeasonCommand { get; }

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

        /// <inheritdoc/>
        [ObservableProperty]
        public PgcPlayerView View { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSignedIn { get; set; }

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
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string CoinCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string FavoriteCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string RatingCountText { get; set; }

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
        public bool IsTracking { get; set; }

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
        public bool IsShowCelebrities { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public EpisodeInformation CurrentEpisode { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowSeasons { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowEpisodes { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowComments { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowExtras { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSectionsEmpty { get; set; }
    }
}
