// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Account;
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
            AppViewModel appViewModel,
            NavigationViewModel navigationViewModel,
            AccountViewModel accountViewModel,
            CoreDispatcher dispatcher)
        {
            _playerProvider = playerProvider;
            _authorizeProvider = authorizeProvider;
            _favoriteProvider = favoriteProvider;
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;
            _accountViewModel = accountViewModel;
            _dispatcher = dispatcher;

            Collaborators = new ObservableCollection<UserItemViewModel>();
            Tags = new ObservableCollection<Tag>();
            FavoriteFolders = new ObservableCollection<VideoFavoriteFolderSelectableViewModel>();

            IsSignedIn = _authorizeProvider.State == Models.Enums.AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;

            ReloadCommand = ReactiveCommand.CreateFromTask(GetDataAsync, outputScheduler: RxApp.MainThreadScheduler);
            RequestFavoriteFolders = ReactiveCommand.CreateFromTask(GetFavoriteFoldersAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading, scheduler: RxApp.MainThreadScheduler);
            _isFavoriteFolderRequesting = RequestFavoriteFolders.IsExecuting.ToProperty(this, x => x.IsFavoriteFolderRequesting, scheduler: RxApp.MainThreadScheduler);

            ReloadCommand.ThrownExceptions.Subscribe(DisplayException);
            RequestFavoriteFolders.ThrownExceptions.Subscribe(DisplayFavoriteFoldersException);
        }

        /// <summary>
        /// 设置视频 Id.
        /// </summary>
        /// <param name="videoId">视频 Id.</param>
        public void SetViedoId(string videoId)
        {
            _presetVideoId = videoId;
            ReloadCommand.Execute().Subscribe();
        }

        private void Reset()
        {
            View = null;
            ResetPublisher();
            ResetOverview();
            ResetOperation();
        }

        private async Task GetDataAsync()
        {
            Reset();
            View = await _playerProvider.GetVideoDetailAsync(_presetVideoId);

            InitializePublisher();
            InitializeOverview();
            InitializeOperation();
        }

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
            => IsSignedIn = e.NewState == Models.Enums.AuthorizeState.SignedIn;
    }
}
