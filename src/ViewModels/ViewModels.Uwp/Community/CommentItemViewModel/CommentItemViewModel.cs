// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;
using Humanizer;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 评论条目视图模型.
    /// </summary>
    public sealed partial class CommentItemViewModel : ViewModelBase, ICommentItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentItemViewModel"/> class.
        /// </summary>
        public CommentItemViewModel(
             ICommunityProvider communityProvider,
             INumberToolkit numberToolkit,
             IResourceToolkit resourceToolkit,
             ICallerViewModel callerViewModel,
             INavigationViewModel navigationViewModel)
        {
            _communityProvider = communityProvider;
            _numberToolkit = numberToolkit;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;
            _navigationViewModel = navigationViewModel;

            ShowCommentDetailCommand = new RelayCommand(ShowDetail);
            ToggleLikeCommand = new AsyncRelayCommand(ToggleLikeAsync);
            ShowUserDetailCommand = new RelayCommand(ShowUserDetail);
            ClickCommand = new RelayCommand(Click);
        }

        /// <inheritdoc/>
        public void InjectData(CommentInformation information)
        {
            Data = information;
            InitializeData();
        }

        /// <inheritdoc/>
        public void SetDetailAction(Action<ICommentItemViewModel> action)
            => _showCommentDetailAction = action;

        /// <inheritdoc/>
        public void SetClickAction(Action<ICommentItemViewModel> action)
            => _clickAction = action;

        private void InitializeData()
        {
            IsLiked = Data.CommunityInformation.IsLiked;
            LikeCountText = _numberToolkit.GetCountText(Data.CommunityInformation.LikeCount);
            PublishDateText = Data.PublishTime.Humanize();
            var replyCount = Data.CommunityInformation.ChildCommentCount;
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
            var result = await _communityProvider.LikeCommentAsync(isLike, Data.Id, Data.CommentId, Data.CommentType);
            if (result)
            {
                IsLiked = isLike;
                if (isLike)
                {
                    Data.CommunityInformation.LikeCount += 1;
                }
                else
                {
                    Data.CommunityInformation.LikeCount -= 1;
                }

                LikeCountText = _numberToolkit.GetCountText(Data.CommunityInformation.LikeCount);
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private void ShowUserDetail()
            => _navigationViewModel.Navigate(PageIds.UserSpace, Data.Publisher.User);
    }
}
