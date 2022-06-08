// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Enums;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 用户条目视图模型.
    /// </summary>
    public sealed partial class UserItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserItemViewModel"/> class.
        /// </summary>
        public UserItemViewModel(
            INumberToolkit numberToolkit,
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            AppViewModel appViewModel)
        {
            _numberToolkit = numberToolkit;
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;

            ToggleRelationCommand = ReactiveCommand.CreateFromTask(ToggleRelationAsync, outputScheduler: RxApp.MainThreadScheduler);
            ShowDetailCommand = ReactiveCommand.Create(ShowDetail, outputScheduler: RxApp.MainThreadScheduler);

            _isRelationChanging = ToggleRelationCommand.IsExecuting.ToProperty(
                this,
                x => x.IsRelationChanging,
                scheduler: RxApp.MainThreadScheduler);
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
                Relation = relation;
            }
        }

        private void ShowDetail()
            => _appViewModel.ShowUserDetail(this);
    }
}
