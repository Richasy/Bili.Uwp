// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private void ResetPublisher()
        {
            IsCooperationVideo = false;
            Author = null;
            Collaborators.Clear();
        }

        private void ResetOverview()
        {
            Tags.Clear();
            IsShowTags = false;
            PublishTime = default;
            PlayCountText = default;
            DanmakuCountText = default;
            CommentCountText = default;
            WatchingCountText = default;
        }

        private void ResetOperation()
        {
            LikeCountText = default;
            CoinCountText = default;
            FavoriteCountText = default;
            IsLiked = false;
            IsCoined = false;
            IsFavorited = false;
        }
    }
}
