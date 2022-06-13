// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private void ResetOverview()
        {
            Celebrities.Clear();
            IsShowCelebrities = false;
        }

        private void ResetOperation()
        {
            IsLiked = false;
            IsCoined = false;
            IsFavorited = false;
            IsTracking = false;
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
            RatingCountText = default;
        }

        private void ResetInterop()
        {
            // TODO：重置下载内容.
            IsVideoFixed = false;
        }

        private void ResetSections()
        {
            Sections.Clear();
            Episodes.Clear();
            Seasons.Clear();
            Extras.Clear();
            Seasons.Clear();
            CurrentSection = null;
            IsShowEpisodes = false;
            IsShowSeasons = false;
            IsShowComments = false;
            IsShowExtras = false;
            _commentPageViewModel.ClearData();
        }
    }
}
