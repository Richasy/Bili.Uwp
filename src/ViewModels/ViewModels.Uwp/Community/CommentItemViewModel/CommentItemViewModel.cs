// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Humanizer;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 评论条目视图模型.
    /// </summary>
    public sealed partial class CommentItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentItemViewModel"/> class.
        /// </summary>
        public CommentItemViewModel(
             ICommunityProvider communityProvider,
             INumberToolkit numberToolkit,
             IResourceToolkit resourceToolkit,
             AppViewModel appViewModel,
             NavigationViewModel navigationViewModel)
        {
            _communityProvider = communityProvider;
            _numberToolkit = numberToolkit;
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;

            ShowCommentDetailCommand = ReactiveCommand.Create(ShowDetail, outputScheduler: RxApp.MainThreadScheduler);
            ToggleLikeCommand = ReactiveCommand.CreateFromTask(ToggleLikeAsync, outputScheduler: RxApp.MainThreadScheduler);
            ShowUserDetailCommand = ReactiveCommand.Create(ShowUserDetail, outputScheduler: RxApp.MainThreadScheduler);
            ClickCommand = ReactiveCommand.Create(Click, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置评论信息.
        /// </summary>
        /// <param name="information">评论信息.</param>
        /// <param name="hightlightUserId">需要高亮的用户Id.</param>
        public void SetInformation(CommentInformation information, string hightlightUserId = null)
        {
            Information = information;
            IsUserHighlight = !string.IsNullOrEmpty(hightlightUserId) && Information.Publisher.User.Id == hightlightUserId;
            InitializeData();
        }

        /// <summary>
        /// 设置显示详情的行为.
        /// </summary>
        /// <param name="action">显示详情（展开评论）的行为.</param>
        public void SetDetailAction(Action<CommentItemViewModel> action)
            => _showCommentDetailAction = action;

        /// <summary>
        /// 设置点击的行为.
        /// </summary>
        /// <param name="action">显示详情（展开评论）的行为.</param>
        public void SetClickAction(Action<CommentItemViewModel> action)
            => _clickAction = action;

        private void InitializeData()
        {
            IsLiked = Information.CommunityInformation.IsLiked;
            LikeCountText = _numberToolkit.GetCountText(Information.CommunityInformation.LikeCount);
            PublishDateText = Information.PublishTime.Humanize();
            var replyCount = Information.CommunityInformation.ChildCommentCount;
            if (replyCount > 0)
            {
                ReplyCountText = string.Format(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.MoreReplyDisplay), replyCount);
            }
        }

        private void ShowDetail()
            => _showCommentDetailAction?.Invoke(this);

        private void Click()
            => _clickAction?.Invoke(this);

        private async Task ToggleLikeAsync()
        {
            var isLike = !IsLiked;
            var result = await _communityProvider.LikeCommentAsync(isLike, Information.Id, Information.CommentId, Information.CommentType);
            if (result)
            {
                IsLiked = isLike;
                if (isLike)
                {
                    Information.CommunityInformation.LikeCount += 1;
                }
                else
                {
                    Information.CommunityInformation.LikeCount -= 1;
                }

                LikeCountText = _numberToolkit.GetCountText(Information.CommunityInformation.LikeCount);
            }
            else
            {
                _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private void ShowUserDetail()
            => _navigationViewModel.Navigate(PageIds.UserSpace, Information.Publisher.User);
    }
}
