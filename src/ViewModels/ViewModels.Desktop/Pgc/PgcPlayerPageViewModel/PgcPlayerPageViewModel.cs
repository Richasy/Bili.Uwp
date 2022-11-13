// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Local;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Pgc
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
            IMediaPlayerViewModel mediaPlayerViewModel,
            ICommentPageViewModel commentPageViewModel,
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

            MediaPlayerViewModel = mediaPlayerViewModel;
            MediaPlayerViewModel.MediaEnded += OnMediaEnded;
            MediaPlayerViewModel.InternalPartChanged += OnInternalPartChanged;
            DownloadViewModel = downloadViewModel;
            IsSignedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;

            ReloadCommand = new AsyncRelayCommand(GetDataAsync);
            RequestFavoriteFoldersCommand = new AsyncRelayCommand(GetFavoriteFoldersAsync);
            ChangeSeasonCommand = new RelayCommand<SeasonInformation>(SelectSeason);
            ChangeEpisodeCommand = new RelayCommand<EpisodeInformation>(SelectEpisode);
            ReloadCommunityInformationCommand = new AsyncRelayCommand(RequestEpisodeInteractionInformationAsync);
            FavoriteEpisodeCommand = new AsyncRelayCommand(FavoriteVideoAsync);
            CoinCommand = new AsyncRelayCommand<int>(CoinAsync);
            LikeCommand = new AsyncRelayCommand(LikeAsync);
            TripleCommand = new AsyncRelayCommand(TripleAsync);
            ShareCommand = new RelayCommand(Share);
            FixedCommand = new RelayCommand(Fix);
            ShowSeasonDetailCommand = new RelayCommand(ShowSeasonDetail);
            TrackSeasonCommand = new AsyncRelayCommand(TrackAsync);
            ClearCommand = new RelayCommand(Reset);

            AttachIsRunningToAsyncCommand(p => IsReloading = p, ReloadCommand);
            AttachIsRunningToAsyncCommand(p => IsFavoriteFolderRequesting = p, RequestFavoriteFoldersCommand);

            AttachExceptionHandlerToAsyncCommand(DisplayException, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayFavoriteFoldersException, RequestFavoriteFoldersCommand);

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
            ReloadCommand.ExecuteAsync(null);
        }

        private void Reset()
        {
            View = null;
            MediaPlayerViewModel.ClearCommand.Execute(null);
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
            _ = _recordViewModel.AddLastPlayItemCommand.ExecuteAsync(snapshot);
            _recordViewModel.AddPlayRecordCommand.Execute(new PlayRecord(View.Information.Identifier, snapshot));
            InitializeOverview();
            InitializeOperation();
            InitializeCommunityInformation();
            InitializeSections();
            InitializeInterop();

            MediaPlayerViewModel.SetPgcData(View, CurrentEpisode);
        }

        partial void OnCurrentSectionChanged(PlayerSectionHeader value)
        {
            if (value != null)
            {
                CheckSectionVisibility();
            }
        }
    }
}
