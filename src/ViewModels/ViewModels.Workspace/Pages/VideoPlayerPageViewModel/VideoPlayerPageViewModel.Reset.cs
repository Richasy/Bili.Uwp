// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Workspace.Pages
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private void ResetPublisher()
        {
            IsError = false;
            IsCooperationVideo = false;
            Author = null;
            TryClear(Collaborators);
        }

        private void ResetOverview()
        {
            TryClear(Tags);
            IsShowTags = false;
            PublishTime = default;
            WatchingCountText = default;
        }

        private void ResetOperation()
        {
            IsLiked = false;
            IsCoined = false;
            IsFavorited = false;
            TryClear(FavoriteFolders);
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
            TryClear(Sections);
            TryClear(RelatedVideos);
            TryClear(VideoParts);
            TryClear(Seasons);
            TryClear(CurrentSeasonVideos);
            CurrentSection = null;
            CurrentSeason = null;
            IsShowUgcSeason = false;
            IsShowRelatedVideos = false;
            IsShowComments = false;
            IsShowParts = false;
            IsShowVideoPlaylist = false;
        }

        private void ClearPlaylist()
            => TryClear(VideoPlaylist);
    }
}
