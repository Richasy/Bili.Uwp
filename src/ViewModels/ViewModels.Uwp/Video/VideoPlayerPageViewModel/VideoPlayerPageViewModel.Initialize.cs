// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.ViewModels.Uwp.Account;
using Humanizer;
using Splat;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private void InitializePublisher()
        {
            IsCooperationVideo = View.Information.Collaborators != null;
            if (IsCooperationVideo)
            {
                foreach (var profile in View.Information.Collaborators)
                {
                    var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
                    userVM.SetProfile(profile);
                    Collaborators.Add(userVM);
                }
            }
            else
            {
                var myId = _authorizeProvider.CurrentUserId;
                var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
                userVM.SetProfile(View.Information.Publisher);
                userVM.Relation = View.PublisherCommunityInformation.Relation;
                userVM.IsRelationButtonShown = !string.IsNullOrEmpty(myId)
                    && myId != View.Information.Publisher.User.Id;
            }
        }

        private void InitializeOverview()
        {
            PublishTime = View.Information.PublishTime.Humanize();
            var communityInfo = View.Information.CommunityInformation;
            PlayCountText = _numberToolkit.GetCountText(communityInfo.PlayCount);
            DanmakuCountText = _numberToolkit.GetCountText(communityInfo.DanmakuCount);
            CommentCountText = _numberToolkit.GetCountText(communityInfo.CommentCount);
            WatchingCountText = "--";

            if (View.Tags != null)
            {
                View.Tags.ToList().ForEach(p => Tags.Add(p));
            }

            IsShowTags = Tags.Count > 0;
        }

        private void InitializeOperation()
        {
            var communityInfo = View.Information.CommunityInformation;
            LikeCountText = _numberToolkit.GetCountText(communityInfo.LikeCount);
            CoinCountText = _numberToolkit.GetCountText(communityInfo.CoinCount);
            FavoriteCountText = _numberToolkit.GetCountText(communityInfo.FavoriteCount);

            IsLiked = View.Operation.IsLiked;
            IsCoined = View.Operation.IsCoined;
            IsFavorited = View.Operation.IsFavorited;
        }
    }
}
