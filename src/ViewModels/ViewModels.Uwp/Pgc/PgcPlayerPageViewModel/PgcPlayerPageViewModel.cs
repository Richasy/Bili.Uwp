// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Local;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Common;
using Bili.ViewModels.Uwp.Community;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel : PlayerPageViewModelBase, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlayerPageViewModel"/> class.
        /// </summary>
        public PgcPlayerPageViewModel(
            IPlayerProvider playerProvider,
            IAuthorizeProvider authorizeProvider,
            IFavoriteProvider favoriteProvider,
            IPgcProvider pgcProvider,
            IResourceToolkit resourceToolkit,
            INumberToolkit numberToolkit,
            ISettingsToolkit settingsToolkit,
            IAppToolkit appToolkit,
            AppViewModel appViewModel,
            NavigationViewModel navigationViewModel,
            AccountViewModel accountViewModel,
            CommentPageViewModel commentPageViewModel,
            MediaPlayerViewModel playerViewModel,
            DownloadModuleViewModel downloadViewModel,
            CoreDispatcher dispatcher)
            : base(playerViewModel)
        {
            _playerProvider = playerProvider;
            _authorizeProvider = authorizeProvider;
            _favoriteProvider = favoriteProvider;
            _pgcProvider = pgcProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _settingsToolkit = settingsToolkit;
            _appToolkit = appToolkit;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;
            _accountViewModel = accountViewModel;
            _commentPageViewModel = commentPageViewModel;
            _dispatcher = dispatcher;

            FavoriteFolders = new ObservableCollection<VideoFavoriteFolderSelectableViewModel>();
            Sections = new ObservableCollection<PlayerSectionHeader>();
            Episodes = new ObservableCollection<EpisodeItemViewModel>();
            Seasons = new ObservableCollection<VideoIdentifierSelectableViewModel>();
            Extras = new ObservableCollection<PgcExtraItemViewModel>();
            Celebrities = new ObservableCollection<UserItemViewModel>();

            DownloadViewModel = downloadViewModel;

            IsSignedIn = _authorizeProvider.State == Models.Enums.AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            MediaPlayerViewModel.MediaEnded += OnMediaEnded;
            MediaPlayerViewModel.InternalPartChanged += OnInternalPartChanged;

            ReloadCommand = ReactiveCommand.CreateFromTask(GetDataAsync, outputScheduler: RxApp.MainThreadScheduler);
            RequestFavoriteFoldersCommand = ReactiveCommand.CreateFromTask(GetFavoriteFoldersAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangeSeasonCommand = ReactiveCommand.Create<SeasonInformation>(SelectSeason, outputScheduler: RxApp.MainThreadScheduler);
            ChangeEpisodeCommand = ReactiveCommand.Create<EpisodeInformation>(SelectEpisode, outputScheduler: RxApp.MainThreadScheduler);
            ReloadInteractionInformationCommand = ReactiveCommand.CreateFromTask(RequestEpisodeInteractionInformationAsync, outputScheduler: RxApp.MainThreadScheduler);
            FavoriteEpisodeCommand = ReactiveCommand.CreateFromTask(FavoriteVideoAsync, outputScheduler: RxApp.MainThreadScheduler);
            CoinCommand = ReactiveCommand.CreateFromTask<int>(CoinAsync, outputScheduler: RxApp.MainThreadScheduler);
            LikeCommand = ReactiveCommand.CreateFromTask(LikeAsync, outputScheduler: RxApp.MainThreadScheduler);
            TripleCommand = ReactiveCommand.CreateFromTask(TripleAsync, outputScheduler: RxApp.MainThreadScheduler);
            ShareCommand = ReactiveCommand.Create(Share, outputScheduler: RxApp.MainThreadScheduler);
            FixedCommand = ReactiveCommand.Create(Fix, outputScheduler: RxApp.MainThreadScheduler);
            ClearCommand = ReactiveCommand.Create(Reset, outputScheduler: RxApp.MainThreadScheduler);
            ShowSeasonDetailCommand = ReactiveCommand.Create(ShowSeasonDetail, outputScheduler: RxApp.MainThreadScheduler);
            TrackSeasonCommand = ReactiveCommand.CreateFromTask(TrackAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading, scheduler: RxApp.MainThreadScheduler);
            _isFavoriteFolderRequesting = RequestFavoriteFoldersCommand.IsExecuting.ToProperty(this, x => x.IsFavoriteFolderRequesting, scheduler: RxApp.MainThreadScheduler);

            ReloadCommand.ThrownExceptions.Subscribe(DisplayException);
            RequestFavoriteFoldersCommand.ThrownExceptions.Subscribe(DisplayFavoriteFoldersException);

            this.WhenAnyValue(p => p.CurrentSection)
                .WhereNotNull()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => CheckSectionVisibility());

            this.WhenAnyValue(p => p.IsOnlyShowIndex)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(isShow => _settingsToolkit.WriteLocalSetting(Models.Enums.SettingNames.IsOnlyShowIndex, isShow));
        }

        /// <summary>
        /// 设置视频.
        /// </summary>
        /// <param name="snapshot">视频信息.</param>
        public void SetSnapshot(PlaySnapshot snapshot)
        {
            _presetEpisodeId = string.IsNullOrEmpty(snapshot.VideoId)
                ? "0"
                : snapshot.VideoId;
            _presetSeasonId = string.IsNullOrEmpty(snapshot.SeasonId)
                ? "0"
                : snapshot.SeasonId;
            _presetTitle = snapshot.Title;
            _needBiliPlus = snapshot.NeedBiliPlus;
            var defaultPlayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            MediaPlayerViewModel.DisplayMode = snapshot.DisplayMode ?? defaultPlayMode;
            ReloadCommand.Execute().Subscribe();
        }

        private void Reset()
        {
            View = null;
            MediaPlayerViewModel.ClearCommand.Execute().Subscribe();
            ResetOverview();
            ResetOperation();
            ResetCommunityInformation();
            ResetInterop();
            ResetSections();
        }

        private async Task GetDataAsync()
        {
            Reset();
            if (_needBiliPlus && !string.IsNullOrEmpty(_presetEpisodeId))
            {
                var data = await _pgcProvider.GetBiliPlusBangumiInformationAsync(_presetEpisodeId);
                if (data != null)
                {
                    var epId = data.PlayUrl.Split('/').Last();
                    _presetEpisodeId = !string.IsNullOrEmpty(epId) && epId.Contains("ep")
                                ? epId.Replace("ep", string.Empty)
                                : "0";
                    _presetSeasonId = data.SeasonId;
                }
            }

            var proxyPack = _appToolkit.GetProxyAndArea(_presetTitle, false);
            View = await _playerProvider.GetPgcDetailAsync(_presetEpisodeId, _presetSeasonId, proxyPack.Item1, proxyPack.Item2);
            var snapshot = new PlaySnapshot(default, View.Information.Identifier.Id, VideoType.Pgc)
            {
                Title = View.Information.Identifier.Title,
            };
            _appViewModel.AddLastPlayItemCommand.Execute(snapshot).Subscribe();
            _appViewModel.AddPlayRecordCommand.Execute(new PlayRecord(View.Information.Identifier, snapshot)).Subscribe();
            InitializeOverview();
            InitializeOperation();
            InitializeCommunityInformation();
            InitializeSections();
            InitializeInterop();

            MediaPlayerViewModel.SetPgcData(View, CurrentEpisode);
        }
    }
}
