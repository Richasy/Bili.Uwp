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
            WatchingCountText = default;
        }

        private void ResetOperation()
        {
            IsLiked = false;
            IsCoined = false;
            IsFavorited = false;
            FavoriteFolders.Clear();
            FavoriteFoldersErrorText = default;
            IsFavoriteFoldersError = false;
        }

        private void ResetCommunityInformation()
        {
            PlayCountText = default;
            DanmakuCountText = default;
            CommentCountText = default;
            LikeCountText = default;
            CoinCountText = default;
            FavoriteCountText = default;
        }

        private void ResetInterop()
        {
            _playNextVideoAction = default;
            MediaPlayerViewModel.SetPlayNextAction(default);
            IsVideoFixed = false;
        }

        private void ResetSections()
        {
            Sections.Clear();
            RelatedVideos.Clear();
            VideoParts.Clear();
            Seasons.Clear();
            CurrentSeasonVideos.Clear();
            CurrentSection = null;
            CurrentSeason = null;
            IsShowUgcSeason = false;
            IsShowRelatedVideos = false;
            IsShowComments = false;
            IsShowParts = false;
            _commentPageViewModel.ClearData();
        }
    }
}
