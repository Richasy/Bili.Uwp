// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Enums;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 用户条目视图模型.
    /// </summary>
    public sealed partial class UserItemViewModel : ViewModelBase, IUserItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserItemViewModel"/> class.
        /// </summary>
        public UserItemViewModel(
            INumberToolkit numberToolkit,
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            ICallerViewModel callerViewModel,
            INavigationViewModel navigationViewModel,
            IAccountViewModel accountViewModel,
            CoreDispatcher dispatcher)
        {
            _numberToolkit = numberToolkit;
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;
            _navigationViewModel = navigationViewModel;
            _accountViewModel = accountViewModel;
            _dispatcher = dispatcher;

            ToggleRelationCommand = new AsyncRelayCommand(ToggleRelationAsync);
            InitializeRelationCommand = new AsyncRelayCommand(InitializeRelationAsync);
            ShowDetailCommand = new RelayCommand(ShowDetail);

            AttachIsRunningToAsyncCommand(p => IsRelationChanging = p, ToggleRelationCommand);
        }

        /// <summary>
        /// 填充用户信息.
        /// </summary>
        /// <param name="information">用户信息.</param>
        public void SetInformation(AccountInformation information)
        {
            User = information.User;
            Introduce = string.IsNullOrEmpty(information.Introduce)
                ? _resourceToolkit.GetLocaleString(LanguageNames.UserEmptySign)
                : information.Introduce;
            IsVip = information.IsVip;
            Level = information.Level;
            if (information.CommunityInformation != null)
            {
                var communityInfo = information.CommunityInformation;
                Relation = information.CommunityInformation.Relation;
                FollowCountText = _numberToolkit.GetCountText(communityInfo.FollowCount);
                FansCountText = _numberToolkit.GetCountText(communityInfo.FansCount);
                CoinCountText = _numberToolkit.GetCountText(communityInfo.CoinCount);
                LikeCountText = _numberToolkit.GetCountText(communityInfo.LikeCount);
            }

            IsRelationButtonShown = Relation != UserRelationStatus.Unknown;
        }

        /// <summary>
        /// 填充用户资料.
        /// </summary>
        /// <param name="profile">用户资料.</param>
        public void SetProfile(RoleProfile profile)
        {
            if (profile != null)
            {
                User = profile.User;
                Role = profile.Role;
            }
        }

        /// <summary>
        /// 填充用户资料.
        /// </summary>
        /// <param name="profile">用户资料.</param>
        public void SetProfile(UserProfile profile)
            => User = profile;

        private async Task ToggleRelationAsync()
        {
            if (_accountViewModel.State != AuthorizeState.SignedIn)
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.NeedLoginFirst), Models.Enums.App.InfoType.Warning);
                return;
            }

            bool? isFollow = null;
            if (Relation == UserRelationStatus.Unfollow || Relation == UserRelationStatus.BeFollowed)
            {
                // 未关注该用户.
                isFollow = true;
            }
            else if (Relation == UserRelationStatus.Following || Relation == UserRelationStatus.Friends)
            {
                isFollow = false;
            }

            var result = await _accountProvider.ModifyUserRelationAsync(User.Id, isFollow.Value);
            if (result)
            {
                var relation = await _accountProvider.GetRelationAsync(User.Id);
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Relation = relation;
                });
            }
        }

        private async Task InitializeRelationAsync()
        {
            var accountVM = Locator.Instance.GetService<IAccountViewModel>();
            if (accountVM.State != AuthorizeState.SignedIn)
            {
                IsRelationButtonShown = false;
                return;
            }

            var relation = await _accountProvider.GetRelationAsync(User.Id);
            Relation = relation;
            IsRelationButtonShown = Relation != UserRelationStatus.Unknown;
        }

        private void ShowDetail()
            => _navigationViewModel.Navigate(PageIds.UserSpace, User);
    }
}
