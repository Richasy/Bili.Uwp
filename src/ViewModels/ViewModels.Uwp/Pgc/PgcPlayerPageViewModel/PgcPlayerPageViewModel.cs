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
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Common;
using Bili.ViewModels.Uwp.Community;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel : PlayerPageViewModelBase, IPgcPlayerPageViewModel
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
            ICallerViewModel callerViewModel,
            IRecordViewModel recordViewModel,
            IAccountViewModel accountViewModel,
            CommentPageViewModel commentPageViewModel,
            IDownloadModuleViewModel downloadViewModel)
        {
            _playerProvider = playerProvider;
            _authorizeProvider = authorizeProvider;
            _favoriteProvider = favoriteProvider;
            _pgcProvider = pgcProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _settingsToolkit = settingsToolkit;
            _appToolkit = appToolkit;
            _callerViewModel = callerViewModel;
            _recordViewModel = recordViewModel;
            _accountViewModel = accountViewModel;
            _commentPageViewModel = commentPageViewModel;

            FavoriteFolders = new ObservableCollection<IVideoFavoriteFolderSelectableViewModel>();
            Sections = new ObservableCollection<PlayerSectionHeader>();
            Episodes = new ObservableCollection<IEpisodeItemViewModel>();
            Seasons = new ObservableCollection<IVideoIdentifierSelectableViewModel>();
            Extras = new ObservableCollection<IPgcExtraItemViewModel>();
            Celebrities = new ObservableCollection<IUserItemViewModel>();

            DownloadViewModel = downloadViewModel;
            ReloadMediaPlayer();
            IsSignedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;

            ReloadCommand = ReactiveCommand.CreateFromTask(GetDataAsync);
            RequestFavoriteFoldersCommand = ReactiveCommand.CreateFromTask(GetFavoriteFoldersAsync);
            ChangeSeasonCommand = ReactiveCommand.Create<SeasonInformation>(SelectSeason);
            ChangeEpisodeCommand = ReactiveCommand.Create<EpisodeInformation>(SelectEpisode);
            ReloadCommunityInformationCommand = ReactiveCommand.CreateFromTask(RequestEpisodeInteractionInformationAsync);
            FavoriteEpisodeCommand = ReactiveCommand.CreateFromTask(FavoriteVideoAsync);
            CoinCommand = ReactiveCommand.CreateFromTask<int>(CoinAsync);
            LikeCommand = ReactiveCommand.CreateFromTask(LikeAsync);
            TripleCommand = ReactiveCommand.CreateFromTask(TripleAsync);
            ShareCommand = ReactiveCommand.Create(Share);
            FixedCommand = ReactiveCommand.Create(Fix);
            ShowSeasonDetailCommand = ReactiveCommand.Create(ShowSeasonDetail);
            TrackSeasonCommand = ReactiveCommand.CreateFromTask(TrackAsync);

            ReloadCommand.IsExecuting.ToPropertyEx(this, x => x.IsReloading);
            RequestFavoriteFoldersCommand.IsExecuting.ToPropertyEx(this, x => x.IsFavoriteFolderRequesting);

            ReloadCommand.ThrownExceptions.Subscribe(DisplayException);
            RequestFavoriteFoldersCommand.ThrownExceptions.Subscribe(DisplayFavoriteFoldersException);

            this.WhenAnyValue(p => p.CurrentSection)
                .WhereNotNull()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => CheckSectionVisibility());

            PropertyChanged += OnPropertyChanged;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Reset()
        {
            View = null;
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
            _recordViewModel.AddLastPlayItemCommand.Execute(snapshot).Subscribe();
            _recordViewModel.AddPlayRecordCommand.Execute(new PlayRecord(View.Information.Identifier, snapshot)).Subscribe();
            InitializeOverview();
            InitializeOperation();
            InitializeCommunityInformation();
            InitializeSections();
            InitializeInterop();

            MediaPlayerViewModel.SetPgcData(View, CurrentEpisode);
        }

        private void ReloadMediaPlayer()
        {
            MediaPlayerViewModel = Locator.Current.GetService<IMediaPlayerViewModel>();
            MediaPlayerViewModel.MediaEnded += OnMediaEnded;
            MediaPlayerViewModel.InternalPartChanged += OnInternalPartChanged;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Reset();
                    if (MediaPlayerViewModel != null)
                    {
                        MediaPlayerViewModel.MediaEnded -= OnMediaEnded;
                        MediaPlayerViewModel.InternalPartChanged -= OnInternalPartChanged;
                        MediaPlayerViewModel.ClearCommand.Execute().Subscribe();
                        MediaPlayerViewModel = null;
                    }

                    ReloadCommand?.Dispose();
                    RequestFavoriteFoldersCommand?.Dispose();
                    ChangeEpisodeCommand?.Dispose();
                    ChangeSeasonCommand?.Dispose();
                    FavoriteEpisodeCommand?.Dispose();
                    CoinCommand?.Dispose();
                    LikeCommand?.Dispose();
                    TripleCommand?.Dispose();
                    TrackSeasonCommand?.Dispose();
                    ReloadCommunityInformationCommand?.Dispose();
                    ShareCommand?.Dispose();
                    FixedCommand?.Dispose();
                    ShowSeasonDetailCommand?.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
