// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Local;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 用户空间视图模型.
    /// </summary>
    public sealed partial class UserSpaceViewModel : InformationFlowViewModelBase<VideoItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSpaceViewModel"/> class.
        /// </summary>
        public UserSpaceViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            NavigationViewModel navigationViewModel,
            AccountViewModel accountViewModel,
            AppViewModel appViewModel,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _navigationViewModel = navigationViewModel;
            _accountViewModel = accountViewModel;
            _appViewModel = appViewModel;

            SearchVideos = new ObservableCollection<VideoItemViewModel>();

            var canSearch = this.WhenAnyValue(p => p.Keyword)
                .Select(p => !string.IsNullOrEmpty(p));

            EnterSearchModeCommand = ReactiveCommand.Create(EnterSearchMode, outputScheduler: RxApp.MainThreadScheduler);
            ExitSearchModeCommand = ReactiveCommand.Create(ExitSearchMode, outputScheduler: RxApp.MainThreadScheduler);
            SearchCommand = ReactiveCommand.CreateFromTask(SearchAsync, canSearch, RxApp.MainThreadScheduler);
            GotoFollowsPageCommand = ReactiveCommand.Create(GotoFollowsPage, outputScheduler: RxApp.MainThreadScheduler);
            GotoFansPageCommand = ReactiveCommand.Create(GotoFansPage, outputScheduler: RxApp.MainThreadScheduler);
            FixedCommand = ReactiveCommand.Create(Fix, outputScheduler: RxApp.MainThreadScheduler);

            _isSearching = SearchCommand.IsExecuting.ToProperty(this, x => x.IsSearching, scheduler: RxApp.MainThreadScheduler);
            _canSearch = canSearch.ToProperty(this, x => x.CanSearch, scheduler: RxApp.MainThreadScheduler);
            SearchCommand.ThrownExceptions.Subscribe(ex => DisplayException(ex));

            this.WhenAnyValue(p => p.Keyword)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => IsSearchMode = !string.IsNullOrEmpty(x));
        }

        /// <summary>
        /// 设置用户信息.
        /// </summary>
        /// <param name="user">用户资料.</param>
        public void SetUserProfile(UserProfile user)
        {
            _userProfile = user;
            IsMe = user.Id == _accountProvider.UserId.ToString();
            TryClear(Items);
            BeforeReload();
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            UserViewModel = null;
            _isSpaceVideoFinished = false;
            IsSpaceVideoEmpty = false;
            ExitSearchMode();
            _accountProvider.ResetSpaceSearchStatus();
            _accountProvider.ResetSpaceVideoStatus();
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (IsRequestModeFinished())
            {
                return;
            }

            if (UserViewModel == null || !UserViewModel.User.Equals(_userProfile))
            {
                // 请求用户数据.
                var view = await _accountProvider.GetUserSpaceInformationAsync(_userProfile.Id);
                var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
                userVM.SetInformation(view.Account);
                UserViewModel = userVM;
                LoadVideoSet(view.VideoSet);
            }
            else
            {
                if (IsSearchMode)
                {
                    await RequestSearchVideosAsync();
                }
                else
                {
                    var videoSet = await _accountProvider.GetUserSpaceVideoSetAsync(_userProfile.Id);
                    LoadVideoSet(videoSet);
                }
            }
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestUserInformationFailed)}\n{errorMsg}";

        private void EnterSearchMode()
        {
            IsSearchMode = true;
            ClearSearchData();
        }

        private void ExitSearchMode()
        {
            IsSearchMode = false;
            ClearSearchData();
        }

        private void ClearSearchData()
        {
            Keyword = string.Empty;
            _requestKeyword = string.Empty;
            _isSearchVideoFinished = false;
            IsSearchVideoEmpty = false;
            TryClear(SearchVideos);
        }

        private async Task SearchAsync()
        {
            _requestKeyword = Keyword;
            await RequestSearchVideosAsync();
        }

        private async Task RequestSearchVideosAsync()
        {
            var data = await _accountProvider.SearchUserSpaceVideoAsync(_userProfile.Id, _requestKeyword);
            LoadVideoSet(data);
        }

        private void LoadVideoSet(VideoSet set)
        {
            var collection = IsSearchMode ? SearchVideos : Items;
            foreach (var item in set.Items)
            {
                var videoVM = Splat.Locator.Current.GetService<VideoItemViewModel>();
                videoVM.SetInformation(item);
                collection.Add(videoVM);
            }

            var isFinished = set.TotalCount <= collection.Count;
            if (IsSearchMode)
            {
                IsSearchVideoEmpty = SearchVideos.Count == 0;
                _isSearchVideoFinished = isFinished;
            }
            else
            {
                IsSpaceVideoEmpty = Items.Count == 0;
                _isSpaceVideoFinished = isFinished;
            }
        }

        private bool IsRequestModeFinished()
            => IsSearchMode ? _isSearchVideoFinished : _isSpaceVideoFinished;

        private void GotoFansPage()
            => _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Fans, UserViewModel.User);

        private void GotoFollowsPage()
        {
            if (IsMe)
            {
                _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.MyFollows);
            }
            else
            {
                _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Follows, UserViewModel.User);
            }
        }

        private void Fix()
        {
            if (_accountViewModel.State != Models.Enums.AuthorizeState.SignedIn)
            {
                _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NeedLoginFirst), Models.Enums.App.InfoType.Warning);
                return;
            }

            if (IsFixed)
            {
                _accountViewModel.RemoveFixedItemCommand.Execute(UserViewModel.User.Id).Subscribe();
                IsFixed = false;
            }
            else
            {
                _accountViewModel.AddFixedItemCommand.Execute(new FixedItem(
                    UserViewModel.User.Avatar.Uri,
                    UserViewModel.User.Name,
                    UserViewModel.User.Id,
                    Models.Enums.App.FixedType.Publisher)).Subscribe();
                IsFixed = true;
            }
        }
    }
}
