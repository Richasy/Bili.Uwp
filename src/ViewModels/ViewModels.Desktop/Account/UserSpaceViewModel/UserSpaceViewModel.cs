// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Local;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Account
{
    /// <summary>
    /// 用户空间视图模型.
    /// </summary>
    public sealed partial class UserSpaceViewModel : InformationFlowViewModelBase<IVideoItemViewModel>, IUserSpaceViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSpaceViewModel"/> class.
        /// </summary>
        public UserSpaceViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            INavigationViewModel navigationViewModel,
            IAccountViewModel accountViewModel,
            ICallerViewModel callerViewModel)
        {
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _navigationViewModel = navigationViewModel;
            _accountViewModel = accountViewModel;
            _callerViewModel = callerViewModel;

            SearchVideos = new ObservableCollection<IVideoItemViewModel>();

            EnterSearchModeCommand = new RelayCommand(EnterSearchMode);
            ExitSearchModeCommand = new RelayCommand(ExitSearchMode);
            SearchCommand = new AsyncRelayCommand(SearchAsync, () => !string.IsNullOrEmpty(Keyword));
            GotoFollowsPageCommand = new RelayCommand(GotoFollowsPage);
            GotoFansPageCommand = new RelayCommand(GotoFansPage);
            FixedCommand = new RelayCommand(Fix);

            AttachExceptionHandlerToAsyncCommand(DisplayException, SearchCommand);
            AttachIsRunningToAsyncCommand(p => IsSearching = p, SearchCommand);
        }

        /// <inheritdoc/>
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
                var userVM = Locator.Instance.GetService<IUserItemViewModel>();
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
            => IsSearchMode = true;

        private void ExitSearchMode()
        {
            IsSearchMode = false;
            Keyword = string.Empty;
            ClearSearchData();
        }

        private void ClearSearchData()
        {
            _requestKeyword = string.Empty;
            _isSearchVideoFinished = false;
            IsSearchVideoEmpty = false;
            _accountProvider.ResetSpaceSearchStatus();
            TryClear(SearchVideos);
        }

        private async Task SearchAsync()
        {
            _requestKeyword = Keyword;
            await RequestSearchVideosAsync();
        }

        private async Task RequestSearchVideosAsync()
        {
            if (string.IsNullOrEmpty(_requestKeyword))
            {
                return;
            }

            var data = await _accountProvider.SearchUserSpaceVideoAsync(_userProfile.Id, _requestKeyword);
            LoadVideoSet(data);
        }

        private void LoadVideoSet(VideoSet set)
        {
            var collection = IsSearchMode ? SearchVideos : Items;
            foreach (var item in set.Items)
            {
                var videoVM = Locator.Instance.GetService<IVideoItemViewModel>();
                videoVM.InjectData(item);
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
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NeedLoginFirst), Models.Enums.App.InfoType.Warning);
                return;
            }

            if (IsFixed)
            {
                _accountViewModel.RemoveFixedItemCommand.ExecuteAsync(UserViewModel.User.Id);
                IsFixed = false;
            }
            else
            {
                _accountViewModel.AddFixedItemCommand.ExecuteAsync(new FixedItem(
                    UserViewModel.User.Avatar.Uri,
                    UserViewModel.User.Name,
                    UserViewModel.User.Id,
                    Models.Enums.App.FixedType.Publisher));
                IsFixed = true;
            }
        }

        partial void OnKeywordChanged(string value)
        {
            CanSearch = !string.IsNullOrEmpty(value);
            ClearSearchData();
            TryClear(SearchVideos);
            if (CanSearch && !IsSearchMode)
            {
                EnterSearchMode();
            }
            else if (!CanSearch && IsSearchMode)
            {
                ExitSearchMode();
            }
        }
    }
}
