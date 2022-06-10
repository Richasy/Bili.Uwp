// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Community;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel : ViewModelBase, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerPageViewModel"/> class.
        /// </summary>
        public VideoPlayerPageViewModel(
            IPlayerProvider playerProvider,
            IAuthorizeProvider authorizeProvider,
            IFavoriteProvider favoriteProvider,
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            INumberToolkit numberToolkit,
            ISettingsToolkit settingsToolkit,
            AppViewModel appViewModel,
            NavigationViewModel navigationViewModel,
            AccountViewModel accountViewModel,
            CommentPageViewModel commentPageViewModel,
            MediaPlayerViewModel playerViewModel,
            CoreDispatcher dispatcher)
        {
            _playerProvider = playerProvider;
            _authorizeProvider = authorizeProvider;
            _favoriteProvider = favoriteProvider;
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _settingsToolkit = settingsToolkit;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;
            _accountViewModel = accountViewModel;
            _commentPageViewModel = commentPageViewModel;
            _dispatcher = dispatcher;
            MediaPlayerViewModel = playerViewModel;

            Collaborators = new ObservableCollection<UserItemViewModel>();
            Tags = new ObservableCollection<Tag>();
            FavoriteFolders = new ObservableCollection<VideoFavoriteFolderSelectableViewModel>();
            Sections = new ObservableCollection<Models.App.Other.PlayerSectionHeader>();
            RelatedVideos = new ObservableCollection<VideoItemViewModel>();
            VideoParts = new ObservableCollection<VideoIdentifierSelectableViewModel>();
            Seasons = new ObservableCollection<Models.Data.Video.VideoSeason>();
            CurrentSeasonVideos = new ObservableCollection<VideoItemViewModel>();

            IsSignedIn = _authorizeProvider.State == Models.Enums.AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;

            ReloadCommand = ReactiveCommand.CreateFromTask(GetDataAsync, outputScheduler: RxApp.MainThreadScheduler);
            RequestFavoriteFoldersCommand = ReactiveCommand.CreateFromTask(GetFavoriteFoldersAsync, outputScheduler: RxApp.MainThreadScheduler);
            SearchTagCommand = ReactiveCommand.Create<Tag>(SearchTag, outputScheduler: RxApp.MainThreadScheduler);
            ReloadCommunityInformationCommand = ReactiveCommand.CreateFromTask(ReloadCommunityInformationAsync, outputScheduler: RxApp.MainThreadScheduler);
            RequestOnlineCountCommand = ReactiveCommand.CreateFromTask(GetOnlineCountAsync, outputScheduler: RxApp.MainThreadScheduler);
            FavoriteVideoCommand = ReactiveCommand.CreateFromTask(FavoriteVideoAsync, outputScheduler: RxApp.MainThreadScheduler);
            CoinCommand = ReactiveCommand.CreateFromTask<int>(CoinAsync, outputScheduler: RxApp.MainThreadScheduler);
            LikeCommand = ReactiveCommand.CreateFromTask(LikeAsync, outputScheduler: RxApp.MainThreadScheduler);
            TripleCommand = ReactiveCommand.CreateFromTask(TripleAsync, outputScheduler: RxApp.MainThreadScheduler);
            ShareCommand = ReactiveCommand.Create(Share, outputScheduler: RxApp.MainThreadScheduler);
            FixedCommand = ReactiveCommand.CreateFromTask(FixAsync, outputScheduler: RxApp.MainThreadScheduler);

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
        /// 设置视频 Id.
        /// </summary>
        /// <param name="snapshot">视频 Id.</param>
        public void SetSnapshot(PlaySnapshot snapshot)
        {
            _presetVideoId = snapshot.VideoId;
            MediaPlayerViewModel.DisplayMode = snapshot.DisplayMode;
            ReloadCommand.Execute().Subscribe();
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

            InitializePublisher();
            InitializeOverview();
            InitializeOperation();
            InitializeCommunityInformation();
            InitializeInterop();
            InitializeSections();

            MediaPlayerViewModel.SetData(View, Models.Enums.VideoType.Video);
        }
    }
}
