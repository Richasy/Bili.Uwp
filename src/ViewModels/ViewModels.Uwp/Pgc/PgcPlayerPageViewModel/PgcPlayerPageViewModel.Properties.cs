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
        private readonly CommentPageViewModel _commentPageViewModel;

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
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> RequestFavoriteFoldersCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<EpisodeInformation, Unit> ChangeEpisodeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<SeasonInformation, Unit> ChangeSeasonCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> FavoriteEpisodeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> TrackSeasonCommand { get; }

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
        public ReactiveCommand<Unit, Unit> ShowSeasonDetailCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

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
        [Reactive]
        public PgcPlayerView View { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSignedIn { get; set; }

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
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string CoinCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string FavoriteCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string RatingCountText { get; set; }

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
        public bool IsTracking { get; set; }

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
        public bool IsShowCelebrities { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public EpisodeInformation CurrentEpisode { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowSeasons { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowEpisodes { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowComments { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowExtras { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSectionsEmpty { get; set; }
    }
}
