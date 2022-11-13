// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Desktop.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private void ResetOverview()
        {
            IsError = false;
            TryClear(Celebrities);
            IsShowCelebrities = false;
        }

        private void ResetOperation()
        {
            IsLiked = false;
            IsCoined = false;
            IsFavorited = false;
            IsTracking = false;
            FavoriteFoldersErrorText = default;
            IsFavoriteFoldersError = false;
            TryClear(FavoriteFolders);
        }

        private void ResetCommunityInformation()
        {
            PlayCountText = default;
            DanmakuCountText = default;
            CommentCountText = default;
            LikeCountText = default;
            CoinCountText = default;
            FavoriteCountText = default;
            RatingCountText = default;
        }

        private void ResetInterop()
        {
            _playNextEpisodeAction = default;
            MediaPlayerViewModel?.SetPlayNextAction(default);
            IsVideoFixed = false;
        }

        private void ResetSections()
        {
            TryClear(Sections);
            TryClear(Episodes);
            TryClear(Seasons);
            TryClear(Extras);
            CurrentSection = null;
            CurrentEpisode = null;
            IsShowEpisodes = false;
            IsShowSeasons = false;
            IsShowComments = false;
            IsShowExtras = false;
            _commentPageViewModel.ClearData();
        }
    }
}
